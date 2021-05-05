using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ShopBridge.Interface;
using ShopBridge.Models;
using ShopBridge.Repository.Context;

namespace ShopBridge.Repository
{
    public class Inventory : IInventory
    {
        string connectionString = "";
        IDbConnection dbConnection;
        private readonly InventoryDataBaseContext _context;
        private IConfiguration _config;

        public Inventory(InventoryDataBaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            // ** get connection string for dapper calling **//
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
            connectionString = configuration.GetConnectionString("Default");
            this.dbConnection = new SqlConnection(connectionString);
        }

        public async Task<object> CreateInventory(InventoryModel request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    request.CreatedDate = DateTime.UtcNow;
                    await _context.InventoryModels.AddRangeAsync(request);
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result > 0 ? request : null;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.Message.ToString();
                }
            }
        }

        public async Task<JArray> GetAllInventories()
        {
            var ItemList = new JArray();
            var items =  await SimpleCRUD.GetListAsync<InventoryModel>(dbConnection, new { IsActive =true });
            return ItemList = items.Count() > 0 ? JArray.FromObject(items.OrderBy(a => a.Name)) : new JArray();
        }

        public async Task<object> GetInventoryById(int InvntId)
        {
            var itemList = await SimpleCRUD.GetListAsync<InventoryModel>(dbConnection, new { ProductId= InvntId });
            return itemList.Count() > 0 ? JArray.FromObject(itemList.OrderByDescending(a => a.CreatedDate)) : new JArray(); ;
        }

        public async Task<bool> IsInventoryItemExist(int InvntId)
        {
            bool isExist = false;
            if (InvntId!=0)
            {
                var company = await SimpleCRUD.GetListAsync<InventoryModel>(dbConnection, new { ProductId = InvntId });
                if (company.Count() > 0)
                    isExist = true;
            }
            return isExist;
        }

        public async Task<object> UpdateInventory(InventoryModel request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                  
                    _context.Update(request);
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result > 0 ? request : null;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.Message.ToString();
                }
            }
        }

        public async Task<int> DeleteInventory(int InvntId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                 var itemRecord = await SimpleCRUD.GetListAsync<InventoryModel>(dbConnection, new { Id = InvntId });
                _context.Remove(itemRecord);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    await transaction.CommitAsync();
                else
                    await transaction.RollbackAsync();
                return InvntId;
            }
        }
    }
}

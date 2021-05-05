using Newtonsoft.Json.Linq;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Interface
{
    public interface IInventory
    {
        Task<JArray> GetAllInventories();
        Task<object> GetInventoryById(int InvntId);
        Task<bool> IsInventoryItemExist(int InvntId);
        Task<object> CreateInventory(InventoryModel request);
        Task<object> UpdateInventory(InventoryModel request);
        Task<int> DeleteInventory(int InvntId);

    }
}

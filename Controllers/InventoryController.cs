using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ShopBridge.Interface;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBridge.Common;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _iInventoryItem;

        public InventoryController(IInventory IInventoryItem)
        {
            _iInventoryItem = IInventoryItem;
        }
        /// <summary>
        /// CreateInventory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateInventory")]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(CommonResponse.ReturnResponse (false, 499, new object(), new JArray(), "Request Must Be Not Null,Please Check Your Request Object.", 0));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(CommonResponse.ReturnResponse(false, 499, ModelState, new JArray(), "Request Is Not Valid,Please Check Your Request Object.", 0));
                }

                //if(request!= null)
                //{
                //    return Ok(CommonResponse.ReturnResponse(false, 499, new object(), new JArray(), "Please Pass Request Values", 0));
                //}

                var result = await _iInventoryItem.CreateInventory(request);
                return Ok(CommonResponse.ReturnResponse(true, 299, result, new JArray(), "Inventory Items(s) Creared Successfully", 1));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CommonResponse.ReturnResponse(false, 599, new object(), new JArray(), ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), 0));
            }
        }

        /// <summary>
        /// UpdateItem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryModel request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(CommonResponse.ReturnResponse(false, 499, new object(), new JArray(), "Request Must Be Not Null,Please Check Your Request Object.", 0));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(CommonResponse.ReturnResponse(false, 499, ModelState, new JArray(), "Request Is Not Valid,Please Check Your Request Object.", 0));
                }

                if (!(_iInventoryItem.IsInventoryItemExist(request.ProductId).Result))
                {
                    return NotFound(CommonResponse.ReturnResponse(false, 400, new object(), new JArray(), $"Item Not Found On This {request.ProductId}.", 0));
                }
                var result = await _iInventoryItem.UpdateInventory(request);

                if (result is not null)
                    return Ok(CommonResponse.ReturnResponse(true, 299, result, new JArray(), "Inventory Item Updated Successfully", 1));
                else
                    return Ok(CommonResponse.ReturnResponse(true, 403, result, new JArray(), "Inventory Item Updated Failed", 0));

            }
            catch (Exception ex)
            {
                return StatusCode(500, CommonResponse.ReturnResponse(false, 599, new object(), new JArray(), ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), 0));
            }
        }

        /// <summary>
        /// DeleteInventory
        /// </summary>
        /// <param name="InvntId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteInventory")]
        public async Task<IActionResult> DeleteInventory([FromBody] int InvntId)
        {
            //var response = new object();
            try
            {
                if (InvntId!=0)
                {
                    var result = await _iInventoryItem.DeleteInventory(InvntId);

                    // response = new { ItemIds = deleteRecords.RecordIds };
                    return Ok(CommonResponse.ReturnResponse(true, 299, new object(), JArray.FromObject(result), "Inventory Item(s) Deleted Successfully", InvntId));
                }
                else
                    return Ok(CommonResponse.ReturnResponse(false, 499, new object(), new JArray(), "Must Pass Item Id's", InvntId));

            }
            catch (Exception ex)
            {
                return StatusCode(500, CommonResponse.ReturnResponse(false, 599, new object(), new JArray(), ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), 0));
            }
        }

        /// <summary>
        /// GetInventoryById
        /// </summary>
        /// <param name="InvntId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInventoryById/{InvntId}")]
        public async Task<IActionResult> GetInventoryById(int InvntId)
        {
            try
            {
                if (!(_iInventoryItem.IsInventoryItemExist(InvntId).Result))
                {
                    return NotFound(CommonResponse.ReturnResponse(false, 404, new object(), new JArray(), $"Item Not Found On This {InvntId}.", 0));
                }
                var client = await _iInventoryItem.GetInventoryById(InvntId);
                return Ok(CommonResponse.ReturnResponse(true, 299, client, new JArray(), "Get Inventory Item Successfully", 1));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CommonResponse.ReturnResponse(false, 599, new object(), new JArray(), ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), 0));
            }
        }
        /// <summary>
        /// GetAllInventories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllInventories")]
        public async Task<IActionResult> GetAllInventories()
        {
            try
            {
                var itemList = await _iInventoryItem.GetAllInventories();
                return Ok(CommonResponse.ReturnResponse(true, 299, new object(), itemList, "Get All Inventory Items Successfully", itemList.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CommonResponse.ReturnResponse(false, 599, new object(), new JArray(), ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), 0));
            }
        }
    }
}

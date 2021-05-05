using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Common
{
    public static class CommonResponse
    {
        public static JObject ReturnResponse(bool IsSuccess, int StatusCode, object ValueResult, JArray ListResult, string StatusMessage, int Count)
        {
            JObject response = new JObject();
            response.Add("IsSuccess", IsSuccess);
            response.Add("StatusCode", StatusCode);
            response.Add("ValueResult", JObject.FromObject(ValueResult));
            response.Add("ListResult", ListResult);
            response.Add("StatusMessage", StatusMessage);
            response.Add("Count", Count);
            return response;
        }
    }
}

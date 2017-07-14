using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Key_HiMSWebProject.Controllers
{
    // A static controller for talking with the API
    public static class DataRequestController
    {
        private static HttpClient client = new HttpClient();
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        // Initialize the HTTP Client with the base Data URL and Headers
        static DataRequestController()
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["dataURL"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Makes an asyncronous request to the API, getting JSON from the path specificed.
        private static async Task<string> Request(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            string json = "";

            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
            }

            return json;
        }

        // Generalized get method that returns a serialized object list from JSON response.
        public static async Task<List<dataType>> GetData<dataType>(string path)
        {
            string json = await Request(path);

            // Allows the application to "gracefully" fail if there was an error getting data.
            // It will just return an empty list. 
            if(json.Length < 1)
            {
                return new List<dataType>();
            }
  
            return serializer.Deserialize<List<dataType>>(json);
        }
    }
}
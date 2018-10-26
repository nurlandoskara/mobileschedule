using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileSchedule
{
    public class DataService
    {
        public static async Task<dynamic> GetData(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }
    }
}

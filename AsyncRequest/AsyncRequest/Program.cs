using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncRequest
{
    class Program
    {
        static readonly int startID = 4;
        static readonly int stopID = 13;
        static readonly string fileName = "result.txt";
        static readonly HttpClient client = new HttpClient();

        static void Main()
        {
            JsonSerializer serializer = new JsonSerializer();
            for (int i = startID; i <= stopID; i++)
            {
                var response = RequestResult(i);
                JsonTextReader reader = new JsonTextReader(new StringReader(response.Result.ToString()));
                SinglePost singlePost = serializer.Deserialize<SinglePost>(reader);
                File.AppendAllLines(fileName, singlePost.ToStringList());
                File.AppendAllText(fileName, "\n");
            }
        }

        static async Task<string> RequestResult(int id)
        {
            string request = "https://jsonplaceholder.typicode.com/posts/" + id.ToString();
            try
            {
                HttpResponseMessage response = await client.GetAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception :{0} ", e.Message);
                return null;
            }
        }
    }
}

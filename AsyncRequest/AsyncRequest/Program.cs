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

        static async Task Main()
        {
            var posts = new List<Task<string>>();
            for (int i = startID; i <= stopID; i++)
            {
                posts.Add(RequestResult(i));
            }

            await Task.WhenAll(posts);

            for (int i = 0; i < posts.Count(); i++)
            {
                SinglePost singlePost = JsonConvert.DeserializeObject<SinglePost>(await posts[i]);
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

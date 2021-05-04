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
            var postsStrings = new List<Task<string>>();
            for (int i = startID; i <= stopID; i++)
            {
                postsStrings.Add(RequestPost(i));
            }

            await Task.WhenAll(postsStrings);

            var posts = new List<SinglePost>();            

            foreach (var singlePost in postsStrings)
            {
                posts.Add(JsonConvert.DeserializeObject<SinglePost>(await singlePost));
            }

            posts.Sort();

            foreach (var post in posts)
            {
                File.AppendAllLines(fileName, post.ToStringList());
                File.AppendAllText(fileName, "\n");
            }
        }

        static async Task<string> RequestPost(int id)
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

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

            string[] postsStringReceived = await Task.WhenAll(postsStrings);

            var posts = new List<SinglePost>();
            foreach (var singlePost in postsStringReceived)
            {
                try
                {
                    posts.Add(JsonConvert.DeserializeObject<SinglePost>(singlePost));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Json converting error :" + ex.Message);
                    return;
                }
            }

            var sortedPosts = posts.OrderBy(p => p.id);

            foreach (var post in sortedPosts)
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
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Exception :{0} ", ex.Message);
                return null;
            }
        }
    }
}

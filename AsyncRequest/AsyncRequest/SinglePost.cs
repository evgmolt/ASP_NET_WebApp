using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncRequest
{
    public class SinglePost 
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }

        public List<string> ToStringList()
        {
            List<string> resultList = new List<string>();
            resultList.Add(userId.ToString());
            resultList.Add(id.ToString());
            resultList.Add(title);
            resultList.Add(body);
            return resultList;
        }
    }
}

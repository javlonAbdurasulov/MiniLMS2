using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Client
{
    public class MynewClient : IMynewClient
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://getpantry.cloud/")
        };
        public async Task<string> GetFreeApi()
        {
            string outPut = "ss";
            return outPut;
        }

    }
}

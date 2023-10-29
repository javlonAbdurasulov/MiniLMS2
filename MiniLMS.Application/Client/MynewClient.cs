using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Client
{
    public class MynewClient : IMynewClient
    {
        private readonly HttpClient _httpClient;
        public MynewClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetFreeApi()
        {
            
            return 
                await _httpClient.
                GetStringAsync(_httpClient.BaseAddress);
                
        }

    }
}

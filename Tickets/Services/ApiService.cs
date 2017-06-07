using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tickets.Models;

namespace Tickets.Services
{
    public class ApiService
    {

        public async Task<User> Login(string urlBase, string servicePrefix,
                                          string controller, User model){
            User user = null;
            try{
                //var request = JsonConvert.SerializeObject(model);

                var values = new Dictionary<string, string>{
                    {"Email", model.Email},
                    {"Password", model.Password}
                };

                var request = new FormUrlEncodedContent(values);

				//var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, request);

                if (!response.IsSuccessStatusCode)
                {
                    return user;
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<User>(result);
                user = newRecord;

                return user;
            }catch(Exception e){
                return user;

            }
            
        }
    }
}

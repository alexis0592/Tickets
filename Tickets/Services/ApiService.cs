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

        public async Task<Response> Login(string urlBase, string servicePrefix,
                                          string controller, Dictionary<string , string> model){
            User user = null;
            try{

                var request = new FormUrlEncodedContent(model);

				//var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, request);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<User>(result);
                user = newRecord;

                return new Response
                {
                    IsSuccess = true,
                    Message = "Login OK",
                    Result = newRecord
                };
            }catch(Exception e){
                return new Response
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
            
        }
    }
}

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

        public async Task<Response> Post<T>(string urlBase, string servicePrefix,
                                          string controller, T model){
            try{
                var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Response OK",
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


        public async Task<Response> GetTicket<T>(string urlBase, string servicePrefix,
                                                 string controller, string ticketCode){
            try{

				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}{2}", servicePrefix, controller, ticketCode);
                var response = await client.GetAsync(url);

                if(!response.IsSuccessStatusCode){
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var ticketResponse = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = ticketResponse
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

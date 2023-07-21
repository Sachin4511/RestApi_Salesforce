using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestApi_Salesforce
{
     class salesforce
    {
        private const string Login_Endpoint = "https://login.salesforce.com/services/oauth2/token";
        private const string Api_Endpoint = "/Services/data/v51.0/";

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        static salesforce()
        {
            // connectivity secured
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        public void login()
        {
            String jsonResponse;
            using (var Client = new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type",Password },
                    {"client_id",ClientId },
                    {"Client_Secret",ClientSecret }, 
                    {"Username",Username},
                    {"password",Password+Token }
                });

                request.Headers.Add("X-PreetyPrint", "1");
                var response = Client.PostAsync(Login_Endpoint, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }

            //Console.WriteLine($"Response:{jsonResponse}");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
            Console.WriteLine("AuthToken = " + AuthToken);
            Console.WriteLine("InstanceUrl = " + InstanceUrl);

        } //login function ends
        
        public string Query(string soqlQuery)
        {
            using (var client = new HttpClient())
            {
                string restRequest = InstanceUrl + Api_Endpoint + "query?q=" + soqlQuery;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PreetyPrint", "1");

                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }



    }
}

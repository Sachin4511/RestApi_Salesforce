using System;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace RestApi_Salesforce
{
    class Program
    {
        private static salesforce CreateClient()
        {
            return new salesforce
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"],
                ClientId = ConfigurationManager.AppSettings["clientId"],
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"],
            };
        }
             static void Main(string[] args)
            {
                var client=CreateClient();
                client.login();
                Console.WriteLine(client.Query("select name from Account"));
            }
        
    }
}

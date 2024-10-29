using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MLB_App.Utils
{
    public class ApiCall
    {
        public string PrepareApiCall(string url, string method)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["apiKey"];
                RestClient restClient = new RestClient(url);
                RestRequest request = GetMethod(method);
                request.Resource = url;
                request.AddHeader("x-rapidapi-key", key);
                request.AddHeader("x-rapidapi-host", "tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com");

                var response = restClient.Execute(request);

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Content;
                }

                return System.Net.HttpStatusCode.BadRequest.ToString();
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public RestRequest GetMethod(string method)
        {
            if (String.IsNullOrEmpty(method)) method = "GET";

            method = method.ToUpper();

            var request = new RestRequest();

            switch (method)
            {
                case "GET":
                    request.Method = Method.Get;
                    break;

                case "POST":
                    request.Method = Method.Post;
                    break;

                case "PUT":
                    request.Method = Method.Put;
                    break;

                case "DELETE":
                    request.Method = Method.Delete;
                    break;

                default:
                    request.Method = Method.Get;
                    break;
            }
            return request;
        }

        public string GetApiUrl(string url)
        {
            string apiUrl = ConfigurationManager.AppSettings[url];
            if (!String.IsNullOrEmpty(apiUrl))
            {
                return apiUrl;
            }

            return ConfigurationManager.AppSettings[0];
        }

        public string CallApiService(string[] urls)
        {
            string apiUrl = GetApiUrl(urls[0]);

            foreach(string url in urls)
            {
                if(!apiUrl.Contains(url))
                    apiUrl += url;
            }

            return PrepareApiCall(apiUrl, "GET");
        }

        public static string EncryptApiKey(string key)
        {

            byte[] myBytes = Encoding.UTF8.GetBytes(key);
            SHA256 mySha = SHA256.Create();
            var hash = HexStringFromBytes(mySha.ComputeHash(myBytes, 0, myBytes.Length));

            return hash;
            
        }

        public static string HexStringFromBytes(IEnumerable<byte> bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();

        }
    }
}
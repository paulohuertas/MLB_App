using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
    }
}
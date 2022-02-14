using AuctionSiteDataRetrieval.Entities;
using AuctionSiteDataRetrieval.Models;
using AuctionSiteDataRetrieval.Services.Authentication;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AuctionSiteDataRetrieval.Services.WebServices
{
    public class AuctionWebServices
    {
        public const string apiUrl =  "http://securityapi-env.eba-3534xsm2.us-east-2.elasticbeanstalk.com/api/";
        IAuthentication _authentication;
        public AuctionWebServices(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        public void UpsertAuctionItems(List<AuctionItemDto> auctionItems)
        {

            //string apiUrl = apiUrl;// _configuration.GetValue<string>("APIURL");
            var url = apiUrl + "AuctionItems";
            //var url = "https://localhost:5001/api/" + "AuctionItems";
            
            _authentication.AuthenticationToken();// _configuration);


            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Accept", "application/json");
            _authentication.SetBearerTokenRest(request);//, _configuration);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(auctionItems), ParameterType.RequestBody);
            IRestResponse response = client.ExecuteAsync(request).Result;


        }

        public void UpsertAuctionSearchSiteRun(AuctionSearchSiteRunDto auctionSearchSiteRunDto)
        {

            //string apiUrl = apiUrl;// _configuration.GetValue<string>("APIURL");
             var url = apiUrl + "AuctionSearchSiteRuns";
           // var url = "https://localhost:5001/api/" + "AuctionSearchSiteRuns";

            _authentication.AuthenticationToken();// _configuration);


            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Accept", "application/json");
            _authentication.SetBearerTokenRest(request);//, _configuration);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(auctionSearchSiteRunDto), ParameterType.RequestBody);
            IRestResponse response = client.ExecuteAsync(request).Result;


        }

        public List<AuctionSite> GetAuctionSites()
        {

            List<AuctionSite> auctionSites = new List<AuctionSite>();
            using (var client = new HttpClient())
            {

                var url = "http://securityapi-env.eba-3534xsm2.us-east-2.elasticbeanstalk.com/api/" + "AuctionSites";

                client.DefaultRequestHeaders
                   .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                _authentication.SetBearerToken(client);//, _configuration);
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    try
                    {
                        auctionSites = JsonConvert.DeserializeObject<List<AuctionSite>>(responseString);
                    }
                    catch (Exception ex)
                    {
                    }
                    Console.WriteLine(responseString);
                }
            }

            return auctionSites;


        }


        public List<AuctionSearchWord> GetAuctionSearchWords()
        {

            List<AuctionSearchWord> auctionSearchWords = new List<AuctionSearchWord>();
            using (var client = new HttpClient())
            {

                var url = apiUrl + "AuctionSearchWords";

                client.DefaultRequestHeaders
                   .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                _authentication.SetBearerToken(client);//, _configuration);
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    try
                    {
                        auctionSearchWords = JsonConvert.DeserializeObject<List<AuctionSearchWord>>(responseString);
                    }
                    catch (Exception ex)
                    {
                    }
                    Console.WriteLine(responseString);
                }
            }

            return auctionSearchWords;


        }




    }
}

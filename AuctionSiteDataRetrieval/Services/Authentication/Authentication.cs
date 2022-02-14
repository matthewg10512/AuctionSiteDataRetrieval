using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AuctionSiteDataRetrieval.Services.Authentication
{
   public class Authentication : IAuthentication
    {
        Label _bearerToken;
        TextBox _txtSecurityCode;
        public Authentication(Label bearerToken,TextBox txtSecurityCode)
        {
            _bearerToken = bearerToken;
            _txtSecurityCode = txtSecurityCode;
        }

        public void SetBearerToken(HttpClient client)//, IConfiguration configuration)
        {
            string bearerToken = _bearerToken.Text;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        public void SetBearerTokenRest(RestRequest request)//, IConfiguration configuration)
        {
            string bearerToken = _bearerToken.Text;

            request.AddHeader("Authorization", "Bearer " + bearerToken);
        }

        public void AuthenticationToken()// IConfiguration configuration)
        {

            string details = "";
            string authInfo;
            var stringDetail = _bearerToken.Text;
            int loopCheck = 4;
            if (stringDetail == "Loading")
            {
                loopCheck = 0;
                while (stringDetail == "Loading" && loopCheck < 4)
                {
                    Thread.Sleep(1000);
                    stringDetail = _bearerToken.Text;
                    loopCheck += 1;
                }
            }
            if (stringDetail == null)
            {
                _bearerToken.Text = "Loading";
                authInfo = GetNewAuth();// configuration);
                _bearerToken.Text = authInfo;
                
            }
            else
            {
                using (var client = new HttpClient())
                {
                    details += "url ";
                    string apiUrl = "http://securityapi-env.eba-3534xsm2.us-east-2.elasticbeanstalk.com/api/";// configuration.GetValue<string>("APIURL");
                    var url = apiUrl + "securities/" + 251;
                    details += "urlHit";
                    SetBearerToken(client);//, configuration);
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {

                        authInfo = GetNewAuth();// configuration);

                        _bearerToken.Text = authInfo;

                        
                    }
                }
            }







        }

        public string GetNewAuth()//IConfiguration configuration)
        {
            string apiUrl = "http://securityapi-env.eba-3534xsm2.us-east-2.elasticbeanstalk.com/api/";// configuration.GetValue<string>("APIURL");;//configuration.GetValue<string>("APIURL");
            var clientPost = new RestClient(apiUrl);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest("signin");
            request.AddParameter("Username", CryptoEngine.Decrypt("mcErUNPwh0rO8TdfRQAqjA==",_txtSecurityCode.Text));
            request.AddParameter("Password", CryptoEngine.Decrypt("FxP4trUWtcmUIN8I+8fdlQ==", _txtSecurityCode.Text));
            //request.AddHeader("Content-Type", "application/json");
            var responseString =  clientPost.Post(request);
            
            var contentString = responseString.Content; // Raw content as string
            contentString = contentString.Replace("\"", "");
            return contentString;
        }


    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SugarCRMRestClient
{
    public class SugarClient
    {
        private string _baseUrl;
        private SugarAuthenticator _sugarAuthenticator;
        private SugarOAuthRequest _authenticationRequest;

        public SugarClient(IDictionary<string, string> apikeys)
        {
            _baseUrl = apikeys.FirstOrDefault(x => x.Key == "SugarUrl").Value;
            _authenticationRequest = new SugarOAuthRequest(apikeys);
            _sugarAuthenticator = new SugarAuthenticator(_authenticationRequest, _baseUrl);

        }

        public string CallSugar(string url, object postData = null, Method method = Method.GET)
        {
            TokenCheck();
            var _restClient = new RestClient(_baseUrl);
            _restClient.Authenticator = _sugarAuthenticator;
            var _restRequest = new RestRequest(url, method);
            switch(method)
            {
                case Method.POST:
                    _restRequest.RequestFormat = DataFormat.Json;
                    _restRequest.AddBody(postData);
                    break;
                case Method.PUT:
                    _restRequest.AddHeader("cache-control", "no-cache");
                    _restRequest.AddHeader("content-type", "application/json");
                    _restRequest.AddParameter("application/json", postData, ParameterType.RequestBody);
                    break;
            }

            return parseResponse(_restClient.Execute(_restRequest));
        }

         private string parseResponse(IRestResponse restResponse)
        {
            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return restResponse.Content;
                case HttpStatusCode.InternalServerError:
                    throw new Exception("Server returned InternalServerError : " + restResponse.StatusCode.ToString() + " " + restResponse.Content);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Server returned UnAuthorisedError : " + restResponse.StatusCode.ToString() + " " + restResponse.Content);
                default:
                    throw new Exception(restResponse.StatusCode.ToString() + " " + restResponse.Content);
            }
        }

        private void TokenCheck()
        {
            if (_sugarAuthenticator.IsAccessTokenExpired)
            {
                if (_sugarAuthenticator.IsRefreshTokenExpired)
                {
                    _sugarAuthenticator.FetchOAuthToken();
                }
                else
                {
                    _sugarAuthenticator.UpdateToken();
                }
            }
        }

    }

}
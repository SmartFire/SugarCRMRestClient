using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;

namespace SugarCRMRestClient
{
      public class SugarAuthenticator : IAuthenticator
    {
        private SugarOAuthRequest _authenticationrequest;
        private string _baseUrl;
        private SugarOAuthToken _oauthToken;

        public SugarAuthenticator(SugarOAuthRequest authenticationrequest, string baseUrl)
        {
            _authenticationrequest = authenticationrequest;
            _baseUrl = baseUrl;
            FetchOAuthToken();
        }
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("oauth-token", _oauthToken.Access_Token);
        }

        public void FetchOAuthToken()
        {
            var authClient = new RestClient(_baseUrl);
            var authrequest = new RestRequest("oauth2/token", Method.POST);
            foreach (var item in _authenticationrequest.GetAuthParams())
            {
                authrequest.AddParameter(item.Key, item.Value);
            }
            var returnval = authClient.Execute<SugarOAuthToken>(authrequest);
            if (returnval.StatusCode == HttpStatusCode.OK)
            {
                _oauthToken = returnval.Data;
            }
            else
                throw new Exception(returnval.StatusCode.ToString() + " " + returnval.Content);
        }

        public void UpdateToken()
        {
            var authClient = new RestClient(_baseUrl);
            var authrequest = new RestRequest("oauth2/token", Method.POST);
            _authenticationrequest.grant_type = "refresh_token";
            _authenticationrequest.refresh_token = _oauthToken.Refresh_Token;
            foreach (var item in _authenticationrequest.GetRefreshAuthParams())
            {
                authrequest.AddParameter(item.Key, item.Value);
            }
            var returnval = authClient.Execute<SugarOAuthToken>(authrequest);
            if (returnval.StatusCode == HttpStatusCode.OK)
            {
                _oauthToken = returnval.Data;
            }
            else
                throw new Exception(returnval.StatusCode.ToString() + " " + returnval.Content);
        }

        public bool IsAccessTokenExpired
        {
            get { return _oauthToken.HasAccessTokenExpired(); }
        }

        public bool IsRefreshTokenExpired
        {
            get { return _oauthToken.HasRefreshTokenExpired(); }
        }

    }
}
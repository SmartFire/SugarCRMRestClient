using System;

namespace SugarCRMRestClient
{
    public class SugarOAuthToken
    {
        private int _access_token_expiry;
        private int _refresh_token_expiry;
        public String Access_Token { get; set; }
        public int Expires_In
        {
            get { return _access_token_expiry; }
            set
            {
                _access_token_expiry = value;
                AccessTokenExpiry = DateTime.Now.AddSeconds(_access_token_expiry - 5);
            }
        }
        public String Token_Type { get; set; }
        public String Scope { get; set; }
        public string Refresh_Token { get; set; }
        public int Refresh_Expire_In
        {
            get { return _refresh_token_expiry; }
            set
            {
                _refresh_token_expiry = value;
                RefreshTokenExpiry = DateTime.Now.AddSeconds(_refresh_token_expiry - 5);
            }
        }
        public String Download_Token { get; set; }

        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        public bool HasAccessTokenExpired()
        {
            return DateTime.Now > AccessTokenExpiry;
        }

        public bool HasRefreshTokenExpired()
        {
            return DateTime.Now > RefreshTokenExpiry;
        }

    }
}
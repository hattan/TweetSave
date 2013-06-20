using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace TweetSave.Models
{
    public class TwitterClient
    {
        public string AccessToken { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }

        public async Task<String> SearchAsync(string q)
        {
            await Init();

            string targetUri = "https://api.twitter.com/1.1/search/tweets.json?q=" + q;
            var handler = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = false };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

            HttpResponseMessage httpResponseMessage = await client.GetAsync(targetUri);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        private async Task<bool> Init()
        {
            AccessToken = ConfigurationManager.AppSettings["Twitter:accessToken"];
            if (!string.IsNullOrEmpty(AccessToken)) return true;

            Key = ConfigurationManager.AppSettings["Twitter:key"];
            Secret = ConfigurationManager.AppSettings["Twitter:secret"];
            AuthResponse response = await GetAccessToken();
            AccessToken = response.access_token;
            SaveAccessTokenToWebConfig(AccessToken);
            return true;
        }

        private async Task<AuthResponse> GetAccessToken()
        {
            string url = "https://api.twitter.com/oauth2/token";
            string credential = GetEncodedBearerCredential();
            var handler = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = false };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + credential);

            HttpContent httpContent = new StringContent("grant_type=client_credentials");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage httpResponseMessage = await client.PostAsync(url, httpContent);
            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(response);
            return authResponse;
        }

        private void SaveAccessTokenToWebConfig(string accessToken)
        {
            Configuration config =
                WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            config.AppSettings.Settings.Remove("Twitter:accessToken");
            config.AppSettings.Settings.Add("Twitter:accessToken", accessToken);
            config.Save();
        }

        private string GetEncodedBearerCredential()
        {
            string combined = Key + ":" + Secret;
            return EncodeTo64(combined);
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        internal class AuthResponse
        {
            public string token_type { get; set; }
            public string access_token { get; set; }
        }
    }
}
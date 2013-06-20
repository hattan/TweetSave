using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using TweetSave.Models;

namespace TweetSave.Controllers
{
    public class TwitterController : ApiController
    {
        public async Task<HttpResponseMessage> Get(string q)
        {
            var twitter = new TwitterClient();
            var tweets = await twitter.SearchAsync(q);

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(tweets)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}
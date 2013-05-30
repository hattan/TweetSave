using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TweetSave.Models;

namespace TweetSave.Controllers
{
    public class TweetController : ApiController
    {
        private TweetSaveDataContext db = new TweetSaveDataContext();

        // GET api/Tweet
        public IEnumerable<Tweet> GetTweets()
        {
            return db.Tweets.AsEnumerable();
        }

        // GET api/Tweet/5
        public Tweet GetTweet(string id)
        {
            Tweet tweet = db.Tweets.Find(id);
            if (tweet == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tweet;
        }

        // PUT api/Tweet/5
        public HttpResponseMessage PutTweet(int id, Tweet tweet)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

           
            db.Entry(tweet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Tweet
        public HttpResponseMessage PostTweet(Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                db.Tweets.Add(tweet);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tweet);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tweet.TweetId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Tweet/5
        public HttpResponseMessage DeleteTweet(int id)
        {
            Tweet tweet = db.Tweets.Find(id);
            if (tweet == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Tweets.Remove(tweet);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tweet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
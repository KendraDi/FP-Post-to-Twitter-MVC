using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCSocialSatelliteDB.Models;

namespace MVCSocialSatelliteDB.Controllers
{
    public class TweetsController : Controller
    {
        private SocialSatelliteEntities db = new SocialSatelliteEntities();

        // GET: Tweets
        public ActionResult Index()
        {
            return View(db.Tweet.ToList());
        }


        //delay tweet timestamp (based off Twitter)

        //  public double GenerateTimestamp(DateTime date)
        //{
        //    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    TimeSpan diff = date.ToUniversalTime() - origin;
        //    return Math.Floor(diff.TotalSeconds);
        //}

        public ActionResult Submit(string text)
        {
            var oauth_token = "oauth_token";
            var oauth_token_secret = "oauth_token_secret";
            var oauth_consumer_key = "oauth_consumer_key";
            var oauth_consumer_secret = "oauth_consumer_secret";

            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";
            var oauth_nonce = Convert.ToBase64String(
                                              new ASCIIEncoding().GetBytes(
                                                   DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                                              - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                                                   DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            var resource_url = "https://api.twitter.com/1.1/statuses/update.json";
            var status = text;

            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(status)
                                        );

            baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url),
                         "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                        "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                   "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );

            var postBody = "status=" + Uri.EscapeDataString(status);

            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }
            WebResponse response = request.GetResponse();

            //Console.WriteLine(response.ToString());
            response.Close();
            

            return View();
        }

          
            public ActionResult TweetForm ()
        {
            
            return View();
        }

        // GET: Tweets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = db.Tweet.Find(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }


        // POST: Tweets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TweetID,IsPosted,URL,TimeToPost")] Tweet tweet)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Tweets.Add(tweet);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tweet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TweetID,IsPosted,URL,TimeToPost")] Tweet tweet)
        {
            if (ModelState.IsValid)
            { 
               
                Tweet Post = new Tweet();
                Post = tweet;
                db.Tweet.Add(tweet);
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        
   


        // GET: Tweets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = db.Tweet.Find(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }

        // POST: Tweets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TweetID,IsPosted,URL,TimeToPost")] Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tweet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tweet);
        }

        // GET: Tweets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = db.Tweet.Find(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }

        // POST: Tweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tweet tweet = db.Tweet.Find(id);
            db.Tweet.Remove(tweet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

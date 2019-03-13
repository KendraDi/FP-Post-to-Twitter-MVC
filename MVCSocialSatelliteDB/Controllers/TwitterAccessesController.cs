using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCSocialSatelliteDB.Models;

namespace MVCSocialSatelliteDB.Controllers
{
    public class TwitterAccessesController : Controller
    {
        private SocialSatelliteEntities db = new SocialSatelliteEntities();

        // GET: TwitterAccesses
        public ActionResult Index()
        {
            return View(db.TwitterAccesses.ToList());
        }

        // GET: TwitterAccesses/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitterAccess twitterAccess = db.TwitterAccesses.Find(id);
            if (twitterAccess == null)
            {
                return HttpNotFound();
            }
            return View(twitterAccess);
        }

        // GET: TwitterAccesses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TwitterAccesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID,UserName,AccessToken,AccessTokenSecret,ConsumerKey")] TwitterAccess twitterAccess)
        {
            if (ModelState.IsValid)
            {
                db.TwitterAccesses.Add(twitterAccess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(twitterAccess);
        }

        // GET: TwitterAccesses/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitterAccess twitterAccess = db.TwitterAccesses.Find(id);
            if (twitterAccess == null)
            {
                return HttpNotFound();
            }
            return View(twitterAccess);
        }

        // POST: TwitterAccesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileID,UserName,AccessToken,AccessTokenSecret,ConsumerKey")] TwitterAccess twitterAccess)
        {
            if (ModelState.IsValid)
            {
                db.Entry(twitterAccess).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(twitterAccess);
        }

        // GET: TwitterAccesses/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitterAccess twitterAccess = db.TwitterAccesses.Find(id);
            if (twitterAccess == null)
            {
                return HttpNotFound();
            }
            return View(twitterAccess);
        }

        // POST: TwitterAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TwitterAccess twitterAccess = db.TwitterAccesses.Find(id);
            db.TwitterAccesses.Remove(twitterAccess);
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

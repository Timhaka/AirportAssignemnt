using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirportSystemAssign.Models;

namespace AirportSystemAssign.Controllers
{
    public class AirplaneTypesController : Controller
    {
        private AirportSystemAssignContext db = new AirportSystemAssignContext();

        // GET: AirplaneTypes
        public ActionResult Index()
        {
     
            return View(db.AirplaneTypes.ToList());
        }

        // GET: AirplaneTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneType airplaneType = db.AirplaneTypes.Find(id);
            if (airplaneType == null)
            {
                return HttpNotFound();
            }
            return View(airplaneType);
        }

        // GET: AirplaneTypes/Create
        public ActionResult Create(int? pilotId)
        {
            ViewBag.pilotid = pilotId;
            return View();
        }

        // POST: AirplaneTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AirplaneType airplaneType)
        {
            if (ModelState.IsValid)
            {
                db.AirplaneTypes.Add(airplaneType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airplaneType);
        }

        // GET: AirplaneTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneType airplaneType = db.AirplaneTypes.Find(id);
            if (airplaneType == null)
            {
                return HttpNotFound();
            }
            return View(airplaneType);
        }

        // POST: AirplaneTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AirplaneType airplaneType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airplaneType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airplaneType);
        }

        // GET: AirplaneTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirplaneType airplaneType = db.AirplaneTypes.Find(id);
            if (airplaneType == null)
            {
                return HttpNotFound();
            }
            return View(airplaneType);
        }

        // POST: AirplaneTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AirplaneType airplaneType = db.AirplaneTypes.Find(id);
            //foreach (var item in db.Airplanes)
            //{
            //    if (item.AirplaneType == airplaneType)
            //    {
            //        item.AirplaneType = null;
            //    }
            //}


            db.AirplaneTypes.Remove(airplaneType);
            
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

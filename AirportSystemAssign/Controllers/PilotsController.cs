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
    public class PilotsController : Controller
    {
        private AirportSystemAssignContext db = new AirportSystemAssignContext();

        // GET: Pilots
        public ActionResult Index()
        {
            return View(db.Pilots.ToList());
        }

        // GET: Pilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots
                .Include(s => s.AirplaneTypes)
                .SingleOrDefault(c => c.Id == id);



            if (pilot == null)
            {
                return HttpNotFound();
            }
            return View(pilot);
        }

        // GET: Pilots/Create
        public ActionResult Create(int? airplaneId)
        {

            ViewBag.AirplaneId = airplaneId;
            ViewBag.airplanetypes = db.AirplaneTypes;

            return View();
        }

        // POST: Pilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Pilot pilot, List<string> airtype)
        {
            if (String.IsNullOrEmpty(pilot.Name))
            {

                return Content(@"<script language='javascript' type='text/javascript'> alert('Need to Enter a name!'); window.location = '/Airports/Index';</script>");
            }


            if (ModelState.IsValid)
            {

             


                pilot.AirplaneTypes = new List<AirplaneType>();
                if (airtype != null)
                {
                    foreach (string classid in airtype)
                    {

                        pilot.AirplaneTypes.Add(db.AirplaneTypes.Find(int.Parse(classid)));
                    }
                }




                db.Pilots.Add(pilot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pilot);
        }

        // GET: Pilots/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.airplanetypes = db.AirplaneTypes;
            

            Pilot pilot = db.Pilots.Find(id);
           
            ViewBag.AirplaneTypeselekted = pilot.AirplaneTypes.Select(sc => sc.Id).ToList();
            if (pilot == null)
            {
                return HttpNotFound();
            }
            return View(pilot);
        }

        // POST: Pilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Pilot pilot, List<string> airtype)
        {

            if (String.IsNullOrEmpty(pilot.Name))
            {

                return Content(@"<script language='javascript' type='text/javascript'> alert('Need to Enter a name!'); window.location = '/Airports/Index';</script>");
            }


            if (ModelState.IsValid)
            {

                Pilot dbpilot = db.Pilots.Find(pilot.Id);

                dbpilot.Name = pilot.Name;

                dbpilot.AirplaneTypes.Clear();
                if (airtype != null)
                {
                    foreach (string airplaneId in airtype)
                    {
   
                         dbpilot.AirplaneTypes.Add(db.AirplaneTypes.Find(int.Parse(airplaneId)));

                     }
                }




                //db.Entry(pilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pilot);
        }

        // GET: Pilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);

            //Pilot pilot = db.Pilots.Include(s => s.Airplane).SingleOrDefault(c => c.Id == id);

            List<Airplane> airlist = new List<Airplane>();

            foreach(Airplane item in db.Airplanes)
            {
                airlist.Add(item);
            }
            ViewBag.airlist = airlist.ToList();


            if (pilot == null)
            {
                return HttpNotFound();
            }
            return View(pilot);
        }

        // POST: Pilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pilot pilot = db.Pilots.Find(id);
            db.Pilots.Remove(pilot);
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

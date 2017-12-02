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
    public class AirplanesController : Controller
    {
        private AirportSystemAssignContext db = new AirportSystemAssignContext();

        // GET: Airplanes
        public ActionResult Index()
        {
            var airplanes = db.Airplanes.Include(a => a.AirplaneType).Include(a => a.Airport).Include(a => a.CoPilot).Include(a => a.Pilot);
            
            return View(airplanes.ToList());
        }

        //get
        public ActionResult RemovePilots(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



            Airplane airplane = db.Airplanes
            .Include(s => s.Pilot)
            .Include(t => t.CoPilot)
            .SingleOrDefault(c => c.Id == id);

            ViewBag.Size = airplane.Size;
            ViewBag.maxNumPass = airplane.MaxNrOfPassengers;
            ViewBag.AirplaneId = id;
            ViewBag.Name = airplane.Name;
            ViewBag.Type = airplane.AirplaneTypeId;
            ViewBag.Airport = airplane.AirportId;


            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemovePilots([Bind(Include = "Id,Name,Size,MaxNrOfPassengers,AirportId,PilotId,CoPilotId,AirplaneTypeId")] Airplane airplane, string answer)
        {

            if (ModelState.IsValid)
            {

                switch (answer)
                {
                    case "Yes":
                        airplane.Pilot = null;
                        airplane.CoPilot = null;
                        db.Entry(airplane).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "No":
                        break;
                }



                return RedirectToAction("Index");

            }
         

            return View(airplane);
        }


        //Get
        public ActionResult TransferAirplane(int? id, int? name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes
            .Include(s => s.Airport)
            .Include(t => t.AirplaneType)
            .SingleOrDefault(c => c.Id == id);

            ViewBag.transferAirplaneId = id;
            ViewBag.airplorts = db.Airports;


            airplane.PilotId = null;
            airplane.CoPilotId = null;

            var airportlist = new List<Airport>();
            foreach (Airport item in db.Airports)
            {
                    airportlist.Add(item);
            }
            ViewBag.templist = airportlist.ToList();


            var pilotList = new List<Pilot>();

            foreach (Pilot item in db.Pilots)
            {
                
                    pilotList.Add(item);
            }

            foreach (var item in db.Airplanes)
            {
                if (pilotList.Contains(item.Pilot) && pilotList.Contains(item.CoPilot))
                {
                   

                   pilotList.Remove(item.Pilot);
                   pilotList.Remove(item.CoPilot);
                }
            }

             foreach (Pilot p in db.Pilots)
                    {
                        if (!p.AirplaneTypes.Contains(airplane.AirplaneType))
                        {
                            pilotList.Remove(p);
                        }
                    }


            ViewBag.pilotTempList = pilotList.ToList();

            if (airplane == null)
            {
                return HttpNotFound();
            }


            ViewBag.Size = airplane.Size;
            ViewBag.maxNumPass = airplane.MaxNrOfPassengers;

            return View(airplane);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferAirplane([Bind(Include = "Id,Name,Size,MaxNrOfPassengers,AirportId,PilotId,CoPilotId,AirplaneTypeId")] Airplane airplane)
        {
            if (airplane.PilotId == null || airplane.CoPilotId == null)
            {
                return Content(@"<script language='javascript' type='text/javascript'> alert('Needs to have 2 pilots'); window.location = '/Airports/Index';</script>");
            }
            

            if (ModelState.IsValid)
            {

                if (airplane.PilotId == airplane.CoPilotId)
                {
                    return RedirectToAction("Index");
                }
               
                db.Entry(airplane).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                

            }


            ViewBag.AirplaneTypeId = new SelectList(db.AirplaneTypes, "Id", "Name", airplane.AirplaneTypeId);
            ViewBag.AirportId = new SelectList(db.Airports, "Id", "Name", airplane.AirportId);
            ViewBag.CoPilotId = new SelectList(db.Pilots, "Id", "Name", airplane.CoPilotId);
            ViewBag.PilotId = new SelectList(db.Pilots, "Id", "Name", airplane.PilotId);
            airplane.Size = db.Airplanes.Find(airplane.Id).Size;
            airplane.MaxNrOfPassengers = db.Airplanes.Find(airplane.Id).MaxNrOfPassengers;



            return View(airplane);
        }





        // GET: Airplanes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Airplane airplane = db.Airplanes.Find(id);

            Airplane airplane = db.Airplanes
            .Include(s => s.Airport)
            .Include(t => t.AirplaneType)
            .SingleOrDefault(c => c.Id == id);


            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // GET: Airplanes/Create
        public ActionResult Create(int? airportid)
        {
            ViewBag.AirplaneTypeId = new SelectList(db.AirplaneTypes, "Id", "Name");
            ViewBag.AirportId = airportid;
            ViewBag.CoPilotId = new SelectList(db.Pilots, "Id", "Name");
            ViewBag.PilotId = new SelectList(db.Pilots, "Id", "Name");





            return View();
        }

        // POST: Airplanes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Size,MaxNrOfPassengers,AirportId,PilotId,CoPilotId,AirplaneTypeId")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                db.Airplanes.Add(airplane);
                db.SaveChanges();
                return RedirectToAction("Index", "Airports", new { Id = airplane.AirportId });
            }

            ViewBag.AirplaneTypeId = new SelectList(db.AirplaneTypes, "Id", "Name", airplane.AirplaneTypeId);
            ViewBag.AirportId = new SelectList(db.Airports, "Id", "Name", airplane.AirportId);
            ViewBag.CoPilotId = new SelectList(db.Pilots, "Id", "Name", airplane.CoPilotId);
            ViewBag.PilotId = new SelectList(db.Pilots, "Id", "Name", airplane.PilotId);
            return View(airplane);
        }

        // GET: Airplanes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }


            ViewBag.AirplaneTypeId = new SelectList(db.AirplaneTypes, "Id", "Name", airplane.AirplaneTypeId);
            ViewBag.AirportId = new SelectList(db.Airports, "Id", "Name", airplane.AirportId);
            ViewBag.Size = airplane.Size;
            ViewBag.pilot = airplane.PilotId;
            ViewBag.Copilot = airplane.CoPilotId;
            ViewBag.TempPlanetype = airplane.AirplaneTypeId;
            airplane.Size = db.Airplanes.Find(airplane.Id).Size;
            airplane.MaxNrOfPassengers = db.Airplanes.Find(airplane.Id).MaxNrOfPassengers;

            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Size,MaxNrOfPassengers,AirportId,PilotId,CoPilotId,AirplaneTypeId")] Airplane airplane, int oldairplanetypeId)
        {
            if (ModelState.IsValid)
            {


                if (airplane.AirplaneTypeId == oldairplanetypeId)
                {
                    db.Entry(airplane).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    airplane.PilotId = null;
                    airplane.CoPilotId = null;
                    db.Entry(airplane).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                
            }



            airplane.Size = db.Airplanes.Find(airplane.Id).Size;
            airplane.MaxNrOfPassengers = db.Airplanes.Find(airplane.Id).MaxNrOfPassengers;

            return View(airplane);
        }

        // GET: Airplanes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airplane airplane = db.Airplanes.Find(id);
            if (airplane == null)
            {
                return HttpNotFound();
            }
            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Airplane airplane = db.Airplanes.Find(id);
            db.Airplanes.Remove(airplane);
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

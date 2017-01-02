using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models.DAO;

namespace FYP_MVC.Controllers
{
    public class chartsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: charts
        public ActionResult Index()
        {
            return View(db.charts.ToList());
        }

        // GET: charts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chart chart = db.charts.Find(id);
            if (chart == null)
            {
                return HttpNotFound();
            }
            return View(chart);
        }

        // GET: charts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: charts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name")] chart chart)
        {
            if (ModelState.IsValid)
            {
                db.charts.Add(chart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chart);
        }

        // GET: charts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chart chart = db.charts.Find(id);
            if (chart == null)
            {
                return HttpNotFound();
            }
            return View(chart);
        }

        // POST: charts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name")] chart chart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chart);
        }

        // GET: charts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chart chart = db.charts.Find(id);
            if (chart == null)
            {
                return HttpNotFound();
            }
            return View(chart);
        }

        // POST: charts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chart chart = db.charts.Find(id);
            db.charts.Remove(chart);
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

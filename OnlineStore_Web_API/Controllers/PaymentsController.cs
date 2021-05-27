using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using OnlineStore_Web_API.Models;

namespace OnlineStore_Web_API.Controllers
{
    public class PaymentsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Payments
        [EnableQuery]
        public IQueryable<Payment> GetPayments()
        {
            return db.Payments;
        }

        // GET: odata/Payments(5)
        [EnableQuery]
        public SingleResult<Payment> GetPayment([FromODataUri] int key)
        {
            return SingleResult.Create(db.Payments.Where(payment => payment.paymentID == key));
        }

        // PUT: odata/Payments(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Payment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Payment payment = db.Payments.Find(key);
            if (payment == null)
            {
                return NotFound();
            }

            patch.Put(payment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(payment);
        }

        // POST: odata/Payments
        public IHttpActionResult Post(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Payments.Add(payment);
            db.SaveChanges();

            return Created(payment);
        }

        // PATCH: odata/Payments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Payment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Payment payment = db.Payments.Find(key);
            if (payment == null)
            {
                return NotFound();
            }

            patch.Patch(payment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(payment);
        }

        // DELETE: odata/Payments(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Payment payment = db.Payments.Find(key);
            if (payment == null)
            {
                return NotFound();
            }

            db.Payments.Remove(payment);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Payments(5)/Bills
        [EnableQuery]
        public IQueryable<Bill> GetBills([FromODataUri] int key)
        {
            return db.Payments.Where(m => m.paymentID == key).SelectMany(m => m.Bills);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentExists(int key)
        {
            return db.Payments.Count(e => e.paymentID == key) > 0;
        }
    }
}

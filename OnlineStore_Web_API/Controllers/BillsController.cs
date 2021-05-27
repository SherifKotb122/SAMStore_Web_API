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
    public class BillsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Bills
        [EnableQuery]
        public IQueryable<Bill> GetBills()
        {
            return db.Bills;
        }

        // GET: odata/Bills(5)
        [EnableQuery]
        public SingleResult<Bill> GetBill([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bills.Where(bill => bill.billID == key));
        }

        // PUT: odata/Bills(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Bill> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Bill bill = db.Bills.Find(key);
            if (bill == null)
            {
                return NotFound();
            }

            patch.Put(bill);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bill);
        }

        // POST: odata/Bills
        public IHttpActionResult Post(Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bills.Add(bill);
            db.SaveChanges();

            return Created(bill);
        }

        // PATCH: odata/Bills(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Bill> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Bill bill = db.Bills.Find(key);
            if (bill == null)
            {
                return NotFound();
            }

            patch.Patch(bill);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bill);
        }

        // DELETE: odata/Bills(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Bill bill = db.Bills.Find(key);
            if (bill == null)
            {
                return NotFound();
            }

            db.Bills.Remove(bill);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Bills(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bills.Where(m => m.billID == key).Select(m => m.Address));
        }

        // GET: odata/Bills(5)/Payment
        [EnableQuery]
        public SingleResult<Payment> GetPayment([FromODataUri] int key)
        {
            return SingleResult.Create(db.Bills.Where(m => m.billID == key).Select(m => m.Payment));
        }

        // GET: odata/Bills(5)/BillProducts
        [EnableQuery]
        public IQueryable<BillProduct> GetBillProducts([FromODataUri] int key)
        {
            return db.Bills.Where(m => m.billID == key).SelectMany(m => m.BillProducts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillExists(int key)
        {
            return db.Bills.Count(e => e.billID == key) > 0;
        }
    }
}

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
    public class ShippingsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Shippings
        [EnableQuery]
        public IQueryable<Shipping> GetShippings()
        {
            return db.Shippings;
        }

        // GET: odata/Shippings(5)
        [EnableQuery]
        public SingleResult<Shipping> GetShipping([FromODataUri] int key)
        {
            return SingleResult.Create(db.Shippings.Where(shipping => shipping.shippingID == key));
        }

        // PUT: odata/Shippings(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Shipping> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Shipping shipping = db.Shippings.Find(key);
            if (shipping == null)
            {
                return NotFound();
            }

            patch.Put(shipping);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipping);
        }

        // POST: odata/Shippings
        public IHttpActionResult Post(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shippings.Add(shipping);
            db.SaveChanges();

            return Created(shipping);
        }

        // PATCH: odata/Shippings(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Shipping> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Shipping shipping = db.Shippings.Find(key);
            if (shipping == null)
            {
                return NotFound();
            }

            patch.Patch(shipping);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipping);
        }

        // DELETE: odata/Shippings(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Shipping shipping = db.Shippings.Find(key);
            if (shipping == null)
            {
                return NotFound();
            }

            db.Shippings.Remove(shipping);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Shippings(5)/Addresses
        [EnableQuery]
        public IQueryable<Address> GetAddresses([FromODataUri] int key)
        {
            return db.Shippings.Where(m => m.shippingID == key).SelectMany(m => m.Addresses);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShippingExists(int key)
        {
            return db.Shippings.Count(e => e.shippingID == key) > 0;
        }
    }
}

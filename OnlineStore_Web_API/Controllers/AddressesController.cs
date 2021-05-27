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
    public class AddressesController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Addresses
        [EnableQuery]
        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
        }

        // GET: odata/Addresses(5)
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.Addresses.Where(address => address.addressID == key));
        }

        // PUT: odata/Addresses(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Address> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = db.Addresses.Find(key);
            if (address == null)
            {
                return NotFound();
            }

            patch.Put(address);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(address);
        }

        // POST: odata/Addresses
        public IHttpActionResult Post(Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            db.SaveChanges();

            return Created(address);
        }

        // PATCH: odata/Addresses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Address> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = db.Addresses.Find(key);
            if (address == null)
            {
                return NotFound();
            }

            patch.Patch(address);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(address);
        }

        // DELETE: odata/Addresses(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Address address = db.Addresses.Find(key);
            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Addresses(5)/Shipping
        [EnableQuery]
        public SingleResult<Shipping> GetShipping([FromODataUri] int key)
        {
            return SingleResult.Create(db.Addresses.Where(m => m.addressID == key).Select(m => m.Shipping));
        }

        // GET: odata/Addresses(5)/Bills
        [EnableQuery]
        public IQueryable<Bill> GetBills([FromODataUri] int key)
        {
            return db.Addresses.Where(m => m.addressID == key).SelectMany(m => m.Bills);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int key)
        {
            return db.Addresses.Count(e => e.addressID == key) > 0;
        }
    }
}

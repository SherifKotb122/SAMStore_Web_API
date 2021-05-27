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
    public class StoresController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Stores
        [EnableQuery]
        public IQueryable<Store> GetStores()
        {
            return db.Stores;
        }

        // GET: odata/Stores(5)
        [EnableQuery]
        public SingleResult<Store> GetStore([FromODataUri] int key)
        {
            return SingleResult.Create(db.Stores.Where(store => store.ID == key));
        }

        // PUT: odata/Stores(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Store> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Store store = db.Stores.Find(key);
            if (store == null)
            {
                return NotFound();
            }

            patch.Put(store);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(store);
        }

        // POST: odata/Stores
        public IHttpActionResult Post(Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stores.Add(store);
            db.SaveChanges();

            return Created(store);
        }

        // PATCH: odata/Stores(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Store> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Store store = db.Stores.Find(key);
            if (store == null)
            {
                return NotFound();
            }

            patch.Patch(store);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(store);
        }

        // DELETE: odata/Stores(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Store store = db.Stores.Find(key);
            if (store == null)
            {
                return NotFound();
            }

            db.Stores.Remove(store);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Stores(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Stores.Where(m => m.ID == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreExists(int key)
        {
            return db.Stores.Count(e => e.ID == key) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using OnlineStore_Web_API.Models;

namespace OnlineStore_Web_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CartsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Carts
        [EnableQuery(MaxExpansionDepth =6)]
        public IQueryable<Cart> GetCarts()
        {
            return db.Carts;
        }

        // GET: odata/Carts(5)
        [EnableQuery]
        public SingleResult<Cart> GetCart([FromODataUri] int key)
        {
            return SingleResult.Create(db.Carts.Where(cart => cart.cartID == key));
        }

        // PUT: odata/Carts(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Cart> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cart cart = db.Carts.Find(key);
            if (cart == null)
            {
                return NotFound();
            }

            patch.Put(cart);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cart);
        }

        // POST: odata/Carts
        public IHttpActionResult Post(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carts.Add(cart);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CartExists(cart.cartID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(cart);
        }

        // PATCH: odata/Carts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Cart> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cart cart = db.Carts.Find(key);
            if (cart == null)
            {
                return NotFound();
            }

            patch.Patch(cart);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cart);
        }

        // DELETE: odata/Carts(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Cart cart = db.Carts.Find(key);
            if (cart == null)
            {
                return NotFound();
            }

            db.Carts.Remove(cart);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Carts(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Carts.Where(m => m.cartID == key).Select(m => m.AspNetUser));
        }

        // GET: odata/Carts(5)/Store
        [EnableQuery]
        public SingleResult<Store> GetStore([FromODataUri] int key)
        {
            return SingleResult.Create(db.Carts.Where(m => m.cartID == key).Select(m => m.Store));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int key)
        {
            return db.Carts.Count(e => e.cartID == key) > 0;
        }
    }
}

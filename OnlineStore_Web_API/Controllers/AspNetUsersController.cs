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
    public class AspNetUsersController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/AspNetUsers
        [EnableQuery]
        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            return db.AspNetUsers;
        }

        // GET: odata/AspNetUsers(5)
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.AspNetUsers.Where(aspNetUser => aspNetUser.Id == key));
        }

        // PUT: odata/AspNetUsers(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<AspNetUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            patch.Put(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUser);
        }

        // POST: odata/AspNetUsers
        public IHttpActionResult Post(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(aspNetUser);
        }

        // PATCH: odata/AspNetUsers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<AspNetUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            patch.Patch(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUser);
        }

        // DELETE: odata/AspNetUsers(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/AspNetUsers(5)/Bills
        [EnableQuery]
        public IQueryable<Bill> GetBills([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.Bills);
        }

        // GET: odata/AspNetUsers(5)/WishLists
        [EnableQuery]
        public IQueryable<WishList> GetWishLists([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.WishLists);
        }

        // GET: odata/AspNetUsers(5)/Reviews
        [EnableQuery]
        public IQueryable<Review> GetReviews([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.Reviews);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string key)
        {
            return db.AspNetUsers.Count(e => e.Id == key) > 0;
        }
    }
}

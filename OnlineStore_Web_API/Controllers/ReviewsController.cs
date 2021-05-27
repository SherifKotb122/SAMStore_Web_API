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
    public class ReviewsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/Reviews
        [EnableQuery]
        public IQueryable<Review> GetReviews()
        {
            return db.Reviews;
        }

        // GET: odata/Reviews(5)
        [EnableQuery]
        public SingleResult<Review> GetReview([FromODataUri] int key)
        {
            return SingleResult.Create(db.Reviews.Where(review => review.reviewID == key));
        }

        // PUT: odata/Reviews(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Review> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Review review = db.Reviews.Find(key);
            if (review == null)
            {
                return NotFound();
            }

            patch.Put(review);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(review);
        }

        // POST: odata/Reviews
        public IHttpActionResult Post(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return Created(review);
        }

        // PATCH: odata/Reviews(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Review> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Review review = db.Reviews.Find(key);
            if (review == null)
            {
                return NotFound();
            }

            patch.Patch(review);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(review);
        }

        // DELETE: odata/Reviews(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Review review = db.Reviews.Find(key);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Reviews(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Reviews.Where(m => m.reviewID == key).Select(m => m.AspNetUser));
        }

        // GET: odata/Reviews(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Reviews.Where(m => m.reviewID == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int key)
        {
            return db.Reviews.Count(e => e.reviewID == key) > 0;
        }
    }
}

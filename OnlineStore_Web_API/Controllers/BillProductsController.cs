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
    public class BillProductsController : ODataController
    {
        private OnlineStore_APIEntities db = new OnlineStore_APIEntities();

        // GET: odata/BillProducts
        [EnableQuery]
        public IQueryable<BillProduct> GetBillProducts()
        {
            return db.BillProducts;
        }

        // GET: odata/BillProducts(5)
        [EnableQuery]
        public SingleResult<BillProduct> GetBillProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.BillProducts.Where(billProduct => billProduct.billProductID == key));
        }

        // PUT: odata/BillProducts(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<BillProduct> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BillProduct billProduct = db.BillProducts.Find(key);
            if (billProduct == null)
            {
                return NotFound();
            }

            patch.Put(billProduct);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(billProduct);
        }

        // POST: odata/BillProducts
        public IHttpActionResult Post(BillProduct billProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BillProducts.Add(billProduct);
            db.SaveChanges();

            return Created(billProduct);
        }

        // PATCH: odata/BillProducts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<BillProduct> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BillProduct billProduct = db.BillProducts.Find(key);
            if (billProduct == null)
            {
                return NotFound();
            }

            patch.Patch(billProduct);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(billProduct);
        }

        // DELETE: odata/BillProducts(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            BillProduct billProduct = db.BillProducts.Find(key);
            if (billProduct == null)
            {
                return NotFound();
            }

            db.BillProducts.Remove(billProduct);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BillProducts(5)/Bill
        [EnableQuery]
        public SingleResult<Bill> GetBill([FromODataUri] int key)
        {
            return SingleResult.Create(db.BillProducts.Where(m => m.billProductID == key).Select(m => m.Bill));
        }

        // GET: odata/BillProducts(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.BillProducts.Where(m => m.billProductID == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillProductExists(int key)
        {
            return db.BillProducts.Count(e => e.billProductID == key) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using OnlineStore_Web_API.Models;

namespace OnlineStore_Web_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<WishList>("WishLists");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Shipping>("Shippings");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<BillProduct>("BillProducts");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Class>("Classes");
            builder.EntitySet<Payment>("Payments");
            builder.EntitySet<AspNetUser>("AspNetUsers");
            builder.EntitySet<Review>("Reviews");
            builder.EntitySet<Store>("Stores");
            builder.EntitySet<Bill>("Bills");
            builder.EntitySet<Cart>("Carts");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

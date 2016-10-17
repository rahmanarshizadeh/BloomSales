using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Controllers.Business
{
    public class SessionHandler
    {
        private HttpSessionStateBase session;
        private readonly string CartIndex = "cart";

        public SessionHandler(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public Order Cart
        {
            get { return session[CartIndex] as Order; }
            set { session[CartIndex] = value; }
        }

        public void DeleteCart()
        {
            session[CartIndex] = null;
        }
    }
}
using BloomSales.Data.Entities;
using BloomSales.Web.Store.Models;
using System.Web;

namespace BloomSales.Web.Store.Controllers.Business
{
    public class SessionHandler
    {
        private HttpSessionStateBase session;
        private readonly string CartIndex = "cart";
        private readonly string BillIndex = "bill";
        private readonly string ShippingIndex = "shipping";
        private readonly string ShippingCostIndex = "shippingCost";

        public SessionHandler(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public Order Cart
        {
            get { return session[CartIndex] as Order; }
            set { session[CartIndex] = value; }
        }

        public BillViewModel Bill
        {
            get { return session[BillIndex] as BillViewModel; }
            set { session[BillIndex] = value; }
        }

        public ShippingInfo Shipping
        {
            get { return session[ShippingIndex] as ShippingInfo; }
            set { session[ShippingIndex] = value; }
        }

        public decimal? ShippingCost
        {
            get { return session[ShippingCostIndex] as decimal?; }
            set { session[ShippingCostIndex] = value; }
        }

        public void DeleteCart()
        {
            session[CartIndex] = null;
        }
    }
}
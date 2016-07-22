using BloomSales.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace BloomSales.Host.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceHost hostLocationService;
        private ServiceHost hostInventoryService;
        private ServiceHost hostShippingService;
        private ServiceHost hostAccountingService;
        private ServiceHost hostOrderService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartLocationService()
        {
            this.hostLocationService = new ServiceHost(typeof(LocationService));
            this.hostLocationService.Open();
        }

        private void StopLocationService()
        {
            this.hostLocationService.Close();
        }

        private void StartInventoryService()
        {
            this.hostInventoryService = new ServiceHost(typeof(InventoryService));
            this.hostInventoryService.Open();
        }

        private void StopInventoryService()
        {
            this.hostInventoryService.Close();
        }

        private void StartShippingService()
        {
            this.hostShippingService = new ServiceHost(typeof(ShippingService));
            this.hostShippingService.Open();
        }

        private void StopShippingService()
        {
            this.hostShippingService.Close();
        }

        private void StartAccountingService()
        {
            this.hostAccountingService = new ServiceHost(typeof(AccountingService));
            this.hostAccountingService.Open();
        }

        private void StopAccountingService()
        {
            this.hostAccountingService.Close();
        }

        private void StartOrerService()
        {
            this.hostOrderService = new ServiceHost(typeof(OrderService));
            this.hostOrderService.Open();
        }

        private void StopOrderService()
        {
            this.hostOrderService.Close();
        }

        private void Log(string log)
        {
            this.txtLog.Text +=
                string.Format("[{0}]: {1}{2}", DateTime.Now.ToString(), log, Environment.NewLine);
        }

        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            if (btnLocation.Content.Equals("Start"))
            {
                StartLocationService();
                btnLocation.Content = "Stop";
                Log("Location service started.");
            }
            else if (btnLocation.Content.Equals("Stop"))
            {
                StopLocationService();
                btnLocation.Content = "Start";
                Log("Location service stopped.");
            }
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if (btnInventory.Content.Equals("Start"))
            {
                StartInventoryService();
                btnInventory.Content = "Stop";
                Log("Inventory service started.");
            }
            else if (btnInventory.Content.Equals("Stop"))
            {
                StopInventoryService();
                btnInventory.Content = "Start";
                Log("Inventory service stopped.");
            }
        }

        private void btnAccounting_Click(object sender, RoutedEventArgs e)
        {
            if (btnAccounting.Content.Equals("Start"))
            {
                StartAccountingService();
                btnAccounting.Content = "Stop";
                Log("Accounting service started.");
            }
            else if (btnAccounting.Content.Equals("Stop"))
            {
                StopAccountingService();
                btnAccounting.Content = "Start";
                Log("Accounting service stopped.");
            }
        }

        private void btnShipping_Click(object sender, RoutedEventArgs e)
        {
            if (btnShipping.Content.Equals("Start"))
            {
                StartShippingService();
                btnShipping.Content = "Stop";
                Log("Shipping service started.");
            }
            else if (btnShipping.Content.Equals("Stop"))
            {
                StopShippingService();
                btnShipping.Content = "Start";
                Log("Shipping service stopped.");
            }
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (btnOrder.Content.Equals("Start"))
            {
                StartOrerService();
                btnOrder.Content = "Stop";
                Log("Order service started.");
            }
            else if (btnOrder.Content.Equals("Stop"))
            {
                StopOrderService();
                btnOrder.Content = "Start";
                Log("Order service stopped.");
            }
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            if (btnAll.Content.Equals("Start All"))
                btnAll.Content = "Stop All";
            else if (btnAll.Content.Equals("Stop All"))
                btnAll.Content = "Start All";

            btnLocation_Click(sender, e);
            btnInventory_Click(sender, e);
            btnShipping_Click(sender, e);
            btnAccounting_Click(sender, e);
            btnOrder_Click(sender, e);
        }
    }
}
using BloomSales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Hosts.Console
{
    public class Program
    {
        private static List<ServiceHost> hosts;

        public static void Main(string[] args)
        {
            if (args.Length == 0)
                PrintHelp();

            hosts = new List<ServiceHost>();

            try
            {
                StartServices(args);
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("Oops! Could not start service(s) requested.");
            }
            catch (AddressAlreadyInUseException)
            {
                System.Console.WriteLine("The addresses for given requested services are in use.");
            }

            System.Console.WriteLine("To exit the application, just type 'exit'.");

            WaitUntilExit();

            StopServices();
        }

        private static void WaitUntilExit()
        {
            while (true)
            {
                string cmd = System.Console.ReadLine();

                if (cmd.ToLower() == "exit")
                    break;
            }
        }

        private static void StartServices(string[] serviceNames)
        {
            foreach (string service in serviceNames)
            {
                switch (service.ToLower())
                {
                    case "accounting":
                        HostService("Accounting Serice", typeof(AccountingService));
                        break;

                    case "inventory":
                        HostService("Inventory Service", typeof(InventoryService));
                        break;

                    case "location":
                        HostService("Location Service", typeof(LocationService));
                        break;

                    case "order":
                        HostService("Order Service", typeof(OrderService));
                        break;

                    case "shipping":
                        HostService("Shipping Service", typeof(ShippingService));
                        break;

                    default:
                        System.Console.WriteLine("Invalid service name!");
                        System.Console.WriteLine();
                        PrintHelp();
                        break;
                }
            }
        }

        private static void HostService(string serviceName, Type serviceType)
        {
            ServiceHost svcHost = new ServiceHost(serviceType);
            svcHost.Open();
            string logMsg = string.Format("[{0}] {1} started.", DateTime.Now, serviceName);
            System.Console.WriteLine(logMsg);
        }

        private static void StopServices()
        {
            if (hosts != null && hosts.Count > 0)
                foreach (var svcHost in hosts)
                    svcHost.Close();
        }

        private static void PrintHelp()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("accounting", "Accounting Service");
            options.Add("inventory", "Inventory Service");
            options.Add("location", "Location Service");
            options.Add("order", "Order Service");
            options.Add("shipping", "Shipping Service");

            System.Console.WriteLine("USAGE:\tbss ServiceName1 [, ServiceName2, ServiceName3, ServiceName4, ServiceName5]");
            System.Console.WriteLine();
            System.Console.WriteLine("The following service names are available. Each service will be started based on " +
                                      "the configurations provided in App.config file.");
            System.Console.WriteLine();
            System.Console.WriteLine("SERVICE NAMES:");

            foreach (string optionKey in options.Keys)
            {
                System.Console.Write("\t" + optionKey + "\t");

                if (optionKey.Length < 8)
                    System.Console.Write("\t");

                System.Console.WriteLine(options[optionKey]);
            }

            System.Console.WriteLine();
        }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Ninject;

namespace GroceryStorePOS
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        private readonly IKernel _kernel = new StandardKernel();

        public void Start()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            this._kernel.Load(callingAssembly);
            var consoleManager = this._kernel.Get<ConsoleManager>();

            Console.WriteLine("Welcome to Grocery Store POS system.");
            char command = ' ';
            while (command != 'q')
            {
                Console.WriteLine();
                Console.WriteLine("Please enter one of the following commands.");
                Console.WriteLine("1. [S]can product into order");
                Console.WriteLine("2. [C]ancel a product from existing order");
                Console.WriteLine("3. [P]rint the existing order");
                Console.WriteLine("4. [D]iscount product by percentage of product price");
                Console.WriteLine("5. D[i]scount product by quantity of product");
                Console.WriteLine("6. C[l]ear the order");
                Console.WriteLine("7. [Q]uit");
                Console.WriteLine();
                command = ReadChar("Please enter the character you want to execute: ");
                switch (command)
                {
                    case 's': // scan product
                        var builder = new StringBuilder();
                        string productId;
                        char subcommand = 's';
                        while (subcommand == 's' || subcommand == 'b')
                        {
                            subcommand = ReadChar("Select a mode to scan a product ([S]ingle / [B]ulk / [F]inish): ");
                            switch (subcommand)
                            {
                                case 's':
                                    productId = ReadString("Please enter a product ID: ");
                                    builder.Append($"Single,{productId},");
                                    break;
                                case 'b':
                                    productId = ReadString("Please enter a product ID: ");
                                    var quantity = ReadInt("Please enter quantity to purchase: ");
                                    builder.Append($"Bulk,{productId},{quantity},");
                                    break;
                            }
                        }

                        if (builder.Length > 0)
                        {
                            builder = builder.Remove(builder.Length - 1, 1);
                            var input = builder.ToString();
                            consoleManager.Scan(input);
                        }

                        break;
                    case 'c': // cancel product
                        productId = ReadString("Please enter a product ID: ");
                        consoleManager.Cancel(productId);
                        break;
                    case 'p': // print order
                        var receipt = consoleManager.Print();
                        foreach (var line in receipt)
                        {
                            Console.WriteLine(line);
                        }

                        Console.WriteLine();
                        break;
                    case 'd': // discount product by percentage
                        productId = ReadString("Please enter a product ID: ");
                        decimal offPct = ReadDecimal("Please enter the percentage off the marked price: ");
                        consoleManager.DiscountByPercentage(productId, offPct);
                        break;
                    case 'i': // discount product by quantity
                        productId = ReadString("Please enter a product ID: ");
                        var buyQty = ReadInt("Please enter the buying quantity: ");
                        var getFreeQty = ReadInt("Please enter the quantity to get for free: ");
                        consoleManager.DiscountByQuantity(productId, buyQty, getFreeQty);
                        break;
                    case 'l': // clear order
                        consoleManager.ClearOrder();
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var launcher = new Program();
            launcher.Start();
        }

        static private char ReadChar(string msg)
        {
            Console.Write(msg);
            var ret = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();
            return ret;
        }

        static private string ReadString(string msg)
        {
            Console.Write(msg);
            var ret = Console.ReadLine();
            return ret;
        }

        static private int ReadInt(string msg)
        {
            while (true)
            {
                var retStr = ReadString(msg);
                if (int.TryParse(retStr, out int quantity) && quantity > 0)
                {
                    return quantity;
                }

                Console.WriteLine("Please enter a valid integer.");
            }
        }

        static private decimal ReadDecimal(string msg)
        {
            while (true)
            {
                var retStr = ReadString(msg);
                if (decimal.TryParse(retStr, out decimal quantity) && quantity > 0)
                {
                    return quantity;
                }

                Console.WriteLine("Please enter a valid integer.");
            }
        }
    }
}

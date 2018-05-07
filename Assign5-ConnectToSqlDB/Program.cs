using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign5_ConnectToSqlDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This program will allow you to obtain product information about certain products or enter new products. ");

            //choose the db schema to connect to
            DataClasses1DataContext db = new DataClasses1DataContext();

            //get user choice
            int choice = menu();


            while (choice != 3)
            {
                if (choice == 1)
                {
                    Console.WriteLine("\nYou have chosen to view product information for specific products:");
                    Console.WriteLine("\nplease enter the minimum price.");

                    Double minPrice = Double.Parse(Console.ReadLine());

                    Console.WriteLine("Please enter the maximum price of the products you wish to see.");

                    Double maxPrice = Double.Parse(Console.ReadLine());



                    var productsInRange = db.PRODUCTs.Where(P => P.P_PRICE.CompareTo(minPrice) >= 0 && P.P_PRICE.CompareTo(maxPrice) <= 0);

                    foreach (var product in productsInRange)
                    {
                        Console.WriteLine("\nProduct Description: " + product.P_DESCRIPT + "\nPrice: " + product.P_PRICE + "\nVendor Name: " + (product.VENDOR?.V_NAME ?? "NO VENDOR"));
                    }
                }

                else if (choice == 2)
                {
                    Console.WriteLine("\nYou have chosen to enter a new product.");



                    Console.WriteLine("Enter \"Y\" for yes to enter a new Vendor, or \"N\" for no to enter an existing Vendor Code of the new Product.");

                    String reply = Console.ReadLine().ToUpper();

                    int vcode = 0;

                    if (reply != "Y" && reply != "N")
                    {
                        Console.WriteLine("Invalid Entry. ");
                    }

                    else
                    {
                        if (reply == "Y")
                        {

                            VENDOR v = new VENDOR();


                            Console.WriteLine("Please enter the Vendor Code: (Code should be all numbers)");
                            vcode = Int32.Parse(Console.ReadLine());

                            v.V_CODE = vcode;

                            //check if v_Code is already in use:
                            while (db.VENDORs.Any(v2 => v2.V_CODE == v.V_CODE))
                            {
                                Console.WriteLine("Vendor Code already in use. Please try again.");
                                vcode = Int32.Parse(Console.ReadLine());

                                v.V_CODE = vcode;

                            }

                            Console.WriteLine("Please enter the vendor name");
                            v.V_NAME = Console.ReadLine();

                            Console.WriteLine("Please enter the Vendor  Contact:");
                            v.V_CONTACT = Console.ReadLine();

                            Console.WriteLine("Please enter the Vendor Area Code:");
                            v.V_AREACODE = Console.ReadLine();

                            Console.WriteLine("Please enter the Vendor Phone:");
                            v.V_PHONE = Console.ReadLine();

                            Console.WriteLine("Please enter the Vendor State:");
                            v.V_STATE = Console.ReadLine();

                            Console.WriteLine("Please enter \'Y\' or \'N\' for  Vendor Order Status:");
                            v.V_ORDER = Char.Parse(Console.ReadLine());

                            bool vendorEntered = false;

                            while (!vendorEntered)
                            {
                                try
                                {
                                    db.VENDORs.InsertOnSubmit(v);
                                    db.SubmitChanges();

                                    Console.WriteLine("New Vendor Entered!");

                                    vendorEntered = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error. Unable to enter vendor");
                                }
                            }

                        }

                        else if (reply == "N")
                        {
                            Console.WriteLine("Please enter the Vendor Code:");
                            vcode = Int32.Parse(Console.ReadLine());

                            //check if v_Code is valid
                            while (!db.VENDORs.Any(v2 => v2.V_CODE == vcode))
                            {
                                Console.WriteLine("Vendor Code is not valid. Please try again.");
                                vcode = Int32.Parse(Console.ReadLine());

                            }
                        }



                        PRODUCT newProduct = new PRODUCT();

                        Console.WriteLine("\nPlease enter the Product Description:");
                        newProduct.P_DESCRIPT = Console.ReadLine();

                        Console.WriteLine("Please enter the Product Code:");
                        newProduct.P_CODE = Console.ReadLine();

                        //check if product code is already in use:
                        while (db.PRODUCTs.Any(p => p.P_CODE == newProduct.P_CODE))
                        {
                            Console.WriteLine("Product Code already in use. Please try again.");
                            newProduct.P_CODE = Console.ReadLine();
                        }

                        Console.WriteLine("Please enter the date the product came in:");

                        bool dateValid = false;

                        while (!dateValid)
                        {
                            try
                            {
                                newProduct.P_INDATE = DateTime.Parse(Console.ReadLine());
                                dateValid = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Date entered was invalid. Please try again");
                            }

                        }

                        Console.WriteLine("Please enter the quantity on hand:");
                        newProduct.P_QOH = Int32.Parse(Console.ReadLine());

                        Console.WriteLine("Please enter the product minimum:");
                        newProduct.P_MIN = Int32.Parse(Console.ReadLine());

                        Console.WriteLine("Please enter the product price:");
                        newProduct.P_PRICE = Decimal.Parse(Console.ReadLine());

                        Console.WriteLine("Please enter the Product Discount :");
                        newProduct.P_DISCOUNT = Decimal.Parse(Console.ReadLine());

                        newProduct.V_CODE = vcode;

                        try
                        {
                            db.PRODUCTs.InsertOnSubmit(newProduct);
                            db.SubmitChanges();

                            Console.WriteLine("New product entered!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error. Unable to enter product.");
                        }

                    }
                }


                else
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                }

                choice = menu();
            }

            // System.Environment.Exit(1);
            Console.ReadKey();
        }

        public static int menu()
        {
            Console.WriteLine("\nPlease choose from the following menu options: Enter a number from 1-3." +
               "\n\t1.View product information.\n\t2. Enter a new product. \n\t3. Quit the program.");

            return Int32.Parse(Console.ReadLine());
        }
    }
}

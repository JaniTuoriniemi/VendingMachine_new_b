using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace VendingMachine
{
    public class Program
    {
        static void Main(string[] args)
        {// Main provides the user interface for the VendingMachine objects.
            VendingMachine vendingMachine = new VendingMachine();
            Console.WriteLine("The vending machine must first be loaded with products.");
            bool is_filling = true;
            while (is_filling)
            {

                Console.WriteLine("Press 1 to load a beverage, 2 to load a snack, 3 to load a toy.\n Press 0 when vending machine is initialised");
                int type_choice = Convert.ToInt32(Console.ReadLine());
                if (type_choice == 1 || type_choice == 2 || type_choice == 3)
                {
                    vendingMachine.LoadProduct(type_choice);
                }
                if (type_choice == 0)
                {
                    is_filling = false;
                }
            }
            bool is_on = true;


            while (is_on)
            {
                Console.WriteLine("Welcome to the vending machine.\r\n To initiate a purchase press 1, to quit press 0");
                bool control = double.TryParse(Console.ReadLine(), out double ansver);
                if (control)
                {
                    switch (ansver)
                    {
                        case 0:
                            is_on = false;
                            Console.WriteLine("Closes the program");// The program shuts down by user choice.
                            break;
                        case 1:
                            bool session = true;
                            //Initialises purchase session.
                            while (session)
                            {

                                Console.WriteLine("The following items are available");
                                vendingMachine.Display();
                                Console.WriteLine("Press 1 to insert money, press 0 to leave");
                                bool control2 = double.TryParse(Console.ReadLine(), out double ansver2);
                                if (control2)
                                {
                                    switch (ansver2)
                                    {
                                        case 0:
                                            session = false;
                                            vendingMachine.Checkout();//Exits purchase session.
                                            break;
                                        case 1:

                                            vendingMachine.Balance = vendingMachine.Deposit(vendingMachine.Denominations, vendingMachine.Balance);
                                            bool session2 = true;
                                            while (session2)
                                            {//Next step of purchase session.
                                                Console.WriteLine("Enter product number to examine a product");
                                                Console.WriteLine("Enter 123 for purchase");
                                                Console.WriteLine("Enter 321 to remove a purchased product");
                                                Console.WriteLine("Enter 0 to get money returned and leave");
                                                bool control3 = int.TryParse(Console.ReadLine(), out int ansver3);
                                                if (control3)
                                                {
                                                    if (0 < ansver3 && ansver3 < vendingMachine.Inventory.Count)
                                                    { vendingMachine.Display(ansver3); }
                                                    else
                                                    {
                                                        switch (ansver3)
                                                        {
                                                            case 0:
                                                                vendingMachine.Checkout();
                                                                //Exits purchase session.
                                                                session2 = false;

                                                                break;
                                                            case 123://Purchase is executed
                                                                Console.WriteLine("Give the product number of the product you vant to buy.");
                                                                bool control4 = int.TryParse(Console.ReadLine(), out int choice);
                                                                if (control4)
                                                                {
                                                                    vendingMachine.Purchase(choice);
                                                                    Console.WriteLine("Your shopping cart is now contains:");
                                                                    vendingMachine.Displaypurchases();
                                                                    Console.WriteLine($"Your have {vendingMachine.Balance} Kr left.");
                                                                }
                                                                else { Console.WriteLine("Could not understand input"); }
                                                                break;
                                                            case 321:// Puchased products are removed from the basket.
                                                                Console.WriteLine("Give the product number of the product you vant to remove");
                                                                bool control5 = int.TryParse(Console.ReadLine(), out int choice2);
                                                                if (control5)
                                                                {
                                                                    vendingMachine.Unpurchase(choice2);
                                                                }
                                                                else { Console.WriteLine("Could not understand input"); }
                                                                break;

                                                            default:
                                                                Console.WriteLine("Could not understand input");
                                                                break;

                                                        }
                                                    }
                                                }
                                                else { Console.WriteLine("Could not understand input"); }
                                            }
                                            break;
                                        default:
                                            Console.WriteLine("Could not understand input");
                                            break;

                                    }
                                }
                                else { Console.WriteLine("Could not understand input"); }
                            }
                            break;
                        default:
                            Console.WriteLine("Could not understand input");
                            break;

                    }

                }
                else { Console.WriteLine("Could not understand input"); }
            }
        }
        interface IVendingMachine
        {
            public void LoadProduct(int type_choice);
            int Deposit(int[] Denominations, int Balance);
            public int[] Cashreturn();
            public void Display();
            public void Display(int ansver);
            public void Displaypurchases();
            public void Purchase(int choice);
            public void Unpurchase(int choice);
            public void Checkout();
        }
        public class VendingMachine : IVendingMachine
        {//Initialises the VendingMachine classes.
            public List<Product> Purchases { get; set; }
            public List<Product> Inventory { get; set; }
            public int[] Denominations { get; set; }
            public int Balance { get; set; }
            public VendingMachine()
            {
                int[] _denominations;//Money denominations are stored and set for the program.
                _denominations = new int[9] { 1, 5, 10, 20, 50, 100, 200, 500, 1000 };
                Array.AsReadOnly<int>(_denominations);
                Denominations = _denominations;
                Balance = 0;//Initial balance.
                Purchases = new List<Product>();//Shopping basket is initialised.
                Inventory = new List<Product>();//Product inventory is built up.

            }
            public void LoadProduct(int type_choice)
            {
                string name;
                int price;
                int product_No;
                switch (type_choice)
                {
                    case 1://The product objects are created by the user.
                        Console.WriteLine("Write the name of the product");
                        name = Console.ReadLine();
                        Console.WriteLine("State the price of the product");
                        price = Convert.ToInt32(Console.ReadLine());
                        product_No = Inventory.Count() + 1;
                        Inventory.Add(new Beverage(name, price, product_No));
                        break;
                    case 2:
                        Console.WriteLine("Write the name of the product");
                        name = Console.ReadLine();
                        Console.WriteLine("State the price of the product");
                        price = Convert.ToInt32(Console.ReadLine());
                        product_No = this.Inventory.Count() + 1;
                        Inventory.Add(new Snack(name, price, product_No));
                        break;
                    case 3:
                        Console.WriteLine("Write the name of the product");
                        name = Console.ReadLine();
                        Console.WriteLine("State the price of the product");
                        price = Convert.ToInt32(Console.ReadLine());
                        product_No = this.Inventory.Count() + 1;
                        Inventory.Add(new Toy(name, price, product_No));
                        break;
                }

            }
            public int Deposit(int[] Denominations, int Balance)
            {//Accepts money from user and sets the balance accordingly.
                int[] money = Denominations;
                for (int i = 0; i < money.Length; i++)
                {
                    if (money[i] < 20)
                    { Console.WriteLine($"How many { money[i]} Kr coins do you want to insert?"); }
                    else
                    { Console.WriteLine($"How many { money[i]} Kr bills do you want to insert?"); }
                    int.TryParse(Console.ReadLine(), out int number);
                    Balance = Balance + number * money[i];
                }
                return Balance;
            }
            public int[] Cashreturn()
            {//A vector matching the VendingMachine.Denominations is created storing the number of bill
                //that must be returned to user.
                int stilleft = Balance;
                int[] money = Denominations;
                int length = money.Length;
                int[] return_money = new int[length];

                for (int i = length - 1; i > -1; i--)
                {
                    if (money[i] <= stilleft)
                    {
                        int quot = Math.DivRem(stilleft, money[i], out int remainder);
                        stilleft = remainder;
                        return_money[i] = quot;
                    }
                }
                return return_money;
            }
            public void Display()
            {//Shows information about all products in inventory.
                for (int i = 0; i < Inventory.Count; i++)
                { Inventory[i].Examine(); }
            }
            public void Display(int ansver)
            {//Shows information about a product with a given product number.
                Inventory[ansver - 1].Examine();
            }
            public void Displaypurchases()
            {//Shows information about purchased products.

                for (int i = 0; i < Purchases.Count; i++)
                { Purchases[i].Examine(); }
            }
            public void Purchase(int choice)
            {// Adds a product to the basket and adjusts the balance according to price.
                int t = Inventory[choice - 1].Show_Price();
                if (Balance >= t)
                {
                    Balance = Balance - t;
                    Purchases.Add(Inventory[choice - 1]);
                }
                else { Console.WriteLine("Not enough money!"); }
            }
            public void Unpurchase(int choice)
            { // Removes a product from the basket and adjusts the balance accordingly.
                bool contained = Purchases.Remove(Inventory[choice - 1]);
                if (contained == false)
                { Console.WriteLine("You never bought this product!"); }
                else
                {
                    int t = Inventory[choice - 1].Show_Price();
                    Balance = Balance + t;
                    Console.WriteLine($"All {Inventory[choice - 1].Show_Name()}s have been removed from your basket");
                }
            }
            public void Checkout()//The money is returned and the purchases are shown upon leaving.
            {
                int[] return_money = Cashreturn();
                string money_message = "You are given ";
                for (int i = 0; i < return_money.Length; i++)
                {
                    if (Denominations[i] < 20)
                    { money_message = money_message + $"{return_money[i]} {Denominations[i]} Kr coins, "; }
                    if (Denominations[i] >= 20)
                    { money_message = money_message + $"{return_money[i]} {Denominations[i]} Kr bills, "; }
                }

                for (int i = 0; i < Purchases.Count; i++)
                {
                    Console.WriteLine(" You have purchased the following items.");
                    Console.WriteLine($"{Purchases[i].Show_Name()}  {Purchases[i].Description()}");
                }
                Console.WriteLine(money_message + "In return");
                Balance = 0;// Balance is zeroed and the shopping basket emptied.
                Purchases = new List<Product>();

            }
        }

        public abstract class Product
        {
            public abstract string Category();
            public abstract string Description();
            public abstract string Show_Name();
            public abstract int Show_Price();
            public abstract void Examine();
            public abstract int Show_Product_No();

        }
        public class Beverage : Product
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int Product_No { get; set; }
            public Beverage(string name, int price, int product_No)
            {
                Name = name;
                Price = price;
                Product_No = product_No;
            }
            public override void Examine()
            { Console.WriteLine("Name: " + Name + " Category: " + Category() + "Use: " + Description() + "Product No: " + Convert.ToString(Show_Product_No()) + ", Price: " + Convert.ToString(Show_Price()) + " Kr "); }
            public override string Category()
            { return "Beverage "; }
            public override string Description()
            { return "You can drink the beverage! "; }
            public override string Show_Name()
            { return Name; }
            public override int Show_Price()
            { return Price; }
            public override int Show_Product_No()
            { return Product_No; }
        }
        public class Snack : Product
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int Product_No { get; set; }
            public Snack(string name, int price, int product_No)
            {
                Name = name;
                Price = price;
                Product_No = product_No;
            }
            public override void Examine()
            { Console.WriteLine("Name: " + Name + " Category: " + Category() + "Use: " + Description() + "Product No: " + Convert.ToString(Show_Product_No()) + ", Price: " + Convert.ToString(Show_Price()) + " Kr "); }
            public override string Category()
            { return "Snack, "; }
            public override string Description()
            { return "You can eat the snack! "; }
            public override string Show_Name()
            { return Name; }
            public override int Show_Price()
            { return Price; }
            public override int Show_Product_No()
            { return Product_No; }
        }
        public class Toy : Product
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int Product_No { get; set; }
            public Toy(string name, int price, int product_No)
            {
                Name = name;
                Price = price;
                Product_No = product_No;
            }
            public override void Examine()
            { Console.WriteLine("Name: " + Name + " Category: " + Category() + "Use: " + Description() + "Product No: " + Convert.ToString(Show_Product_No()) + ", Price: " + Convert.ToString(Show_Price()) + " Kr "); }
            public override string Category()
            { return "Toys, "; }
            public override string Description()
            { return "You can play with the toy! "; }
            public override string Show_Name()
            { return Name; }
            public override int Show_Price()
            { return Price; }
            public override int Show_Product_No()
            { return Product_No; }
        }
    }
}



















using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Xunit;

namespace VendingMachine
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Deposit()// Test that the quantities of each denomination given by the user result in the correct balance.
        {
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            int Balance = 0;
            int[] Denominations = new int[9] { 1, 5, 10, 20, 50, 100, 200, 500, 1000 };

            int expectedValue = 1886;
            var input = new StringReader(@"1
1
1
1
1
1
1
1
1");
            Console.SetIn(input);
            Balance = vendingMachine.Deposit(Denominations, Balance);


            Assert.Equal(expectedValue, Balance);

        }
        [Fact]
        public void Test_LoadProduct()// Tests that a proper object is created upon user request.
        {
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            var input = new StringReader(@"coke
10");
            Console.SetIn(input);
            vendingMachine.LoadProduct(1);
            int expected_price = 10;
            string expected_name = "coke";
            string expected_type = "Beverage ";
            Assert.Equal(expected_price, vendingMachine.Inventory[0].Show_Price());
            Assert.Equal(expected_name, vendingMachine.Inventory[0].Show_Name());
            Assert.Equal(expected_type, vendingMachine.Inventory[0].Category());
        }

        [Fact]
        public void Test_Cashreturn()// Tests that the correct number of each money denomination is returned to the user.
        {
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            vendingMachine.Balance = 152;
            int[] expectedValue = new int[9] { 2, 0, 0, 0, 1, 1, 0, 0, 0 };
            int[] ret = vendingMachine.Cashreturn();
            Assert.Equal(expectedValue, ret);
        }
        [Fact]
        public void Test_Cashreturn2()
        {//The money must be returned in the proper set of valeurs.Second test.
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            vendingMachine.Balance = 1005;
            int[] expectedValue = new int[9] { 0, 1, 0, 0, 0, 0, 0, 0, 1 };
            int[] ret = vendingMachine.Cashreturn();
            Assert.Equal(expectedValue, ret);
        }
        [Fact]
        public void Test_Purchase()
        {// Tests that the chosen product is added to basket and money correctly withdrawn from the balance.
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            int product_No = vendingMachine.Inventory.Count() + 1;
            vendingMachine.Inventory.Add(new Program.Beverage("Test_tab", 10, product_No));
            vendingMachine.Balance = 15;
            vendingMachine.Purchase(1);
            bool expected = (vendingMachine.Purchases[0] == vendingMachine.Inventory[0]);
            Assert.Equal(expected, true);
            int expected_balance = 5;
            Assert.Equal(expected_balance, vendingMachine.Balance);
        }
        [Fact]
        public void Test_Purchase2()
        {// Tests that the purchase is not executed in case of insufficient funds.
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            int product_No = vendingMachine.Inventory.Count() + 1;
            vendingMachine.Inventory.Add(new Program.Beverage("Test_tab", 10, product_No));
            vendingMachine.Balance = 9;
            vendingMachine.Purchase(1);
            int expected = 0;
            int expected_balance = 9;
            Assert.Equal(expected, vendingMachine.Purchases.Count());
            Assert.Equal(expected_balance, vendingMachine.Balance);
        }
        [Fact]
        public void Test_Unpurchase()
        {// Test that the item is removed from basket and money returned upon unpurchase.
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            int product_No = vendingMachine.Inventory.Count() + 1;
            vendingMachine.Inventory.Add(new Program.Beverage("Test_tab", 10, product_No));
            vendingMachine.Purchases.Add(vendingMachine.Inventory[0]);
            vendingMachine.Balance = 0;
            vendingMachine.Unpurchase(1);
            int a = vendingMachine.Purchases.Count;
            int expected = 0;
            Assert.Equal(expected, a);
            int expected2 = 10;
            Assert.Equal(expected2, vendingMachine.Balance);
        }

        [Fact]
        public void Test_Checkout()
        {// Test for that  balance is rezeroed and the shoppingbasket emptied upon checkout.
            Program.VendingMachine vendingMachine = new Program.VendingMachine();
            vendingMachine.Balance = 57;
            int product_No = vendingMachine.Inventory.Count() + 1;
            vendingMachine.Purchases.Add(new Program.Beverage("Test_tab", 10, product_No));
            vendingMachine.Checkout();

            int expected_balance = 0;
            int a = vendingMachine.Purchases.Count;
            int expected = 0;
            Assert.Equal(expected, a);
            Assert.Equal(expected_balance, vendingMachine.Balance);

        }

    }
}



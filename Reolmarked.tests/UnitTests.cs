using Xunit;
using Reolmarked.Model;
using System;

namespace Reolmarked.Tests
{
    public class UnitTests
    {

        /*
         Da unit tests først blev startet dagen inden vi skulle afleveres, er det ikke gennemtænkt eller "relevant" nok til selve programmet.
         Den tester overfladisk at man kan lave en ny instans af en klasse uden at teste selve metoderne fra Reolmarked programmet.
         I stedet for at teste private async void AddProductBtnClick() i ProductViewModel, så tester den blot var product = new Product(), 
         som ikke rigtigt fortæller om programmet fungere eller ej.
         Det skal selvfølelig gøres bedre til eksamensprojektet.

         Vi bruger UI elementer som MessageBox.Show() i vores metoder som kræver at vi bygger det om for at muliggøre tests af selve metoderne, noget vi ikke har tid til.
        */
        #region Vare Tests

        [Fact]
        public void Test_AddNewProduct()
        {
            var product = new Product("1234567890", "Test Produkt", 50, 1);

            Assert.Equal("1234567890", product.ProductSerialNumber);
            Assert.Equal("Test Produkt", product.ProductDescription);
            Assert.Equal(50, product.ProductPrice);
            Assert.Equal(1, product.ShelfNumber);
            Assert.False(product.ProductSold);
        }

        [Fact]
        public void Test_EditProduct()
        {
            var product = new Product("1234567890", "Test Produkt2", 100, 1);

            product.ProductDescription = "ændret beskrivelse";
            product.ProductPrice = 150;

            Assert.Equal("ændret beskrivelse", product.ProductDescription);
            Assert.Equal(150, product.ProductPrice);
        }

        [Fact]
        public void Test_ChangeShelfOnProduct()
        {
            var product = new Product("1234567890", "Test Produkt", 100, 1);

            product.ShelfNumber = 5;

            Assert.Equal(5, product.ShelfNumber);
        }

        [Fact]
        public void Test_MarkProductAsSold()
        {
            var product = new Product("1234567890", "Test Produktt", 100, 1);

            product.ProductSold = true;

            Assert.True(product.ProductSold);
        }

        #endregion

        #region Lejer Tests

        [Fact]
        public void Test_AddNewRenter()
        {
            var renter = new Renter("Hans Hansen", "12345678", "hansen@email.dk", "1234-567890");

            Assert.Equal("Hans Hansen", renter.RenterName);
            Assert.Equal("12345678", renter.RenterTelephoneNumber);
            Assert.Equal("hansen@email.dk", renter.RenterEmail);
            Assert.Equal("1234-567890", renter.RenterAccountNumber);
        }

        [Fact]
        public void Test_UpdateRenterInfo()
        {
            var renter = new Renter("Hans Hansen", "12345678", "hansen@email.dk", "1234-567890");

            renter.RenterTelephoneNumber = "87654321";
            renter.RenterEmail = "nyemail@email.dk";

            Assert.Equal("87654321", renter.RenterTelephoneNumber);
            Assert.Equal("nyemail@email.dk", renter.RenterEmail);
        }

        #endregion

        #region Reol Tests

        [Fact]
        public void Test_CreateShelf()
        {
            var shelf = new Shelf(1, 1);

            Assert.Equal(1, shelf.ShelfNumber);
            Assert.Equal(1, shelf.ShelfTypeId);
            Assert.False(shelf.ShelfRented);
        }

        [Fact]
        public void Test_RentShelf()
        {
            var shelf = new Shelf(5, 1);

            shelf.ShelfRented = true;

            Assert.True(shelf.ShelfRented);
            Assert.Equal(5, shelf.ShelfNumber);
        }

        [Fact]
        public void Test_FreeShelf()
        {
            var shelf = new Shelf(10, 1);
            shelf.ShelfRented = true;

            shelf.ShelfRented = false;

            Assert.False(shelf.ShelfRented);
        }

        #endregion

        #region Lejeaftale Tests

        [Fact]
        public void Test_CreateRentalAgreement()
        {
            var agreement = new RentalAgreement(1, DateTime.Now, 1650);

            Assert.Equal(1, agreement.RenterId);
            Assert.Equal(1650, agreement.RentalAgreementTotalPrice);
        }

        [Fact]
        public void Test_CreateAgreementLine()
        {
            var agreementLine = new AgreementLine(5, 1);

            Assert.Equal(5, agreementLine.ShelfNumber);
            Assert.Equal(1, agreementLine.RentalAgreementId);
        }

        [Fact]
        public void Test_UpdateRentalAgreementPrice()
        {
            var agreement = new RentalAgreement(1, DateTime.Now, 1650);

            agreement.RentalAgreementTotalPrice = 2475;

            Assert.Equal(2475, agreement.RentalAgreementTotalPrice);
        }

        #endregion

        #region Betaling Tests

        [Fact]
        public void Test_CreateTransaction()
        {
            var transaction = new Transaction(DateTime.Now, 285, 300, 1);

            Assert.Equal(285, transaction.TransactionTotalAmount);
            Assert.Equal(300, transaction.TransactionPaidAmount);
            Assert.Equal(1, transaction.PaymentMethodId);
        }

        [Fact]
        public void Test_CalculateChange()
        {
            var transaction = new Transaction(DateTime.Now, 285, 300, 1);

            var change = transaction.TransactionPaidAmount - transaction.TransactionTotalAmount;

            Assert.Equal(15, change);
        }

        [Fact]
        public void Test_CreateTransactionLine()
        {
            var transactionLine = new TransactionLine(1, "1234567890");

            Assert.Equal(1, transactionLine.TransactionId);
            Assert.Equal("1234567890", transactionLine.ProductSerialNumber);
        }

        [Fact]
        public void Test_ExactPayment()
        {
            var transaction = new Transaction(DateTime.Now, 100, 100, 1);

            var change = transaction.TransactionPaidAmount - transaction.TransactionTotalAmount;

            Assert.Equal(0, change);
        }

        [Fact]
        public void Test_MultipleProductsPayment()
        {
            var product1Price = 100;
            var product2Price = 150;
            var product3Price = 35;
            var totalPrice = product1Price + product2Price + product3Price;

            var transaction = new Transaction(DateTime.Now, totalPrice, 300, 1);
            var change = transaction.TransactionPaidAmount - transaction.TransactionTotalAmount;

            Assert.Equal(285, transaction.TransactionTotalAmount);
            Assert.Equal(15, change);
        }

        #endregion

        #region PaymentMethod Tests

        [Fact]
        public void Test_CreatePaymentMethod()
        {
            var paymentMethod = new PaymentMethod("Kontant");

            Assert.Equal("Kontant", paymentMethod.PaymentMethodName);
        }

        #endregion

        #region ShelfType Tests

        [Fact]
        public void Test_CreateShelfType()
        {
            var shelfType = new ShelfType("Standard", "6 hylder", 850);

            Assert.Equal("Standard", shelfType.ShelfTypeName);
            Assert.Equal("6 hylder", shelfType.ShelfTypeDescription);
            Assert.Equal(850, shelfType.ShelfTypePrice);
        }

        [Fact]
        public void Test_UpdateShelfTypePrice()
        {
            var shelfType = new ShelfType("Standard", "6 hylder", 850);

            shelfType.ShelfTypePrice = 900;

            Assert.Equal(900, shelfType.ShelfTypePrice);
        }

        #endregion

        #region Månedlig Afregning Tests

        [Fact]
        public void Test_CreateMonthlySettlement()
        {
            var settlement = new MonthlySettlement(1, DateTime.Now, 850, 1000, 100);

            Assert.Equal(1, settlement.RenterId);
            Assert.Equal(850, settlement.TotalRent);
            Assert.Equal(1000, settlement.TotalSales);
            Assert.Equal(100, settlement.Commission);
            Assert.Equal(50, settlement.NetAmount);
            Assert.False(settlement.IsPaid);
        }

        [Fact]
        public void Test_MarkSettlementAsPaid()
        {
            var settlement = new MonthlySettlement(1, DateTime.Now, 850, 1000, 100);

            settlement.IsPaid = true;

            Assert.True(settlement.IsPaid);
        }

        [Fact]
        public void Test_RenterOwesShop()
        {
            var settlement = new MonthlySettlement(1, DateTime.Now, 850, 500, 50);

            Assert.True(settlement.NetAmount < 0);
            Assert.Equal(-400, settlement.NetAmount);
        }

        [Fact]
        public void Test_ShopOwesRenter()
        {
            var settlement = new MonthlySettlement(1, DateTime.Now, 850, 2000, 200);

            Assert.True(settlement.NetAmount > 0);
            Assert.Equal(950, settlement.NetAmount);
        }

        #endregion
    }
}
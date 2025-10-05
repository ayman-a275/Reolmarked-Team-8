using System.Net.Http.Headers;
using Reolmarked.Model;

namespace Reolmarked.tests
{
    public class UnitTest1
    {
        //InMemory Database
        //tilføj ny vare
        //redigere en ny vare
        //ændre reol
        //tilføj ny lejer
        //lej en reol
        //slet en lejeaftale
        //tag imod betaling
        [Fact]
        public void Test_AddNewProduct()
        {
            var product = new Product("1234567890", "Test Product", 50, 1);

            Assert.Equal("1234567890", product.ProductSerialNumber);
            Assert.Equal("Test Product", product.ProductDescription);
            Assert.Equal(50, product.ProductPrice);
            Assert.Equal(1, product.ShelfNumber);
            Assert.False(product.ProductSold);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuctionHouse.Models;

namespace AuctionHouse_Tests
{
    public class BuyerTests
    {
        private Buyer MakeValidBuyer()
        {
            Buyer buyer = new Buyer
            {
                Id = 1,
                FirstName = "Jimmie",
                LastName = "Peters",
                Email = "jpeters@gmail.com",
                Bids = new List<Bid>()
            };
            return buyer;
        }

        [Test]
        public void ValidBuyer_IsValid()
        {
            // Arrange
            Buyer buyer = MakeValidBuyer();

            // Act
            ModelValidator mv = new ModelValidator(buyer);

            // Assert
            Assert.That(mv.Valid, Is.True);
        }

        [Test]
        public void Buyer_WithMissingFirstName_IsNotValid()
        {
            // Arrange
            Buyer buyer = MakeValidBuyer();
            buyer.FirstName = null!;

            // Act
            ModelValidator mv = new ModelValidator(buyer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(mv.Valid, Is.False);
                Assert.That(mv.ContainsFailureFor("FirstName"), Is.True);
            });
        }

        [Test]
        public void Buyer_WithEmptyStringForFirstName_IsNotValid()
        {
            // Arrange
            Buyer buyer = MakeValidBuyer();
            buyer.FirstName = "";

            // Act
            ModelValidator mv = new ModelValidator(buyer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(mv.Valid, Is.False);
                Assert.That(mv.ContainsFailureFor("FirstName"), Is.True);
            });
        }

        [Test]
        public void Buyer_WithTooLongFirstName_IsNotValid()
        {
            // Arrange
            Buyer buyer = MakeValidBuyer();
            buyer.FirstName = "PhilipJohannaDeweyKarlCoryConnieBrendanLolaJimmieVivian";

            // Act
            ModelValidator mv = new ModelValidator(buyer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(mv.Valid, Is.False);
                Assert.That(mv.ContainsFailureFor("FirstName"), Is.True);
            });
        }

        // Continue on in this fashion, both positive and negative cases
    }
}

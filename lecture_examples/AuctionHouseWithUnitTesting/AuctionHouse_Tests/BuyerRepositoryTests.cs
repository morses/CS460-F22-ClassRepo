using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.DAL.Concrete;
using AuctionHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Castle.Components.DictionaryAdapter;

namespace AuctionHouse_Tests
{
    public class BuyerRepositoryTests
    {
        private Mock<AuctionHouseDbContext> _mockContext;
        private Mock<DbSet<Buyer>> _mockBuyerDbSet;
        private List<Buyer> _buyers;
        private List<Item> _items;
        private List<Bid> _bids;

        [SetUp]
        public void Setup()
        {
            _buyers = new List<Buyer>
            {
                new Buyer {Id = 1, FirstName = "Jane", LastName = "Stone", Email = "jstone@gmail.com"},
                new Buyer {Id = 2, FirstName = "Tom", LastName = "McMasters", Email = "tom@yahoo.com"},
                new Buyer {Id = 3, FirstName = "Otto", LastName = "Vanderwall", Email = "otto@otto.com"}
            };
            // For the current contents of BuyerRepository we don't need to mock Items and Bids, but doing it anyway to show how to do it
            _items = new List<Item>
            {
                new Item {Id = 1, Name = "Abraham Lincoln Hammer", Description = "A bench mallet fashioned from a broken rail-splitting maul in 1829 and owned by Abraham Lincoln", Condition = "Lightly used",SellerId = 3},
                new Item {Id = 2, Name = "Albert Einsteins Telescope", Description = "A brass telescope owned by Albert Einstein in Germany, circa 1927", Condition = "Mint in box", SellerId = 1},
                new Item {Id = 3, Name = "Bob Dylan Love Poems", Description = "Five versions of an original unpublished, handwritten, love poem by Bob Dylan", Condition = "Over used", SellerId = 2}
            };
            _bids = new List<Bid>
            {
                new Bid {Id = 1, ItemId = 1, BuyerId = 3, Price = 250000M, TimeSubmitted = new DateTime(2017,12,4,9,4,22)},
                new Bid {Id = 2, ItemId = 3, BuyerId = 1, Price = 95000M, TimeSubmitted = new DateTime(2017,12,4,8,44,3)}
            };
            // All ID's are set, but presently all navigation properties are null.  Let's set them (if we need them that is)
            // You can set them manually
            // _buyers[0].Bids = new List<Bid>{ _bids[1] };
            // _buyers[1].Bids = new List<Bid>();
            // _buyers[2].Bids = new List<Bid>{ _bids[0] };
            // or easier with LINQ
            _buyers.ForEach(b =>
            {
                b.Bids = _bids.Where(bid => bid.ItemId == b.Id).ToList();       // one to many bids
            });
            _items.ForEach(i =>
            {
                //i.Seller = _sellers.Single(s => s.Id == i.SellerId);      // we didn't mock sellers yet
                i.Bids = _bids.Where(bid => bid.ItemId == i.Id).ToList();   // one to many bids
            });
            _bids.ForEach(b =>
            {
                b.Buyer = _buyers.Single(buyer => buyer.Id == b.BuyerId);   // to one buyer
                b.Item = _items.Single(i => i.Id == b.ItemId);              // to one item
            });
            // Finally, mock the context and dbsets
            _mockContext = new Mock<AuctionHouseDbContext>();
            _mockBuyerDbSet = MockHelpers.GetMockDbSet(_buyers.AsQueryable());
            _mockContext.Setup(ctx => ctx.Buyers).Returns(_mockBuyerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Buyer>()).Returns(_mockBuyerDbSet.Object);
        }

        [Test]
        public void NumberOfBuyers_WithThreeBuyers_ReturnsThree()
        {
            // Arrange
            IBuyerRepository buyerRepository = new BuyerRepository(_mockContext.Object);
            int expected = 3;

            // Act
            int actual = buyerRepository.NumberOfBuyers();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void NumberOfBuyers_WithNoBuyers_ReturnsZero()
        {
            // Arrange
            // need to manually set this one up because it's different than the standard one
            // if you need to do this frequently, put it in a private helper method
            _buyers.Clear();
            int expected = 0;
            _mockContext = new Mock<AuctionHouseDbContext>();
            _mockBuyerDbSet = MockHelpers.GetMockDbSet(_buyers.AsQueryable());
            _mockContext.Setup(ctx => ctx.Buyers).Returns(_mockBuyerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Buyer>()).Returns(_mockBuyerDbSet.Object);
            IBuyerRepository buyerRepository = new BuyerRepository(_mockContext.Object);

            // Act
            int actual = buyerRepository.NumberOfBuyers();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmailList_WithValidBuyers_ReturnsCorrectEmailList()
        {
            // Arrange
            IBuyerRepository buyerRepository = new BuyerRepository(_mockContext.Object);
            List<string> expected = new string[] { "tom@yahoo.com", "jstone@gmail.com", "otto@otto.com"}.ToList();
            
            // Email list should be ordered by the buyer's last name

            // Act
            List<string> actual = buyerRepository.EmailList();

            // Assert
            // NUnit 3.0 can compare collections for equality, same size and same elements in order
            // see: https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.AreEqual.html
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EmailList_WithNoBuyers_ReturnsEmptyEmailList()
        {
            // Arrange
            _buyers.Clear();
            _mockContext = new Mock<AuctionHouseDbContext>();
            _mockBuyerDbSet = MockHelpers.GetMockDbSet(_buyers.AsQueryable());
            _mockContext.Setup(ctx => ctx.Buyers).Returns(_mockBuyerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Buyer>()).Returns(_mockBuyerDbSet.Object);
            IBuyerRepository buyerRepository = new BuyerRepository(_mockContext.Object);
            List<string> expected = new string[] {  }.ToList();

            // Email list should be ordered by the buyer's last name

            // Act
            List<string> actual = buyerRepository.EmailList();

            // Assert
            // NUnit 3.0 can compare collections for equality, same size and same elements in order
            // see: https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.AreEqual.html
            Assert.AreEqual(expected, actual);
        }
    }
}

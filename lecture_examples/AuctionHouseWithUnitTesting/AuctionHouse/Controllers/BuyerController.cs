using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Models;
using AuctionHouse.Models.DTO;
using AuctionHouse.DAL.Abstract;

namespace AuctionHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        //private readonly AuctionHouseDbContext _context;
        private readonly IBuyerRepository _buyerRepository;

        //public BuyerController(AuctionHouseDbContext context)
        public BuyerController(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
            //_context = context;
        }

        // GET: api/Buyer
        [HttpGet]
        public ActionResult<IEnumerable<BuyerDTO>> GetBuyers()
        {
            List<Buyer> buyers = _buyerRepository.GetAll().ToList();
            List<BuyerDTO> buyersDTO = buyers.Select(b => new BuyerDTO(b, b.Bids.Count)).ToList();
            // To be sure and avoid cycles, set all navigation properties to null (id's will still be there though)
            buyersDTO.ForEach(b => b.Buyer.Bids = null!);
            return buyersDTO;
        }
/*
        // GET: api/Buyer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Buyer>> GetBuyer(int id)
        {
          if (_context.Buyers == null)
          {
              return NotFound();
          }
            var buyer = await _context.Buyers.FindAsync(id);

            if (buyer == null)
            {
                return NotFound();
            }

            return buyer;
        }

        // PUT: api/Buyer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuyer(int id, Buyer buyer)
        {
            if (id != buyer.Id)
            {
                return BadRequest();
            }

            _context.Entry(buyer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Buyer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Buyer>> PostBuyer(Buyer buyer)
        {
          if (_context.Buyers == null)
          {
              return Problem("Entity set 'AuctionHouseDbContext.Buyers'  is null.");
          }
            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuyer", new { id = buyer.Id }, buyer);
        }

        // DELETE: api/Buyer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyer(int id)
        {
            if (_context.Buyers == null)
            {
                return NotFound();
            }
            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }

            _context.Buyers.Remove(buyer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuyerExists(int id)
        {
            return (_context.Buyers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
*/
    }
}

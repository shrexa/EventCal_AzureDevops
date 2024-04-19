using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Events.Calendar.Data;
using Microsoft.AspNetCore.Authorization;
using DomainNs = Events.Calendar.Domain;
using ModelNs = Events.Calendar.Model;

namespace Events.Calendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CountryController : ControllerBase
    {
        private readonly EventsCalendarContext _context;

        public CountryController(EventsCalendarContext context)
        {
            _context = context;
        }

        //Test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelNs.CountryModel>>> GetCountry()
        {
            var countries = await _context.Country.ToListAsync();
            var countriesModel = countries.Select(c => new ModelNs.CountryModel()
            {
                Id = c.Id,
                Name = c.Name,
                IsoCode = c.IsoCode
            });
            return Ok(countriesModel);
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelNs.CountryModel>> GetCountry(Guid id)
        {
            var Country = await _context.Country.FindAsync(id);

            if (Country == null)
            {
                return NotFound();
            }

            var countriesModel = new ModelNs.CountryModel()
            {
                Id = Country.Id,
                Name = Country.Name,
                IsoCode = Country.IsoCode
            };
            return Ok(countriesModel);
        }

        //PUT: api/Country/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(Guid id, ModelNs.CountryModel Country)
        {
            if (id != Country.Id)
            {
                return BadRequest();
            }

            //Country.RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks));

            var CountryDb = await _context.Country.FindAsync(id);
            if (CountryDb == null)
            {
                return BadRequest();
            }
            CountryDb.Name = Country.Name;
            CountryDb.IsoCode = Country.IsoCode;
            CountryDb.ModifiedOn = DateTime.UtcNow;
            CountryDb.ModifiedBy = "System";
            CountryDb.RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks));

            _context.Country.Update(CountryDb);
            _context.Entry(CountryDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        //POST: api/Country
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModelNs.CountryModel>> PostCountry(ModelNs.CountryModel Country)
        {
            var CountryDb = new DomainNs.Country
            {
                //Id = Country.Id,
                Name = Country.Name,
                IsoCode = Country.IsoCode,
                StatusCode = DomainNs.StatusCodeEnum.Active,
                StatusSubCode = DomainNs.StatusSubCodeEnum.New,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks))
            };
            _context.Country.Add(CountryDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = Country.Id }, Country);
        }

        // DELETE: api/Country/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCountry(Guid id)
        //{
        //    var Country = await _context.Country.FindAsync(id);
        //    if (Country == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Country.Remove(Country);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CountryExists(Guid id)
        {
            return _context.Country.Any(e => e.Id == id);
        }
    }
}

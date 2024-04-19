using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Events.Calendar.Data;
using ModelNs = Events.Calendar.Model;
using DomainNs = Events.Calendar.Domain;

namespace Events.Calendar.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    //[Authorize]
    public class CityController : ControllerBase
    {
        private readonly EventsCalendarContext _context;
        public CityController(EventsCalendarContext context)
        {
            _context = context;
        }

        //GET: api/City
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelNs.CityModel>>> GetCity()
        {
            var cities = await _context.City.ToListAsync();
            var citiesModel = cities.Select(c2 => new ModelNs.CityModel()
            {
                Id = c2.Id,
                Name = c2.Name,
                IsoCode = c2.IsoCode
            });
            return Ok(citiesModel);
        }


        //xyz
        // GET: api/City/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelNs.CityModel>> GetCity(Guid id)
        {
            var City = await _context.City.FindAsync(id);

            if (City == null)
            {
                return NotFound();
            }

            var citiesModel = new ModelNs.CityModel()
            {
                Id = City.Id,
                Name = City.Name,
                IsoCode = City.IsoCode
            };
            return Ok(citiesModel);
        }



        //PUT: api/City/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, ModelNs.CityModel City)
        {
            if (id != City.Id)
            {
                return BadRequest();
            }

            var CityDb = await _context.City.FindAsync(id);
            if (CityDb == null)
            {
                return BadRequest();
            }
            CityDb.Name = City.Name;
            CityDb.IsoCode = City.IsoCode;
            CityDb.CountryId = City.CountryId;
            CityDb.ModifiedOn = DateTime.UtcNow;
            CityDb.ModifiedBy = "System";
            CityDb.RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks));

            _context.City.Update(CityDb);
            _context.Entry(CityDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
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



        //POST: api/City
        [HttpPost]
        public async Task<ActionResult<ModelNs.CityModel>> PostCity(ModelNs.CityModel City)
        {
            var CityDb = new DomainNs.City
            {
                //Id = City.Id,
                Name = City.Name,
                IsoCode = City.IsoCode,
                StatusCode = DomainNs.StatusCodeEnum.Active,
                StatusSubCode = DomainNs.StatusSubCodeEnum.New,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks))
            };
            _context.City.Add(CityDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = City.Id }, City);
        }


        private bool CityExists(Guid id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
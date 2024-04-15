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
    public class LocationController : ControllerBase
    {
        private readonly EventsCalendarContext _context;
        public LocationController(EventsCalendarContext context)
        {
            _context = context;
        }

        //GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelNs.LocationModel>>> GetLocation()
        {
            var locations = await _context.Location.ToListAsync();
            var locationsModel = locations.Select(c2 => new ModelNs.LocationModel()
            {
                Id = c2.Id,
                Name = c2.Name,
                IsoCode = c2.IsoCode
            });
            return Ok(locationsModel);
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelNs.LocationModel>> GetLocation(Guid id)
        {
            var Location = await _context.Location.FindAsync(id);

            if (Location == null)
            {
                return NotFound();
            }

            var locationsModel = new ModelNs.LocationModel()
            {
                Id = Location.Id,
                Name = Location.Name,
                IsoCode = Location.IsoCode
            };
            return Ok(locationsModel);
        }
        
        //PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(Guid id, ModelNs.LocationModel Location)
        {
            if (id != Location.Id)
            {
                return BadRequest();
            }

            var LocationDb = await _context.Location.FindAsync(id);
            if (LocationDb == null)
            {
                return BadRequest();
            }
            LocationDb.Name = Location.Name;
            LocationDb.Code = Location.Code;
            LocationDb.CountryId = Location.CountryId;
            LocationDb.ModifiedOn = DateTime.UtcNow;
            LocationDb.ModifiedBy = "System";
            LocationDb.RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks));

            _context.Location.Update(LocationDb);
            _context.Entry(LocationDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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


        //POST: api/Location
        [HttpPost]
        public async Task<ActionResult<ModelNs.LocationModel>> PostLocation(ModelNs.LocationModel Location)
        {
            var LocationDb = new DomainNs.Location
            {
                Id = Location.Id,
                Name = Location.Name,
                Code = Location.Code,
                Description = Location.Description,
                StatusCode = DomainNs.StatusCodeEnum.Active,
                StatusSubCode = DomainNs.StatusSubCodeEnum.New,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks))
            };
            _context.Location.Add(LocationDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = Location.Id }, Location);
        }


        private bool LocationExists(Guid id)
        {
            return _context.Location.Any(e => e.Id == id);
        }


        //PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(Guid id, ModelNs.LocationModel Location)
        {
            if (id != Location.Id)
            {
                return BadRequest();
            }

            var LocationDb = await _context.Location.FindAsync(id);
            if (LocationDb == null)
            {
                return BadRequest();
            }
            LocationDb.Name = Location.Name;
            LocationDb.IsoCode = Location.IsoCode;
            LocationDb.CountryId = Location.CountryId;
            LocationDb.ModifiedOn = DateTime.UtcNow;
            LocationDb.ModifiedBy = "System";
            LocationDb.RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks));

            _context.Location.Update(LocationDb);
            _context.Entry(LocationDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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


        //POST: api/Location
        [HttpPost]
        public async Task<ActionResult<ModelNs.LocationModel>> PostLocation(ModelNs.LocationModel Location)
        {
            var LocationDb = new DomainNs.Location
            {
                //Id = Location.Id,
                Name = Location.Name,
                IsoCode = Location.IsoCode,
                StatusCode = DomainNs.StatusCodeEnum.Active,
                StatusSubCode = DomainNs.StatusSubCodeEnum.New,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                RowVersion = BitConverter.GetBytes(Convert.ToUInt64(DateTime.UtcNow.Ticks))
            };
            _context.Location.Add(LocationDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = Location.Id }, Location);
        }


        private bool LocationExists(Guid id)
        {
            return _context.Location.Any(e => e.Id == id);
        }
    }
}

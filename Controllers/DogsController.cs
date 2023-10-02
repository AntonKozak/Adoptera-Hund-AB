using adoptera_hund.api.Data;
using adoptera_hund.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adoptera_hund.api.Controllers;

[ApiController]
    [Route("api/v1/dogs")]
    public class DogsController : ControllerBase
    {
        private readonly AdopteraHundContext _data;
      
      public DogsController(AdopteraHundContext data)
      {
        _data = data;

      }
      
      [HttpGet()]
      
        public async Task<ActionResult> GetAllDogs()
        {
            var result = await _data.DogsDataBase.ToListAsync();
            return Ok(result);
        }

      [HttpGet("{id}")]
      [Authorize(Roles = "Admin, Customer, User")]
        public async Task<ActionResult> GetByName(int id)
        {
            var result = await _data.DogsDataBase.FindAsync(id);
            return Ok(result);
        }

      [HttpGet("name/{name}")]
      [Authorize(Roles = "Admin, Customer, User")]
        public async Task<ActionResult> GetByName(string name)
        {
            var result = await _data.DogsDataBase.Select(d => new DogsListViewModel{
                Id = d.Id,
                Name = d.Name,
                Age = d.Age,
                Race = d.Race,
                Gender = d.Gender,
                City = d.City,
                Description = d.Description
            }).SingleOrDefaultAsync(n => n.Name!.ToUpper().Trim() == name.ToUpper().Trim());
            return Ok(result);
        }

      [HttpGet("race/{race}")]
      [Authorize(Roles = "Admin, Customer, User")]
        public async Task<ActionResult> GetByRace(string race)
        {
            var result = await _data.DogsDataBase.Select(d => new DogsListViewModel{
                Id = d.Id,
                Name = d.Name,
                Age = d.Age,
                Race = d.Race,
                Gender = d.Gender,
                City = d.City,
                Description = d.Description
            }).SingleOrDefaultAsync(n => n.Race!.ToUpper().Trim() == race.ToUpper().Trim());
            return Ok(result);
        }

        
        [HttpPost()]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult AddDog ()
        {   
            if(!ModelState.IsValid) return BadRequest("Information is required, please write all information its important");
            
            return Created(nameof(GetByName), new{ message = "AddDog"});
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult UpdateDog(int id)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult DeleteDog(int id)
        {
            return NoContent();
        }

        [HttpPatch ("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult MarkAsImportent(int id)
        {
            return NoContent();
        }
    }

using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context) {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        // [HttpGet]
        // public IEnumerable<Pet> GetPets() {
        //     return new List<Pet>();
        // }

        [HttpGet]
        public IEnumerable<Pet> GetAllPets() {
            return _context.Pets
            .Include(pet => pet.petOwner)
            .ToList();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Pet pet) {
            _context.Pets.Add(pet);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAllPets), pet);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePet(int id) {
            Pet petToDelete = _context.Pets.Find(id);
            if (petToDelete == null) return NotFound();

            _context.Pets.Remove(petToDelete);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/checkin")]
        public IActionResult CheckIn(int id) {
            Pet pet = _context.Pets
            .SingleOrDefault( p => p.id == id); // Why SingleOrDefault necessary?

            if(pet == null) return NotFound();

            pet.checkedInAt = DateTime.UtcNow;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}/checkout")]
        public IActionResult CheckOut(int id) {
            Pet pet = _context.Pets
            .SingleOrDefault( p => p.id == id);

            if(pet == null) return NotFound();

            pet.checkedInAt = null;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}")]
        public IActionResult EditPet(int id, [FromBody] Pet pet) {


            _context.Update(pet);
            _context.SaveChanges();

            return Ok(pet);
        }

        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<Pet> GetPets() {
        //     PetOwner blaine = new PetOwner{
        //         name = "Blaine"
        //     };

        //     Pet newPet1 = new Pet {
        //         name = "Big Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     Pet newPet2 = new Pet {
        //         name = "Little Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<Pet>{ newPet1, newPet2};
        // }
    }
}

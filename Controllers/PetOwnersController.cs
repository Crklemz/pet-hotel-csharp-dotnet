using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context) {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        // [HttpGet]
        // public IEnumerable<PetOwner> GetPets() {
        //     return new List<PetOwner>();
        // }

        [HttpGet]
        public IEnumerable<PetOwner> GetAllPetOwners() {
            var owners = _context.PetOwners.ToList();
            return owners;
        }

        [HttpGet(("{id}"))]
        public IActionResult GetById(int id) {
            PetOwner owner = _context.PetOwners
                .Include(o => o.pets)
                .SingleOrDefault( owner => owner.id == id);
            if (owner == null) return NotFound();
            return Ok(owner);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PetOwner owner) {
            _context.Add(owner);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAllPetOwners), owner);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePetOwner(int id) {
            PetOwner ownerToDelete = _context.PetOwners.Find(id);
            if (ownerToDelete == null) return NotFound();

            _context.PetOwners.Remove(ownerToDelete);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult EditOwner(int id, [FromBody] PetOwner owner) {


            _context.Update(owner);
            _context.SaveChanges();

            return Ok(owner);
        }
    }
}

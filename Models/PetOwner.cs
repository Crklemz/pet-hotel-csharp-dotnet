using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace pet_hotel
{
    public class PetOwner {

        public int id {get; set;}

        [Required]
        public string name {get; set;}

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string emailAddress {get; set;}

        [JsonIgnore]
        public ICollection<Pet> pets {get; set;}

        [NotMapped]
        // public int petCount {get;} --> throws a 0 on the DOM - why? Ask in class
        public int petCount {
            get {
                return (pets == null ? 0 : pets.Count);
            }
        }


    }
}

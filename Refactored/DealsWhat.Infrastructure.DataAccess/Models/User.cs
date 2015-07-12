using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealsWhat.Models
{
    [Table("AspNetUsers")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        //public string AddressLine1 { get; set; }
        //public string Street { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string ZipCode { get; set; }


        public virtual IList<Cart> Carts { get; set; }

        public User()
        {
            Carts = new List<Cart>();
        }
    }
}
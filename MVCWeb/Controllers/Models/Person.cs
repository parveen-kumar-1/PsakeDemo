using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Controllers
{
    public class Person
    {
        public int Id { get; set; }
        
        [StringLength(maximumLength: 20, ErrorMessage = "Name too long. Max 20 chars please.")]
        public string Name { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
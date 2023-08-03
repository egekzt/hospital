using System;
using System.ComponentModel.DataAnnotations;
namespace hospital.Models
{
    public class Secretary
    {
        [Key]
        public string? ssn { get; set; }
        public string? name { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}


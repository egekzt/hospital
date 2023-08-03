using System;
using System.ComponentModel.DataAnnotations;
namespace hospital.Models
{
    public class Doctor
    {
        public string? fullName { get; set; }
        [Key]
        public string? ssn { get; set; }
        public int? departmentId { get; set; }
        public string? phoneNumer { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        
    }
}


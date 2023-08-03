using System;
using System.ComponentModel.DataAnnotations;
namespace hospital.Models
{
    public class Department
    {
        [Key]
        public int? id { get; set; }
        public string? name { get; set; }
        public string? headOfDepartment { get; set; }
    }
}


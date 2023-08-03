using System;
using System.ComponentModel.DataAnnotations;
namespace hospital.Models
{
    public class Building
    {
        [Key]
        public string? adress { get; set; }
        public int? numberOfRooms { get; set; }
        public int? departmentId { get; set; }
    }
}


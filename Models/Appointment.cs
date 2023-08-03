using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospital.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        public string? doctorSsn { get; set; }
        public string? patientSsn { get; set; }
        public DateTime? date { get; set; }
        public string? adressOfBuilding { get; set; }
        public int? roomNumber { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor{ get; set; }
    }
}


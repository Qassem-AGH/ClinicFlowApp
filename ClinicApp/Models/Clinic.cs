using System;
using System.Collections.Generic;

namespace ClinicFlowApp.Models
{
    public partial class Clinic
    {
        public Clinic()
        {
            Doctors = new HashSet<Doctor>();
        }

        public int ClinicId { get; set; }
        public string ClinicName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
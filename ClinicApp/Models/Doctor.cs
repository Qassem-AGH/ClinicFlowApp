using System;
using System.Collections.Generic;

namespace ClinicFlowApp.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int DoctorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public int ClinicId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Clinic Clinic { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
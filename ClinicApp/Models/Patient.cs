using System;
using System.Collections.Generic;

namespace ClinicFlowApp.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int PatientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
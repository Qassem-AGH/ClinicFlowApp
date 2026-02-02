using System;
using System.Collections.Generic;

namespace ClinicFlowApp.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentTreatments = new HashSet<AppointmentTreatment>();
        }

        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<AppointmentTreatment> AppointmentTreatments { get; set; }
    }
}
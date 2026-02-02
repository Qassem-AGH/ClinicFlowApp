using System;
using System.Collections.Generic;

namespace ClinicFlowApp.Models
{
    public partial class Treatment
    {
        public Treatment()
        {
            AppointmentTreatments = new HashSet<AppointmentTreatment>();
        }

        public int TreatmentId { get; set; }
        public string TreatmentName { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<AppointmentTreatment> AppointmentTreatments { get; set; }
    }
}
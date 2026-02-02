using System;

namespace ClinicFlowApp.Models
{
    public partial class AppointmentTreatment
    {
        public int AppointmentId { get; set; }
        public int TreatmentId { get; set; }

        public virtual Appointment Appointment { get; set; } = null!;
        public virtual Treatment Treatment { get; set; } = null!;
    }
}
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ClinicFlowApp.Models;

namespace Clinic.Models
{
    public partial class ClinicFlowDbContext : DbContext
    {
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<AppointmentTreatment> AppointmentTreatments { get; set; } = null!;
        public virtual DbSet<ClinicFlowApp.Models.Clinic> Clinics { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Treatment> Treatments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ⚠️ VARNING: Hårdkodad connection string - använd appsettings.json i produktion!
                optionsBuilder.UseSqlServer("Server=WIN-F1BEDHK5AQC\\SQLEXPRESS01;Database=ClinicDB;Integrated Security=true;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===================================
            // APPOINTMENT CONFIGURATION
            // ===================================
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId);

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('Scheduled')");

                // Foreign Key: Appointment -> Doctor
                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Doctor");

                // Foreign Key: Appointment -> Patient
                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Patient");
            });

            // ===================================
            // APPOINTMENT TREATMENT CONFIGURATION (Many-to-Many)
            // ===================================
            modelBuilder.Entity<AppointmentTreatment>(entity =>
            {
                // Composite Primary Key
                entity.HasKey(e => new { e.AppointmentId, e.TreatmentId })
                    .HasName("PK_AppointmentTreatments");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");

                // Foreign Key: AppointmentTreatment -> Appointment
                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.AppointmentTreatments)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AppointmentTreatments_Appointment");

                // Foreign Key: AppointmentTreatment -> Treatment
                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.AppointmentTreatments)
                    .HasForeignKey(d => d.TreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppointmentTreatments_Treatment");
            });

            // ===================================
            // CLINIC CONFIGURATION
            // ===================================
            modelBuilder.Entity<ClinicFlowApp.Models.Clinic>(entity =>
            {
                entity.HasKey(e => e.ClinicId);

                entity.ToTable("Clinic");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.ClinicName).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            // ===================================
            // DOCTOR CONFIGURATION
            // ===================================
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId);

                entity.ToTable("Doctor");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Specialization).HasMaxLength(100);

                // Foreign Key: Doctor -> Clinic
                entity.HasOne(d => d.Clinic)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctor_Clinic");
            });

            // ===================================
            // PATIENT CONFIGURATION
            // ===================================
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                // Unique constraint on Email
                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            // ===================================
            // TREATMENT CONFIGURATION
            // ===================================
            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.HasKey(e => e.TreatmentId);

                entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TreatmentName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
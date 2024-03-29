﻿using Doctorify.Domain.Models.Enums;

namespace Doctorify.Domain.Models.Entities;

public class Appointment : BaseEntity
{
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    
    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public long PatientId { get; set; }
    public Patient Patient { get; set; }
    
    public byte[] RowVersion { get; set; }
}
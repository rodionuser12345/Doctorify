namespace Doctorify.Domain.Models.Dtos;

public class AppointmentDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public short Cabinet { get; set; }
    public long DoctorId { get; set; }
    public long PatientId { get; set; }
}
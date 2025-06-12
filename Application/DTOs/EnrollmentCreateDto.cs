namespace Application.DTOs;

public class EnrollmentCreateDto
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}

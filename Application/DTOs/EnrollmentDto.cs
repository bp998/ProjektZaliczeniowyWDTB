namespace Application.DTOs;

public class EnrollmentDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
}

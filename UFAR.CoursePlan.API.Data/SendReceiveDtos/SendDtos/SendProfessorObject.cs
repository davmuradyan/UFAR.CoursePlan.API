namespace UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

public record SendProfessorObject {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
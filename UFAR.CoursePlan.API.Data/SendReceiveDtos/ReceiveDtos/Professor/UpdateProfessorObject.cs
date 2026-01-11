namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public record UpdateProfessorObject {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
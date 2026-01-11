namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public record CreateProfessorObject {
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}
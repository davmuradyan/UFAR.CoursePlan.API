namespace UFAR.CoursePlan.API.SendReceiveDtos.SendDtos;

public record SendLoapItemObject {
    public required int Id { get; set; }
    public string Value { get; set; } = null!;
}
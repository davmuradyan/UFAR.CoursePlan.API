namespace UFAR.CoursePlan.API.Data.SendReceiveDtos.ReceiveDtos;

public record UpdateLoapItemObject {
    public required int Id { get; init; }
    public string? Value { get; init; }
}
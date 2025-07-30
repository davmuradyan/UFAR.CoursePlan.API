namespace UFAR.CoursePlan.API_Core.CreatingDtos {
    public record CreateProfDto {
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
    }
}
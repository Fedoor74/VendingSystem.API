namespace VendingSystem.API.DTOs
{
    public record RegisterRequest(string FullName, string Email, string? Phone, string Password);
}
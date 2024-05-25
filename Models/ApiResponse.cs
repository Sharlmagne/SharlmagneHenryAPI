namespace SharlmagneHenryAPI.Models;

public record ApiResponse<T>(int Status, string? Message, T? Data);

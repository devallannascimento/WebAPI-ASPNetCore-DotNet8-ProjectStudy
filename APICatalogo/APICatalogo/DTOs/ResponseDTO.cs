using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ResponseDTO
{
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
}

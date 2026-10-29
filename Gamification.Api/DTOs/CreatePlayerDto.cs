using System.ComponentModel.DataAnnotations;

namespace Gamification.Api.DTOs;


public class CreatePlayerDto
{
    [Required(ErrorMessage = "Никнейм не может быть пустым.")]
    [StringLength(50, ErrorMessage = "Никнейм не длиннее 50 символов.")]
    public string Nickname { get; set; } = string.Empty;
}
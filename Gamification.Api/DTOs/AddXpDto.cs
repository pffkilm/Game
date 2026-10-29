using System.ComponentModel.DataAnnotations;

namespace Gamification.Api.DTOs;

public class AddXpDto
{
    [Range(1, 10000, ErrorMessage = "Опыт должен быть от 1 до 10000.")]
    public int Amount { get; set; }

    [Required(ErrorMessage = "Источник не может быть пустым.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Источник от 2 до 50 символов.")]
    [RegularExpression(@"^[A-Za-zА-Яа-яЁё0-9_]+$",
        ErrorMessage = "Источник: только буквы, цифры и подчёркивание.")]
    public string Source { get; set; } = string.Empty;
}
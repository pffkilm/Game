using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gamification.Api.DTOs;

// Вход: только проверка, что поля не пустые
public class AuthRequestDto
{
    [Required(ErrorMessage = "Email обязателен.")]
    [EmailAddress(ErrorMessage = "Неверный формат Email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен.")]
    public string Password { get; set; } = string.Empty;
}

// Регистрация: строгие правила + межполевая проверка
public class RegisterRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Никнейм обязателен.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Никнейм от 3 до 30 символов.")]
    public string Nickname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен.")]
    [EmailAddress(ErrorMessage = "Неверный формат Email.")]
    public string Email { get; set; } = string.Empty;

    [DefaultValue("Test1234")]
    [Required(ErrorMessage = "Пароль обязателен.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль от 8 до 100 символов.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
        ErrorMessage = "Пароль: минимум 8 символов, буква и цифра.")]
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (ConfirmPassword != Password)
            yield return new ValidationResult("Пароли не совпадают.", new[] { nameof(ConfirmPassword) });
    }
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
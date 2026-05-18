using System.ComponentModel.DataAnnotations;

namespace LangApp.Client.Models.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Zadejte platný e-mail.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné.")]
    [MinLength(6, ErrorMessage = "Heslo musí mít alespoň 6 znaků.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

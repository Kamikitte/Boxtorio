﻿using System.ComponentModel.DataAnnotations;

namespace Boxtorio.Models;

public sealed class CreateAccountModel
{
    public CreateAccountModel(string name, string email, string password, string retryPassword, string role)
    {
        Name = name;
        Email = email;
        Password = password;
        RetryPassword = retryPassword;
        Role = role;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; init; }

    [Required] [Compare(nameof(Password))]
    public string RetryPassword { get; set; }

    [Required]
    public string Role { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountDemo.Models;

public class UserProfileModel
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
    public List<string> Roles { get; set; } = [];
}
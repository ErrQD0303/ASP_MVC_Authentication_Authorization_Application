using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserStorage.Models;

public class User
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
}
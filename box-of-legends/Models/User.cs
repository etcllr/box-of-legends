using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace box_of_legends.Models;

public class User : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
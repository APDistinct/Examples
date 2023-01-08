using System;

namespace Chrono.Services.Models;

public class UserInfoIncome
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? Position { get; set; }
    public string? Country { get; set; }
    public string? Location { get; set; }

    public DateTime LocationFrom { get; set; }

    //public IEnumerable<string>? Department { get; set; }
    //public IEnumerable<string>? BusinessLine { get; set; }
    //public int LocationId { get; set; }
    //public DateTime LocationFrom { get; set; }
    public string Sid { get; set; } = null!;
    public bool Deleted { get; set; }
}
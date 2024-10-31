using Repositories.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Models;

public partial class ViroCureUser
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Role { get; set; }
    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}

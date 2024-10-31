using Repositories.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string Fullname { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public string Phone { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual ICollection<PersonVirus> PersonViruses { get; set; } = new List<PersonVirus>();

    public virtual ViroCureUser? User { get; set; }
}

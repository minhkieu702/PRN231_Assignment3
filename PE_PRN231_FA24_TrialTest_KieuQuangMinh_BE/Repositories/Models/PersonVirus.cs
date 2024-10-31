using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Models;

public partial class PersonVirus
{
    public int PersonId { get; set; }
 
    public int VirusId { get; set; }

    public double? ResistanceRate { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Virus Virus { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RequestModel
{
    public class VirusRequestModel
    {
        [Required]
        public string VirusName { get; set; }
        [Required]
        public double? ResistanceRate { get; set; }
    }
}

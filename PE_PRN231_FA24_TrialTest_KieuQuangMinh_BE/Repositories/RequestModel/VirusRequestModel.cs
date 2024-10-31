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
        public string VirusName { get; set; }

        [Range(0, 1, ErrorMessage = "Resistance Rate: Must be between 0 and 1")]
        public double? ResistanceRate { get; set; }
    }
}

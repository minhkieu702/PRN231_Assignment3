using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RequestModel
{
    public class PersonUpdateRequestModel
    {
        [Required]
        public string Fullname { get; set; } = null!;
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string Phone { get; set; } = null!;

        public List<VirusRequestModel?> Viruses { get; set; } = [];
    }
}

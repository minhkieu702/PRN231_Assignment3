using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RequestModel
{
    public class PersonRequestModel
    {
        [Required]
        public int PersonID { get; set; }
        [Required]
        public string Fullname { get; set; } = null!;

        [Required]
        public DateOnly BirthDay { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public string Phone { get; set; } = null!;

        public List<VirusRequestModel?> Viruses { get; set; } = [];
    }
}

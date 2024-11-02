using Repositories.Common;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
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

        [BirthdayValidation("01-01-2007")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+84\d{9}$", ErrorMessage = "Phone number must be in the format +84989xxxxxx.")]
        public string Phone { get; set; } = null!;

        public List<VirusRequestModel?> Viruses { get; set; } = [];
    }
}

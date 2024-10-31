using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ResponseModel
{
    public class PersonResponseModel
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Phone { get; set; }
        public List<VirusResponseModel> Viruses { get; set; }
    }
}

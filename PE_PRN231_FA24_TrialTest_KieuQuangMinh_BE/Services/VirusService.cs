using Repositories.Models;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VirusService
    {
        private readonly VirusRepository repo = new();

        public List<Virus> GetViruses() => repo.GetViruses();
    }
}

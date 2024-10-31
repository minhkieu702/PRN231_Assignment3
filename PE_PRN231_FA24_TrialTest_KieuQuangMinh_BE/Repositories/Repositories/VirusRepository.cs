using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class VirusRepository
    {
        public List<Virus> GetViruses()
        {
            ViroCureFal2024dbContext context = new();
            return context.Viruses.ToList();
        }
    }
}

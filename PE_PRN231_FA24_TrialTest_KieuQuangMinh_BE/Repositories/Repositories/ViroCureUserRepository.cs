using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ViroCureUserRepository
    {
        ViroCureFal2024dbContext _context;
        public ViroCureUser Login(ViroCureUser user)
        {
            try
            {
                _context = new();
                var result = _context.ViroCureUsers.FirstOrDefault(c => c.Email.Equals(user.Email) && c.Password.Equals(user.Password));
                if (result == null) throw new Exception("Invalid email or password");
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

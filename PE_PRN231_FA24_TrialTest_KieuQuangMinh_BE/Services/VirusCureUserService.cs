
using Repositories.Repositories;
using Repositories.Models;

namespace Services
{
    public class VirusCureUserService
    {
        ViroCureUserRepository repository = new();

        public ViroCureUser Login(ViroCureUser person) => repository.Login(person);
    }
}

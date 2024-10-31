using AutoMapper;
using Repositories.Models;
using Repositories.Repositories;
using Repositories.RequestModel;
using Repositories.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService
    {
        private readonly PersonRepository repository;

        public PersonService(IMapper mapper)
        {
            repository = new(mapper);
        }

        public int CreatePerson(PersonRequestModel person) => repository.CreatePerson(person);

        public PersonResponseModel GetPerson(int id) => repository.GetPerson(id);

        public List<PersonResponseModel> GetPersons() => repository.GetPersons();

        public void UpdatePerson(int id, PersonUpdateRequestModel person) => repository.UpdatePerson(id, person);

        public void DeletePerson(int id) => repository.DeletePerson(id);
    }
}

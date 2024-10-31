using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Repositories.RequestModel;
using Repositories.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PersonRepository
    {
        private readonly IMapper _mapper;
        ViroCureFal2024dbContext _context;
        public PersonRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int CreatePerson(PersonRequestModel person)
        {
            try
            {
                _context = new();
                _context.People.Add(new Person
                {
                    BirthDay = DateOnly.FromDateTime(person.BirthDay),
                    Fullname = person.Fullname,
                    PersonId = person.PersonID,
                    Phone = person.Phone
                });
                var viruses = _context.Viruses.ToList();
                foreach (var virus in person.Viruses)
                {
                    var existedVirus = viruses.FirstOrDefault(c => c.VirusName == virus.VirusName);

                    if (existedVirus != null)
                    {
                        _context.PersonViruses.Add(new PersonVirus
                        {
                            PersonId = person.PersonID,
                            VirusId = existedVirus.VirusId,
                            ResistanceRate = virus.ResistanceRate,
                        });
                    }
                }
                if (_context.SaveChanges() == 0)
                {
                    throw new Exception("Person and viruses added failed");
                }
                return person.PersonID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PersonResponseModel GetPerson(int id)
        {
            try
            {
                _context = new();
                var resposne = _context.People
                    .Include(c => c.PersonViruses)
                    .ThenInclude(c => c.Virus)
                    .AsNoTracking()
                    .FirstOrDefault(c => c.PersonId == id);
                if (resposne == null)
                {
                    throw new Exception("Not found");
                }
                return _mapper.Map<PersonResponseModel>(resposne);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PersonResponseModel> GetPersons()
        {
            try
            {
                _context = new();
                var resposne = _context.People
                    .Include(c => c.PersonViruses)
                    .ThenInclude(c => c.Virus)
                    .AsNoTracking()
                    .ToList();
                if (resposne == null)
                {
                    throw new Exception("Not found");
                }
                return resposne.Select(_mapper.Map<PersonResponseModel>).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePerson(int id, PersonUpdateRequestModel request)
        {
            _context = new();
            try
            {
                var person = _context.People.Include(c => c.PersonViruses)
                    .ThenInclude(c => c.Virus)
                    .FirstOrDefault(c => c.PersonId.Equals(id));
                if (person == null) throw new Exception("The person is not found");

                person.Fullname = request.Fullname;
                person.BirthDay = DateOnly.FromDateTime(request.BirthDay);
                _context.People.Update(person);

                var currentPersonViruese = _context.PersonViruses
                    .Include(c => c.Virus)
                    .Where(c => c.PersonId == id);

                var removePersonViruses = currentPersonViruese
                    .Where(c => !request.Viruses.Select(c => c.VirusName).Contains(c.Virus.VirusName)).ToList();

                _context.PersonViruses.RemoveRange(removePersonViruses);

                foreach (var virus in request.Viruses.DistinctBy(c => c.VirusName))
                {
                    var associatedVirus = currentPersonViruese
                    .FirstOrDefault(c => c.Virus.VirusName.Equals(virus.VirusName));
                    
                    var existedVirus = _context.Viruses.FirstOrDefault(c => c.VirusName.Equals(virus.VirusName));
                    if (existedVirus == null) throw new Exception(virus.VirusName + " is not found in system");

                    if (associatedVirus == null)
                    {
                        _context.PersonViruses.Add(new PersonVirus
                        {
                            VirusId = existedVirus.VirusId,
                            PersonId = id,
                            ResistanceRate = virus.ResistanceRate
                        });
                    }
                    else
                    {
                        associatedVirus.Virus = existedVirus;
                        _context.PersonViruses.Update(associatedVirus);
                    }
                }
                if (_context.SaveChanges() == 0)
                {
                    throw new Exception("Update failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeletePerson(int id)
        {
            try
            {
                _context = new();
                _context.PersonViruses.RemoveRange(_context.PersonViruses.Where(c => c.PersonId == id));
                _context.People.Remove(_context.People.Find(id));
                if (_context.SaveChanges() == 0)
                {
                    throw new Exception("Delete failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

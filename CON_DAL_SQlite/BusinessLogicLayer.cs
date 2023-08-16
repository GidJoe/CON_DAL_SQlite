using System.Collections.Generic;

namespace CON_DAL_SQlite
{
    public class BusinessLogicLayer
    {
        private readonly IPersonRepository _repository;

        public BusinessLogicLayer(IPersonRepository repository)
        {
            _repository = repository;
        }

        public long AddPerson(string name, string vorname, string geburtsdatum, int age, int urlaubstage, string wohnort)
        {
            return _repository.CreatePerson(name, vorname, geburtsdatum, age, urlaubstage, wohnort);
        }

        public List<Person> RetrieveAllPersons()
        {
            return _repository.GetAllPersons();
        }

        public Person RetrievePerson(long personId)
        {
            return _repository.GetPerson(personId);
        }

        public void ModifyPerson(long personId, string? name = null, string? vorname = null, string? geburtsdatum = null, int? age = null, int? urlaubstage = null, string? wohnort = null)
        {
            _repository.UpdatePerson(personId, name, vorname, geburtsdatum, age, urlaubstage, wohnort);
        }

        public void RemovePerson(long personId)
        {
            _repository.DeletePerson(personId);
        }
    }
}

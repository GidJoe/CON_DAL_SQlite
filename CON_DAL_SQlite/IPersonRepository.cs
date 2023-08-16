using Microsoft.Data.Sqlite;

namespace CON_DAL_SQlite
{
    public interface IPersonRepository
    {
        long CreatePerson(string name, string vorname, string geburtsdatum, int age, int urlaubstage, string wohnort);

        public List<Person> GetAllPersons();

        void UpdatePerson(long personId, string name = null, string vorname = null, string geburtsdatum = null, int? age = null, int? urlaubstage = null, string wohnort = null);

        public Person GetPerson(long personId);

        void DeletePerson(long personId);
    }
}

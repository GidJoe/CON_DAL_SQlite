using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CON_DAL_SQlite
{
    public class Person
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Vorname { get; set; }
        public string? Geburtsdatum { get; set; }
        public int Age { get; set; }
        public int Urlaubstage { get; set; }
        public string? Wohnort { get; set; }
    }
}

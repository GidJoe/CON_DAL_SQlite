using CON_DAL_SQlite;
using System;
using System.Collections.Generic;

namespace CON_DAL_SQlite
{
    class PresentationLayer
    {
        static void Main()
        {
            /*  Diesmal verwenden wir eine SQLite Datenbank im DataAccessLayer.
             * 
             * 
             * 
             * 
             * 
             */



            BusinessLogicLayer bll = new BusinessLogicLayer(new DataAccessLayer());

            // Beispielhafte Verwendung der Methoden


            // bll.RemovePerson(8);
            // bll.AddPerson("Müller", "Hans", "01.01.1990", 31, 30, "Musterstadt");
            // bll.ModifyPerson(46, "MüllerChanged", "Hans", "01.01.1990", 31, 30, "Musterstadt");

            List<Person> allPersons = bll.RetrieveAllPersons();
            ConsoleHelper.PrintTable(allPersons);
        }
    }


}


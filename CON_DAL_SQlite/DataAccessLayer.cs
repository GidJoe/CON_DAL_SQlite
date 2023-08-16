using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CON_DAL_SQlite
{
    public class DataAccessLayer : IPersonRepository
    {
        private readonly string _connectionString;



        /* Enviorment Variables
         * ____________________
         * Hier nutzen wir eine Evironment Variable, um die Connection String zu setzen.
         * Connection Strings sollten nicht unverschlüsselt im Code stehen, da dass ein potentielles Sicherheitsrisiko darstellt.
         * Darum ist es sinnvoll, diese in einer sogennanten Umgebungsvariable zu speichern.
         * 
         * 
         * setx VARIABLE_NAME "ConnectionString" /M
         * Mit diesem Windows CMD Befehl können wir eine Umgebungsvariable setzen auf Systemebene.
         * 
         * _connectionString = Environment.GetEnvironmentVariable("VARIABLE_NAME")
         *
         *  User Secrets
         *  ___________________
         *  Alternative könnte man im Rahmen der Entwicklung auch den UserSecrets von .NET Core nutzen.
         *  Rechtsklick auf das Projekt -> Geheime Benutzerschlüssel verwalten -> Neuen Schlüssel anlegen
         *  -> Per 
         * 
         */

        public DataAccessLayer()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddUserSecrets<DataAccessLayer>();

            IConfiguration configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            //_connectionString = Environment.GetEnvironmentVariable("MY_DB_CONNECTION_STRING")
            //                   ?? "Data Source=C:\\Users\\MWB\\OneDrive\\CloudRepos\\CON_DAL_SQlite\\CON_DAL_SQlite\\person.db";
        }

        /* Der Null-Coalescing-Operator (??) wird hier genutzt um einen Fallback Wert zu setzen, falls die Umgebungsvariable nicht gesetzt ist.
         * Das dient ausschileßlich der Demonstration und sollte nicht in der Praxis genutzt werden.
        */


        private SqliteConnection CreateConnection()
        {

            return new SqliteConnection(_connectionString);
        }

        private SqliteCommand CreateCommand(string query, SqliteConnection connection, Dictionary<string, object?> parameters = null)
        {
            var command = new SqliteCommand(query, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            return command;
        }

        #region CRUD

        public long CreatePerson(string name, string vorname, string geburtsdatum, int age, int urlaubstage, string wohnort)
        {
            string insertQuery = @"
                INSERT INTO Person (name, vorname, geburtsdatum, age, urlaubstage, wohnort)
                VALUES (@name, @vorname, @geburtsdatum, @age, @urlaubstage, @wohnort);
                SELECT last_insert_rowid();";

            using var connection = CreateConnection();
            connection.Open();

            var parameters = new Dictionary<string, object?>
            {
                { "@name", name },
                { "@vorname", vorname },
                { "@geburtsdatum", geburtsdatum },
                { "@age", age },
                { "@urlaubstage", urlaubstage },
                { "@wohnort", wohnort }
            };

            using var command = CreateCommand(insertQuery, connection, parameters);
            return (long)command.ExecuteScalar();
        }

        public Person GetPerson(long personId)
        {
            using var connection = CreateConnection();
            connection.Open();

            var parameters = new Dictionary<string, object?> { { "@id", personId } };
            using var command = CreateCommand("SELECT * FROM Person WHERE id = @id", connection, parameters);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Person
                {
                    Id = Convert.ToInt64(reader["Id"]),
                    Name = reader["Name"]?.ToString(),
                    Vorname = reader["Vorname"]?.ToString(),
                    Geburtsdatum = reader["Geburtsdatum"]?.ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Urlaubstage = Convert.ToInt32(reader["Urlaubstage"]),
                    Wohnort = reader["Wohnort"]?.ToString()
                };
            }

            return null;
        }

        public List<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            using var connection = CreateConnection();
            connection.Open();
            using var command = CreateCommand("SELECT * FROM Person", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                persons.Add(new Person
                {
                    Id = Convert.ToInt64(reader["Id"]),
                    Name = reader["Name"]?.ToString(),
                    Vorname = reader["Vorname"]?.ToString(),
                    Geburtsdatum = reader["Geburtsdatum"]?.ToString(),
                    Age = Convert.ToInt32(reader["Age"]),
                    Urlaubstage = Convert.ToInt32(reader["Urlaubstage"]),
                    Wohnort = reader["Wohnort"]?.ToString()
                });
            }

            return persons;
        }

        public void UpdatePerson(long personId, string? name = null, string? vorname = null, string? geburtsdatum = null, int? age = null, int? urlaubstage = null, string? wohnort = null)
        {
            string updateQuery = @"
            UPDATE Person SET 
                name = COALESCE(@name, name),
                vorname = COALESCE(@vorname, vorname),
                geburtsdatum = COALESCE(@geburtsdatum, geburtsdatum),
                age = COALESCE(@age, age),
                urlaubstage = COALESCE(@urlaubstage, urlaubstage),
                wohnort = COALESCE(@wohnort, wohnort)
            WHERE id = @id";

            var parameters = new Dictionary<string, object?>
            {
                { "@id", personId },
                { "@name", name },
                { "@vorname", vorname },
                { "@geburtsdatum", geburtsdatum },
                { "@age", age },
                { "@urlaubstage", urlaubstage },
                { "@wohnort", wohnort }
            };

            using var connection = CreateConnection();
            connection.Open();
            using var command = CreateCommand(updateQuery, connection, parameters);
            command.ExecuteNonQuery();
        }

        public void DeletePerson(long personId)
        {
            string deleteQuery = "DELETE FROM Person WHERE id = @id";

            using var connection = CreateConnection();
            connection.Open();

            var parameters = new Dictionary<string, object?> { { "@id", personId } };
            using var command = CreateCommand(deleteQuery, connection, parameters);
            command.ExecuteNonQuery();
        }
        #endregion CRUD
    }
}

using CON_DAL_SQlite;
using Spectre.Console;
using System.Collections.Generic;

public static class ConsoleHelper
{
    public static void PrintTable(List<Person> persons)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[bold yellow]ID[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Name[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Vorname[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Geburtsdatum[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Age[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Urlaubstage[/]").Centered())
            .AddColumn(new TableColumn("[bold yellow]Wohnort[/]").Centered());

        foreach (var person in persons)
        {
            table.AddRow(
                person.Id.ToString(),
                person.Name,
                person.Vorname,
                person.Geburtsdatum,
                person.Age.ToString(),
                person.Urlaubstage.ToString(),
                person.Wohnort
            );
        }

        AnsiConsole.Write(table);
    }
}

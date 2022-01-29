using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace ApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var responseAsStream = await client.GetStreamAsync("https://thronesapi.com/api/v2/Characters");

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            // Console.WriteLine(responseAsStream);

            var table = new ConsoleTable("Full Name", "Title", "Family");

            foreach (var person in characters)
            {
                table.AddRow(person.FullName, person.Title, person.Family);
            }

            table.Write();
        }
    }
}

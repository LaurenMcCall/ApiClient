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

        public static string PromptForString(string prompt)
        {
            Console.Write(prompt);
            var userInput = Console.ReadLine();

            return userInput;
        }

        static async Task Main(string[] args)
        {

            var client = new HttpClient();

            var genderSearch = PromptForString("Please type: Female or Male \n");

            var url = $"https://anapioficeandfire.com/api/characters?gender={genderSearch}";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            // Console.WriteLine(responseAsStream);

            var table = new ConsoleTable("Name", "Gender", "Culture", "Is Alive");

            foreach (var person in characters)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.IsAlive);
            }

            table.Write();
        }
    }
}

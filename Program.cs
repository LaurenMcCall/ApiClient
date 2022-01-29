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

        public static void DisplayMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("How would you like to view the characters? ");
            Console.WriteLine("[V]iew all available data ");
            Console.WriteLine("[G]ender");
            Console.WriteLine("[C]ulture ");
            Console.WriteLine("[A]nyone alive still? ");
            Console.WriteLine("");
        }

        static async Task Main(string[] args)
        {
            await ViewByGender();
        }

        private static async Task ViewByGender()
        {
            var client = new HttpClient();

            var genderSearch = PromptForString("Please type: Female or Male \n");

            var url = $"https://anapioficeandfire.com/api/characters?gender={genderSearch}";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            // Console.WriteLine(responseAsStream);

            var table = new ConsoleTable("Name", "Gender", "Culture", "Born");

            foreach (var person in characters)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born);
            }

            table.Write();
        }
    }
}

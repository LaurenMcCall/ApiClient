using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("[A]nyone still alive? ");
            Console.WriteLine("");
        }

        static async Task Main(string[] args)
        {
            var keepGoing = true;
            while (keepGoing)
            {
                DisplayMenu();
                var choice = Console.ReadLine().ToUpper();
                Console.WriteLine("");


                switch (choice)
                {
                    case "V":
                        await ViewAllCharacterData();
                        break;
                    case "G":
                        await ViewByGender();
                        break;
                    case "C":
                        break;
                    case "A":
                        break;
                }
            }
        }

        private static async Task ViewByGender()
        {
            var client = new HttpClient();
            var genderSearch = PromptForString("Please type: [F]emale or [M]ale \n").ToUpper();
            if (genderSearch == "F")
            {
                genderSearch = "Female";
            }
            else if (genderSearch == "M")
            {
                genderSearch = "Male";
            }
            else
            {
                Console.WriteLine("That's not a valid selection. ");
            }

            var url = $"https://anapioficeandfire.com/api/characters?gender={genderSearch}";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "");

            var table = new ConsoleTable("Name", "Gender", "Culture", "Born");

            foreach (var person in removeNullNames)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born);
            }

            table.Write();
        }

        private static async Task ViewAllCharacterData()
        {
            var client = new HttpClient();
            // var genderSearch = PromptForString("Please type: Female or Male \n");

            var url = $"https://anapioficeandfire.com/api/characters";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var table = new ConsoleTable("Name", "Gender", "Culture", "Born", "Died");

            foreach (var person in characters)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born, person.Died);
            }
            table.Write();
        }
    }
}

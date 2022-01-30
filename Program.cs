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
        public static void DisplayGreeting()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("       \\****__              ____                                              ");
            Console.WriteLine("         |    *****\\_      --/ *\\-__                                          ");
            Console.WriteLine("         /_          (_    ./ ,/----'                                         ");
            Console.WriteLine("           \\__         (_./  /                                                ");
            Console.WriteLine("              \\__           \\___----^__                                       ");
            Console.WriteLine("               _/   _                  \\                                      ");
            Console.WriteLine("        |    _/  __/ )\"\\ _____         *\\                                    ");
            Console.WriteLine("        |\\__/   /    ^ ^       \\____      )                                   ");
            Console.WriteLine("         \\___--\"                    \\_____ )                                  ");
            Console.WriteLine("                                          \"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("👑 ~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~ 👑");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("        Welcome to the Game of Thrones Database      ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("👑 ~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~ 👑");
            Console.ForegroundColor = ConsoleColor.White;

        }

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
            Console.WriteLine("[V]iew all available character data ");
            Console.WriteLine("[G]ender");
            Console.WriteLine("[C]ulture ");
            Console.WriteLine("[W]ho is with the gods now? ");
            Console.WriteLine("[S]earch by name ");
            Console.WriteLine("[Q]uit...and tell me that thing about the night...");
            Console.WriteLine("");
        }

        static async Task Main(string[] args)
        {
            DisplayGreeting();
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
                        await ViewByCulture();
                        break;
                    case "W":
                        await ViewByIsAlive();
                        break;
                    case "S":
                        await SearchByName();
                        break;
                    case "Q":
                        Console.WriteLine("Remember...The night is dark and full of terrors. ");
                        Console.WriteLine("");
                        Console.WriteLine("Now exiting ");
                        Console.WriteLine("");

                        keepGoing = false;
                        break;
                }
            }
        }

        private static async Task SearchByName()
        {
            var client = new HttpClient();
            var nameSearch = PromptForString("Which character would you like to view? \n").ToUpper();

            var url = $"https://anapioficeandfire.com/api/characters?name={nameSearch}&page=1&pageSize=50";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "").OrderBy(character => character.Name);

            var table = new ConsoleTable("Name", "Gender", "Culture", "Born", "Died");

            foreach (var person in removeNullNames)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born, person.Died);
            }

            table.Write();
        }

        private static async Task ViewByGender()
        {
            var client = new HttpClient();
            var genderSearch = PromptForString("Would you like to view the [F]emale or [M]ale characters? \n").ToUpper();
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

            var url = $"https://anapioficeandfire.com/api/characters?gender={genderSearch}&page=1&pageSize=50";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "").OrderBy(character => character.Name);

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

            var url = $"https://www.anapioficeandfire.com/api/characters?page=1-43&pageSize=50";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "").OrderBy(character => character.Name);

            var table = new ConsoleTable("Name", "Gender", "Culture", "Born", "Died");

            foreach (var person in removeNullNames)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born, person.Died);
            }
            table.Write();
        }

        private static async Task ViewByCulture()
        {
            var client = new HttpClient();
            var cultureSearch = PromptForString("Please make a selection: \n[Q]artheen, [D]ornish, [N]orthmen, [I]ronborn, [W]esteros, [B]raavosi, [D]othraki or [V]alyrian \n").ToUpper();
            cultureSearch = CulturesToSelect(cultureSearch);

            var url = $"https://anapioficeandfire.com/api/characters?culture={cultureSearch}&page=1&pageSize=50";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "").OrderBy(character => character.Name);

            var table = new ConsoleTable("Name", "Culture", "Gender", "Born", "Died");

            foreach (var person in removeNullNames)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born, person.Died);
            }
            table.Write();
        }

        private static string CulturesToSelect(string cultureSearch)
        {
            if (cultureSearch == "Q")
            {
                cultureSearch = "Qartheen";
            }
            if (cultureSearch == "D")
            {
                cultureSearch = "Dornish";
            }
            if (cultureSearch == "I")
            {
                cultureSearch = "Ironborn";
            }
            if (cultureSearch == "N")
            {
                cultureSearch = "Northmen";
            }
            if (cultureSearch == "W")
            {
                cultureSearch = "Westeros";
            }
            else if (cultureSearch == "B")
            {
                cultureSearch = "Braavosi";
            }
            else if (cultureSearch == "V")
            {
                cultureSearch = "Valyrian";
            }
            else if (cultureSearch == "D")
            {
                cultureSearch = "Dothraki";
            }
            else
            {
                Console.WriteLine("That's not a valid selection. ");
            }
            return cultureSearch;
        }

        private static async Task ViewByIsAlive()
        {
            var client = new HttpClient();

            var url = $"https://anapioficeandfire.com/api/characters?isAlive={false}&page=1&pageSize=50";
            var responseAsStream = await client.GetStreamAsync(url);

            var characters = await JsonSerializer.DeserializeAsync<List<Characters>>(responseAsStream);
            var removeNullNames = characters.Where(character => character.Name != "").OrderBy(character => character.Name);

            var table = new ConsoleTable("Name", "Gender", "Culture", "Born", "Died");

            foreach (var person in removeNullNames)
            {
                table.AddRow(person.Name, person.Gender, person.Culture, person.Born, person.Died);
            }
            table.Write();
        }
    }
}

using System;
using System.Data;
using System.Globalization;
using System.Text.Json;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("computers.json");
            Console.WriteLine(computersJson);


            // NEW VERSION OF JSON
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            if (computers != null)
            {
                foreach (Computer computer in computers)
                {
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    ) VALUES ('" + EscapeSingleQuote(computer.Motherboard)
                            + "','" + computer.HasWifi
                            + "','" + computer.HasLTE
                            + "','" + (computer.ReleaseDate != null ? computer.ReleaseDate.Value.ToString("yyyy-MM-dd") : "")
                            + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                            + "','" + EscapeSingleQuote(computer.VideoCard)
                    + "')\n";
                    dapper.ExecuteSql(sql);
                }
            }

            string computersCopySystem = JsonSerializer.Serialize(computers, options);

            File.WriteAllText("computersCopySystem.txt", computersCopySystem);

        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");
            return output;
        }

    }
}
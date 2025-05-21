using System;
using System.Data;
using System.Globalization;
using System.Text.Json;
using AutoMapper;
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

            string computersJson = File.ReadAllText("computersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(destination =>
                        destination.ComputerId, options =>
                            options.MapFrom(source =>
                                source.computer_id))
                    .ForMember(destination =>
                        destination.Motherboard, options =>
                            options.MapFrom(source =>
                                source.motherboard))
                    .ForMember(destination =>
                        destination.CPUCores, options =>
                            options.MapFrom(source =>
                                source.cpu_cores))
                    .ForMember(destination =>
                        destination.HasWifi, options =>
                            options.MapFrom(source =>
                                source.has_wifi))
                    .ForMember(destination =>
                        destination.HasLTE, options =>
                            options.MapFrom(source =>
                                source.has_lte))
                    .ForMember(destination =>
                        destination.ReleaseDate, options =>
                            options.MapFrom(source =>
                                source.release_date))
                    .ForMember(destination =>
                        destination.Price, options =>
                            options.MapFrom(source =>
                                source.price))
                    .ForMember(destination =>
                        destination.VideoCard, options =>
                            options.MapFrom(source =>
                                source.video_card));
            }));


            // Console.WriteLine(computersJson);


            // // NEW VERSION OF JSON
            // JsonSerializerOptions options = new JsonSerializerOptions()
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // };


            //  USING MAPPING
            IEnumerable<ComputerSnake>? computerSystemMapped = JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);

            if (computerSystemMapped != null)
            {
                IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computerSystemMapped);
                Console.WriteLine("Automapper Count: " + computerResult.Count());
                // foreach (Computer computer in computerResult)
                // {
                //     Console.WriteLine(computer.Motherboard);
                // }
            }

            // USING JSON PROPERTYNAME
            IEnumerable<Computer>? computerSystem = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);
            Console.WriteLine("JSON Property Count: " + computerSystem.Count()); 
            // foreach (Computer computer in computerSystem)
            // {
            //     Console.WriteLine(computer.Motherboard);
            // }

            // if (computers != null)
            // {
            //     foreach (Computer computer in computers)
            //     {
            //         string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //             Motherboard,
            //             HasWifi,
            //             HasLTE,
            //             ReleaseDate,
            //             Price,
            //             VideoCard
            //         ) VALUES ('" + EscapeSingleQuote(computer.Motherboard)
            //                 + "','" + computer.HasWifi
            //                 + "','" + computer.HasLTE
            //                 + "','" + (computer.ReleaseDate != null ? computer.ReleaseDate.Value.ToString("yyyy-MM-dd") : "")
            //                 + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
            //                 + "','" + EscapeSingleQuote(computer.VideoCard)
            //         + "')\n";
            //         dapper.ExecuteSql(sql);
            //     }
            // }

            // string computersCopySystem = JsonSerializer.Serialize(computers, options);

            // File.WriteAllText("computersCopySystem.txt", computersCopySystem);

        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");
            return output;
        }

    }
}
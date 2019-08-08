// ============================================================================
//   ______    _        _____                       _
//  |  ____|  | |      / ____|                     | |
//  | |__   __| | __ _| (___   __ _ _ __ ___  _ __ | | ___
//  |  __| / _` |/ _` |\___ \ / _` | '_ ` _ \| '_ \| |/ _ \
//  | |___| (_| | (_| |____) | (_| | | | | | | |_) | |  __/
//  |______\__,_|\__,_|_____/ \__,_|_| |_| |_| .__/|_|\___|
//                                           | |
//                                           |_|
// MIT License
//
// Copyright (c) 2017-2019 Sunny Chen (daxnet)
//
// ============================================================================

using Microsoft.Extensions.CommandLineUtils;
using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace dbinit
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "dbinit";
            app.FullName = "EdaSample Database Initialization Utility";
            app.HelpOption("-?|--help");
            app.VersionOption("-v|--version", () => Assembly.GetExecutingAssembly().GetName().Version.ToString());

            var scriptFileOption = app.Option("-s|--script", "The PostgreSQL script file for initializing the database.", CommandOptionType.SingleValue);
            var hostOption = app.Option("-h|--host", "The host name of the PostgreSQL server.", CommandOptionType.SingleValue);
            var portOption = app.Option("--port", "The port number of the PostgreSQL server.", CommandOptionType.SingleValue);
            var userIdOption = app.Option("-u|--uid", "The User ID for connecting the PostgreSQL database.", CommandOptionType.SingleValue);
            var passwordOption = app.Option("-p|--password", "The password for connecting the PostgreSQL database.", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var scriptFileName = string.Empty;
                if (scriptFileOption.HasValue())
                {
                    scriptFileName = scriptFileOption.Value();
                }
                else
                {
                    app.ShowHint();
                    return -1;
                }

                var host = hostOption.HasValue() ? hostOption.Value() : "localhost";
                var port = portOption.HasValue() ? Convert.ToInt32(portOption.Value()) : 5432;
                var userId = userIdOption.HasValue() ? userIdOption.Value() : "postgres";
                var password = passwordOption.HasValue() ? passwordOption.Value() : "password";
                var connectionString = $"User ID={userId}; Password={password}; Host={host}; Port={port};";

                return ExecutScript(connectionString, scriptFileName);
            });

            try
            {
                app.Execute(args);
                Console.WriteLine("Script executed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static int ExecutScript(string connectionString, string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("The specified script file doesn't exist.");
                return -1;
            }

            var script = File.ReadAllText(fileName);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = script;
                    command.CommandType = CommandType.Text;
                    return command.ExecuteNonQuery();
                }
            }
        }

        #endregion Private Methods
    }
}
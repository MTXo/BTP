using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BTP.DBConnection
{
    internal class databaseConnection
    {
        public async Task DatabaseCommand(string SQLCommand, Dictionary<string, object> parameters)
        {
            string myConnectionString = "server=127.0.0.1;uid=root;pwd=;database=tydzien_postaci";

            using (var myConnection = new MySqlConnection(myConnectionString))
            {
                try
                {
                    await myConnection.OpenAsync();

                    using (var myCommand = new MySqlCommand(SQLCommand, myConnection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                myCommand.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        await myCommand.ExecuteNonQueryAsync();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Błąd MySQL: " + ex.Message);
                }
            }
        }
        public async Task<MySqlDataReader> ExecuteReaderAsync(string SQLCommand, Dictionary<string, object> parameters)
        {
            string myConnectionString = "server=127.0.0.1;uid=root;pwd=;database=tydzien_postaci";

            var myConnection = new MySqlConnection(myConnectionString);
            await myConnection.OpenAsync();

            var myCommand = new MySqlCommand(SQLCommand, myConnection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    myCommand.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return await myCommand.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
        }
    }
}

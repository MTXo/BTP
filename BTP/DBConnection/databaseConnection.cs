using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BTP.DBConnection
{
    internal class databaseConnection
    {
        public async Task DatabaseCommand(string SQLCommand)
        {
            MySqlConnection myConnection = null; 
            string myConnectionString;
            myConnectionString = "server=127.0.0.1;uid=root;pwd=12345;database=test";

            try
            {
                myConnection = new MySqlConnection(myConnectionString);
                await myConnection.OpenAsync();

                MySqlCommand myCommand = new MySqlCommand
                {
                    Connection = myConnection,
                    CommandText = SQLCommand
                };
            }
            catch (MySqlException ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (myConnection != null && myConnection.State == System.Data.ConnectionState.Open)
                {
                    await myConnection.CloseAsync();
                }
            }
        }
    }
}

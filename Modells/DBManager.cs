using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Net.Mail;

namespace React__User_Control__API.Modells
{
    public class DBManager {

        private string host = "localhost";
        private string userID = "root";
        private string password = "Geheim123*";
        private string database = "ToDo_DB";

        private MySqlConnection connection;

        public DBManager() {
            string connectionString = $"server={host};userid={userID};password={password};database={database};";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }

        public ToDoResult CreateUser(string username, string email, string password, string saltbase64) {
            try {
                string sqlQuery = "INSERT INTO register(UserName, Password, Email, saltBase64) VALUES (@username, @password, " +
                    "@email, @saltbase64) ";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection)) {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@saltbase64", saltbase64);
                    command.ExecuteNonQuery();

                }
                return new ToDoResult(true, "", "Konto erstellt");
            }
            catch (Exception ex) {
                return new ToDoResult(false, ex.Message);
            }

        }
        public ToDoResult IsUserTaken(string username) {

            string sqlQuery = "SELECT Count(*) FROM register WHERE UserName = @username";
            try {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection)) {

                    command.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return new ToDoResult(true, "", count > 0);
                }
            }
            catch (Exception ex) {
                return new ToDoResult(false, "Could not fetch UserName from Table"); 
            }
        }
           
        public ToDoResult DomainCheck(string email) {
            try {
                var emailAddress = new MailAddress(email);

                if (emailAddress.Host == "bene-edu.ch") {
                    return new ToDoResult(true, "");
                } else return new ToDoResult(false, "Email must be 'bene-edu.ch'");
            }
            catch (Exception ex) {
                return new ToDoResult(false, "Could not Check Email");
            }
        }

    }
}

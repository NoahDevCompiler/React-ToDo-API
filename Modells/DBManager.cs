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

        public void CreateUser(string username, string email, string password, int id) {
            try {
                string sqlQuery = "INSERT INTO Registration(ID, UserName, Password, Email) VALUES (@id, @username, @password, " +
                    "@email) ";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection)) {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);

                }

            }
            catch (Exception ex) {

            }

        }
        public ToDoResult IsUserTaken(string username) {

            string sqlQuery = "SELECT Count(*) FROM Registration WHERE UserName = @username";
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

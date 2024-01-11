using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Net.Mail;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System.Data.SqlTypes;

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

        //Table ToDo

        public ToDoResult InsertToDO(string name, string description, string type, DateTime startdate, DateTime enddate, int? isfinished = null) {

            isfinished ??= 0;

            try {
                string sqlquery = "INSERT INTO todos (name, description, type, startdate, enddate) VALUES(@name, @description, @type, @startdate, @enddate)";

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlquery, connection)) {
                    sqlcmd.Parameters.AddWithValue("@name", name);
                    sqlcmd.Parameters.AddWithValue("@description", description);                  
                    sqlcmd.Parameters.AddWithValue("@type", type);
                    sqlcmd.Parameters.AddWithValue("@startdate", startdate);
                    sqlcmd.Parameters.AddWithValue("@enddate", enddate);
                    sqlcmd.Parameters.AddWithValue("@isFinished", isfinished);

                    sqlcmd.ExecuteNonQuery();
                    return new ToDoResult(true, sqlquery);
                }
            }
            catch {



                return new ToDoResult(false, "konnte nicht gefetcht werden" );
            }
            

        }
        public ToDoResult GetToDos(bool completedOnly = default) {
            try {
                string query = "SELECT Name, Description, Type, Startdate, Enddate FROM todos";

                if (completedOnly == true) query += " WHERE isfinished = 1";
                else if (completedOnly == false) query += " WHERE isfinished = 0";

                using(MySqlCommand sqlcmd = new MySqlCommand(query, connection)) {

                    List<Todo> todo = new List<Todo>();

                    var reader = sqlcmd.ExecuteReader();
                    
                    while (reader.Read()) {
                        todo.Add(new Todo(
                                reader.GetString("Name"),
                                reader.GetString("Description"),
                                reader.GetString("Type"),
                                reader.GetDateTime("Startdate"),
                                reader.GetDateTime("Enddate")
                     )) ;
                    }
                    reader.Close();
                    var jsonresult = JsonConvert.SerializeObject(todo);
                    return new ToDoResult(true, "", jsonresult);
                
                }
            }
            catch {
                return new ToDoResult(false, "blödian");
            }

        }
        public ToDoResult UpdateCompletion(int id, int isfinished) {
            try {
                string query = $"UPDATE todos SET isfinished = @isfinished WHERE ID = @ID;";

                using (MySqlCommand cmd = new MySqlCommand( query, connection))
                {
                    cmd.Parameters.AddWithValue("@isfinished", isfinished);
                    cmd.Parameters.AddWithValue("@ID", id);

                    cmd.ExecuteNonQuery();
                    return new ToDoResult(true, query);
                }
            }
            catch {
                return new ToDoResult(false, "could not update to database");
            }
        }


    }
}

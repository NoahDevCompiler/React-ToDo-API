﻿using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Net.Mail;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

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

                string sqlQuery = "INSERT INTO user_acc(UserName, Password, Email, saltBase64) VALUES (@username, @password, " +
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
                return new ToDoResult(false, "Database failed: " + ex.Message);
            }

        }
        public ToDoResult IsUserTaken(string username, string email) {

            string sqlQuery = "SELECT Count(*) as ExistingUser FROM user_acc WHERE UserName = @Username OR  Email = @Email;";
            try {
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection)) {

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if(count == 0) {
                        return new ToDoResult(true, "", count > 0);
                    }
                    else return new ToDoResult(false, "UserName oder Email bereits vergeben");
  
                }
            }
            catch (Exception ex) {
                return new ToDoResult(false, "Database failed"); 
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
                return new ToDoResult(false, "Database failed");
            }
        }


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
                return new ToDoResult(false, "Database failed");
            }
            

        }
        public ToDoResult GetID(string email) {
            try {
                string query = "SELECT ID From user_acc Where email = @email";

                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {

                    cmd.Parameters.AddWithValue("@email", email);
                    var reader = cmd.ExecuteReader();                  
                    string id = "";
                    if (reader.Read()) { 
                        id = reader.GetString("ID");
                    }
                    reader.Close();
                    if (id == "") {
                        return new ToDoResult(false, "Database failed");
                    }
                    return new ToDoResult(true, "", id);
                }
            }
            catch {

            }

            return new ToDoResult(false, "");
        }
        
        public ToDoResult GetUsername(int id) {
            try {
                string query = "SELECT UserName From user_acc Where ID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, connection)) {

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    string username = "";
                    if (reader.Read()) {
                        username = reader.GetString("UserName");
                    }
                    reader.Close();
                    if (username == "") {
                        return new ToDoResult(false, "Database failed");
                    }
                    return new ToDoResult(true, "", username);
                }
            }
            catch {
                
            }
            return new ToDoResult(false, "");
        }
        public ToDoResult GetToDos(bool completedOnly = default) {
            try {
                string query = "SELECT Name, Description, Type, Startdate, Enddate, ID FROM todos";

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
                                reader.GetDateTime("Enddate"),
                                reader.GetInt32("ID")
                     )) ;
                    }
                    reader.Close();
                    var jsonresult = JsonConvert.SerializeObject(todo);
                    return new ToDoResult(true, "", jsonresult);
                
                }
            }
            catch {
                return new ToDoResult(false, "Database failed");
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
        public ToDoResult DeleteToDo(int id) { 
            try {
                string query = "DELETE FROM todos WHERE ID = @ID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    
                    cmd.ExecuteNonQuery();
                    return new ToDoResult(true, query);
                }
            }
            catch {
                return new ToDoResult(false, "Database failed"); 
            }
        }
        public ToDoResult CheckEmail(string email) {
            try {
                string query = "SELECT * FROM user_acc WHERE Email = @Email";

                using(MySqlCommand cmd = new MySqlCommand(query, connection)) {

                    cmd.Parameters.AddWithValue("@Email", email);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0) {
                        return new ToDoResult(false, "User nicht gefunden");
                    }
                    else {
                        return new ToDoResult(true, "", count > 0);
                    }
 

                }
            }
            catch {
                return new ToDoResult(false, "Database failed");
            }

        }
        public ToDoResult CheckLogin(string email, string password) {

            byte[] storedPassword = null;
            byte[] storedSalt = null; 
            
            try {

              
                string getstored = "SELECT saltBase64, Password From user_acc Where Email = @Email";

                using (MySqlCommand command = new MySqlCommand(getstored, connection)) {

                    command.Parameters.AddWithValue("@Email", email);

                    using (MySqlDataReader reader = command.ExecuteReader()) {

                        if (reader.Read()) {
                            string PasswordHash = reader.GetString("Password");
                            string SaltHash = reader.GetString("saltBase64");

                            storedPassword = Convert.FromBase64String(PasswordHash);
                            storedSalt = Convert.FromBase64String(SaltHash);

                            if (Auth.PasswordHash.VerifyPassword(password, storedPassword, storedSalt)) {

                                return new ToDoResult(true, "Passwort übereinstimmung");

                            } else return new ToDoResult(false, "Falsches Passwort");

                        } else return new ToDoResult(false, "Database error");
                    }
                }

            }
            catch {
                return new ToDoResult(false, "Database failed");
            }
        }

    }
    
  
}

﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    internal class Database
    {
        public bool is_online_database = true;

        public void Initialize_Database()
        {
            Database_Connection();
            Create_Users_Table();
        }

        private string Database_Connection()
        {
            string db_server = is_online_database ? "prototype.personalizedonlinefashion.shop" : "localhost";
            string db_user_id = "db_system_user";
            string db_password = "h$jNg}[%pnNp";
            string db_name = "db_system";

            return $"server={db_server};user id={db_user_id};password={db_password};database=db_system;database={db_name}";
        }

        private void Create_Users_Table()
        {
            string database_connection = Database_Connection();

            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                conn.Open();
                MySqlCommand cmd_1 = new MySqlCommand("SHOW TABLES LIKE 'users'", conn);

                object result_1 = cmd_1.ExecuteScalar();

                if (result_1 == null)
                {
                    string sql_2 = "CREATE TABLE `users` (`id` INT AUTO_INCREMENT PRIMARY KEY, `uuid` CHAR(36) NOT NULL, `name` VARCHAR(100) NOT NULL, `username` VARCHAR(100) NOT NULL UNIQUE, `password` VARCHAR(255) NOT NULL, `image` VARCHAR(100) NOT NULL, `user_type` VARCHAR(10) NOT NULL, `created_at` VARCHAR(20) NOT NULL, `updated_at` VARCHAR(20) NOT NULL)";
                    MySqlCommand cmd_2 = new MySqlCommand(sql_2, conn);

                    cmd_2.ExecuteNonQuery();

                    Insert_Admin_Data();
                }
            }
        }

        private void Insert_Admin_Data()
        {
            DateTime currentDateTime = DateTime.Now;

            Dictionary<string, object> user_data = new Dictionary<string, object>
            {
                { "uuid", Guid.NewGuid().ToString() },
                { "name", "Administrator" },
                { "username", "admin" },
                { "password", BCrypt.Net.BCrypt.HashPassword("admin123") },
                { "image", "default-user-image.png" },
                { "user_type", "admin" },
                { "created_at", currentDateTime },
                { "updated_at", currentDateTime }
            };

            Insert("users", user_data);
        }

        public string Generate_UUID()
        {
            return Guid.NewGuid().ToString();
        }

        public bool Insert(string table_name, Dictionary<string, object> data)
        {
            try
            {
                string database_connection = Database_Connection();
                using (MySqlConnection conn = new MySqlConnection(database_connection))
                {
                    conn.Open();

                    string columns = string.Join(", ", data.Keys);
                    string parameters = string.Join(", ", data.Keys.Select(key => "@" + key));

                    string query = $"INSERT INTO {table_name} ({columns}) VALUES ({parameters})";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var kvp in data)
                        {
                            cmd.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

                return false;
            }
        }

        public bool Update(string table_name, Dictionary<string, object> data, Dictionary<string, object> conditions, string logical_operator = "AND")
        {
            try
            {
                string database_connection = Database_Connection();
                using (MySqlConnection conn = new MySqlConnection(database_connection))
                {
                    conn.Open();

                    var setClause = string.Join(", ", data.Keys.Select(key => $"{key} = @{key}"));

                    var conditionClause = string.Join($" {logical_operator} ", conditions.Keys.Select(key => $"{key} = @{key}_cond"));

                    string query = $"UPDATE {table_name} SET {setClause} WHERE {conditionClause}";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (var kvp in data)
                        {
                            cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
                        }

                        foreach (var kvp in conditions)
                        {
                            cmd.Parameters.AddWithValue($"@{kvp.Key}_cond", kvp.Value ?? DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        public void Delete(string table_name, string column = null, object value = null)
        {
            string database_connection = Database_Connection();

            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                try
                {
                    conn.Open();

                    string query;

                    if (column != null && value != null)
                    {
                        query = $"DELETE FROM {table_name} WHERE {column} = @value";
                    }

                    else
                    {
                        query = $"DELETE FROM {table_name}";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (column != null && value != null)
                        {
                            cmd.Parameters.AddWithValue("@value", value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}");
                }
            }
        }

        public Dictionary<string, object> Select_One(string table_name, Dictionary<string, object> conditions, string logical_operator = "AND")
        {
            string database_connection = Database_Connection();

            Dictionary<string, object> result = new Dictionary<string, object>();
            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                conn.Open();

                List<string> conditionClauses = new List<string>();
                foreach (var condition in conditions)
                {
                    conditionClauses.Add($"{condition.Key} = @{condition.Key}");
                }
                string whereClause = string.Join($" {logical_operator} ", conditionClauses);

                string query = $"SELECT * FROM {table_name} WHERE {whereClause} LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    foreach (var condition in conditions)
                    {
                        cmd.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result.Add(reader.GetName(i), reader.GetValue(i));
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<Dictionary<string, object>> Select_Many(string table_name, string column_name, object value, string order_by_column, string order_by_value, string operation = "=", string optional_column_name = null, object optional_value = null)
        {
            string database_connection = Database_Connection();
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                conn.Open();

                string query = $"SELECT * FROM {table_name} WHERE {column_name} {operation} @value";

                if (optional_value != null && optional_column_name != null)
                {
                    query += $" AND {optional_column_name} = @optional_value";
                }

                query += $" ORDER BY `{order_by_column}` {order_by_value}";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@value", value);

                    if (optional_value != null && optional_column_name != null)
                    {
                        cmd.Parameters.AddWithValue("@optional_value", optional_value);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }

        public List<Dictionary<string, object>> Select_All(string table_name, string order_by_column, string order_by_value)
        {
            string database_connection = Database_Connection();
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                conn.Open();

                string query = $"SELECT * FROM `{table_name}` ORDER BY `{order_by_column}` {order_by_value}";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }

        public List<Dictionary<string, object>> Query(string sql, Dictionary<string, object> parameters = null)
        {
            string database_connection = Database_Connection();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            using (MySqlConnection conn = new MySqlConnection(database_connection))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue($"@{param.Key}", param.Value);
                        }
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MySQLData
{
    public class DBConnection
    {
        public string Server = "f0881346.xsph.ru";
        public string DatabaseName = "f0881346_db";
        public string UserName = "f0881346_db";
        public string Password = "HSodoHVP";
        public MySqlConnection Connection { get; set; }
        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return new DBConnection();
        }
        public MySqlConnection openConnect()
        {
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
            Connection = new MySqlConnection(connstring);
            Connection.Open();
            return Connection;
        }
        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }
            return true;
        }
        public void Close()
        {
            if (Connection != null) {Connection.Close(); }
            
            Connection = null;
        }

        public List<T> Query<T>(string query)
        {
            using (var cmd = new MySqlCommand(query, Connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<T>();
                    while (reader.Read())
                    {
                        var item = Activator.CreateInstance<T>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var propertyInfo = item.GetType().GetProperty(reader.GetName(i));
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(item, reader.GetValue(i));
                            }
                        }
                        results.Add(item);
                    }
                    return results;
                }
            }
        }

    }
}
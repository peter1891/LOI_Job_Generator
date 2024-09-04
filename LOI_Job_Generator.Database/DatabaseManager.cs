using LOI_Job_Generator.Events;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace LOI_Job_Generator.Database
{
    public static class DatabaseManager
    {
        private static string connectionString = "Data Source=.\\Job_Generator_DB.db;Version=3;";

        public static void ExecuteQuery(string query = null)
        {
            try
            {
                using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                {
                    dbConnection.Open();

                    if (query != null)
                    {
                        using (IDbCommand dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.CommandText = query;
                            dbCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SQLiteException se)
            {
                throw new Exception(se.Message);
            }
        }

        public static List<T> SelectQuery<T>(string query = null)
        {
            List<T> dataList = new List<T>();

            try
            { 
                using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                {
                    dbConnection.Open();

                    if (query != null)
                    {
                        using (IDbCommand dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.CommandText = query;

                            using (IDataReader dbCommandReader = dbCommand.ExecuteReader())
                            {
                                while (dbCommandReader.Read())
                                {
                                    T data = Activator.CreateInstance<T>();

                                    PropertyInfo[] dataProperties = typeof(T).GetProperties();
                                    foreach (PropertyInfo property in dataProperties)
                                    {
                                        try
                                        {
                                            object dataDb = dbCommandReader[property.Name];
                                            property.SetValue(data, dataDb);
                                        }
                                        catch (Exception ex) { }
                                    }

                                    dataList.Add((T)data);
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException se)
            {
                throw new Exception(se.Message);
            }

            return dataList;
        }

    }
}

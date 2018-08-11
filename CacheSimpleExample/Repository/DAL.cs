using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using CacheSimpleExample.Interfaces;
using Polly;

namespace CacheSimpleExample.Repository
{
    public class DAL : IDAL
    {
        private static readonly Policy RetryPolicy = Policy.Handle<SqlException>().WaitAndRetry(2, i => TimeSpan.FromSeconds(Math.Pow(2, i)));
        private readonly string _databaseConnection;
        private int _counter;

        public DAL()
        {
            _databaseConnection = ConfigurationManager.ConnectionStrings["local"].ConnectionString;
        }

        public string GetDateTimeFromSql()
        {
            string response;
            try
            {
                response = RetryPolicy.Execute(GetDateFromSql);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in the DLA: {ex.Message}");
                throw;
            }

            return response;
        }

        private string GetDateFromSql()
        {
            _counter += 1;
            ExceptionMock();
            const string queryString = "SELECT GETDATE() as currentDate";

            using (var connection = new SqlConnection(_databaseConnection))
            {
                var cmd = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["currentDate"].ToString();
                    }
                }
            }

            return null;
        }

        private void ExceptionMock()
        {
            var whenToFail = new List<int> { 2 };

            if (!whenToFail.Contains(_counter)) return;
            Console.WriteLine("Faking DB exception throw for Polly retry");

            using (var connection = new SqlConnection(_databaseConnection))
            {
                var cmd = new SqlCommand("raiserror('Exception in sql side', 16, 1)", connection);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

using ClientsExercise.Data;
using ClientsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientsExercise.Services
{
    public class AdoDataProvider : IDataProvider
    {
        private readonly string _connectionString;
        public AdoDataProvider(PhonesCatalogueContext context)
        {
            _connectionString = context.Database.GetDbConnection().ConnectionString;
        }
        public Client CreateClient(Client model)
        {
            var dataTable = new DataTable("dataTable");
            var phoneNumbersTable = new DataTable("phoneNumbersTable");
            phoneNumbersTable.Columns.Add("Type", typeof(string));
            phoneNumbersTable.Columns.Add("Number", typeof(string));
            foreach (var number in model.PhoneNumbers)
            {
                var row = phoneNumbersTable.NewRow();
                row["Type"] = number.Type;
                row["Number"] = number.Number;
                phoneNumbersTable.Rows.Add(row);
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "CreateClient"
            };
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@LastName", model.LastName);
            command.Parameters.AddWithValue("@Address", model.Address);
            command.Parameters.AddWithValue("@Email", model.Email);
            command.Parameters.AddWithValue("@PhoneNumbers", phoneNumbersTable);

            var dataAdapter = new SqlDataAdapter
            {
                SelectCommand = command
            };

            dataAdapter.Fill(dataTable);
            return ConvertDataTableToObject(dataTable).FirstOrDefault();
        }

        public bool DeleteClient(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "DeleteClient"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IList<Client> GetList()
        {
            var dataTable = new DataTable("dataTable");
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetClients"
            };
            var dataAdapter = new SqlDataAdapter
            {
                SelectCommand = command
            };

            dataAdapter.Fill(dataTable);
            return ConvertDataTableToObject(dataTable);
        }

        public Client UpdateClient(Client model)
        {
            var dataTable = new DataTable("dataTable");
            var phoneNumbersTable = new DataTable("phoneNumbersTable");
            phoneNumbersTable.Columns.Add("Type", typeof(string));
            phoneNumbersTable.Columns.Add("Number", typeof(string));
            foreach (var number in model.PhoneNumbers)
            {
                var row = phoneNumbersTable.NewRow();
                row["Type"] = number.Type;
                row["Number"] = number.Number;
                phoneNumbersTable.Rows.Add(row);
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = $"select top 1 1 from Clients where id = {model.Id}"
            };
            connection.Open();
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new Exception("client not found");
            }

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UpdateClient";
            command.Parameters.AddWithValue("@Id", model.Id);
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@LastName", model.LastName);
            command.Parameters.AddWithValue("@Address", model.Address);
            command.Parameters.AddWithValue("@Email", model.Email);
            command.Parameters.AddWithValue("@PhoneNumbers", phoneNumbersTable);

            var dataAdapter = new SqlDataAdapter
            {
                SelectCommand = command
            };

            dataAdapter.Fill(dataTable);
            connection.Close();
            return ConvertDataTableToObject(dataTable).FirstOrDefault();
        }

        private List<Client> ConvertDataTableToObject(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(x => new
            {
                Id = x.Field<int>("Id"),
                FirstName = x.Field<string>("FirstName"),
                LastName = x.Field<string>("LastName"),
                Address = x.Field<string>("Address"),
                Email = x.Field<string>("Email"),
                PhoneTypeName = x.Field<string>("PhoneTypeName"),
                PhoneNumber = x.Field<string>("PhoneNumber")
            }).GroupBy(x => new { x.Id, x.FirstName, x.LastName, x.Address, x.Email })
             .Select(x => new Client
             {
                 Id = x.Key.Id,
                 FirstName = x.Key.FirstName,
                 LastName = x.Key.LastName,
                 Address = x.Key.Address,
                 Email = x.Key.Email,
                 PhoneNumbers = x.Select(p => new PhoneNumber
                 {
                     Type = p.PhoneTypeName ?? "",
                     Number = p.PhoneNumber
                 }).ToList()
             }).ToList();
        }
    }
}

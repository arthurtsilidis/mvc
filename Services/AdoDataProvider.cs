using ClientsExercise.Data;
using ClientsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ClientsExercise.Services
{


    public class AdoDataProvider : IDataProvider
    {
        private readonly PhonesCatalogueContext _context = null;
        public AdoDataProvider()
        {
            _context = new PhonesCatalogueContext();
        }
        public Client CreateClient(Client model)
        {
            var dataTable = new DataTable("dataTable");
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            foreach (var number in model.PhoneNumbers)
            {
                var row = dataTable.NewRow();
                row["Type"] = number.Type;
                row["Number"] = number.Number;
                dataTable.Rows.Add(row);
            }
            using var connection = new SqlConnection(@"Server =.\; Database = PhonesCatalogue; Trusted_Connection = True;");
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
            command.Parameters.AddWithValue("@PhoneNumbers", dataTable);

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
                using var connection = new SqlConnection(@"Server =.\; Database = PhonesCatalogue; Trusted_Connection = True;");
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
            using var connection = new SqlConnection(@"Server =.\; Database = PhonesCatalogue; Trusted_Connection = True;");
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
            throw new NotImplementedException();
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

using gRPCBasico.Data.Connection;
using gRPCBasico.Protos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCBasico.Data.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IConfiguration _configuration;

        public PersonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PersonResponse GetPersonById(int businessEntityID)
        {
            try
            {
                PersonResponse response = new PersonResponse();

                using (var cnn = new SqlServerConnection(_configuration).GetConnection())
                {
                    string sql = "Select BusinessEntityID, FirstName, LastName from Users where BusinessEntityID=@BusinessEntityID";

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 200;
                        cmd.Connection = cnn;

                        cmd.Parameters.Add(new SqlParameter("@BusinessEntityID", businessEntityID));

                        cnn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.BusinessEntityID = Convert.ToInt32(reader["BusinessEntityID"]);
                                response.FirstName = reader["FirstName"].ToString();
                                response.LastName = reader["LastName"].ToString();
                            }
                        }
                        cnn.Close();
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonResponse> GetPersonByLastname(string lastName)
        {
            try
            {
                List<PersonResponse> responses = new List<PersonResponse>();

                using (var cnn = new SqlServerConnection(_configuration).GetConnection())
                {
                    string sql = "Select BusinessEntityID, FirstName, LastName from Users where LastName like '%@LastName%'";

                    using (var cmd = new SqlCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 200;
                        cmd.Connection = cnn;

                        cmd.Parameters.Add(new SqlParameter("@LastName", lastName));

                        cnn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                responses.Add(new PersonResponse()
                                {
                                    BusinessEntityID = Convert.ToInt32(reader["BusinessEntityID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString()
                                });
                            }
                        }
                        cnn.Close();
                    }
                }
                return responses;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

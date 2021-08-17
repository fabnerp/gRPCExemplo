using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace gRPCServiceBasic.Data.Connection
{
    public class SqlServerConnection
    {
        private readonly IConfiguration _configuration;

        public SqlServerConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Default"));        
        }
    }
}

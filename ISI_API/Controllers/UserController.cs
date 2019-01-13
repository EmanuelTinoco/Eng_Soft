using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ISI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ISI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase 
    {

        private IConfiguration configuration;
        private SqlConnection con;
        private SqlCommand com;

        public UserController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        [HttpPost]
        public void Post([FromBody] User r)
        {
            string constr = configuration.GetValue<string>("MySettings:DbConnection");
            con = new SqlConnection(constr);

            com = new SqlCommand("AddUser", con);
            com.CommandType = CommandType.StoredProcedure;

            //com.Parameters.AddWithValue("@id", r.Id);
            com.Parameters.AddWithValue("@id", r.Id);
            com.Parameters.AddWithValue("@username", r.Username);
            com.Parameters.AddWithValue("@email", r.Email);

            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        
    }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ISI_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ISI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistoController : ControllerBase
    {

        private IConfiguration configuration;
        private SqlConnection con;
        private SqlCommand com;

        public RegistoController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        
        // GET api/registo/5
        [HttpGet("{id}")]
        public ActionResult<Registo> Get(int id)
        {
            SqlDataReader reader = null;

            string constr= configuration.GetValue<string>("MySettings:DbConnection");
            con = new SqlConnection(constr);

            com = new SqlCommand("ShowRegistos", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@id", id);

            con.Open();
            reader = com.ExecuteReader();

            List<Registo> Lista = new List<Registo>();
            Registo emp = null;
            while (reader.Read())
            {
                emp = new Registo();
                emp.Id = Convert.ToInt32(reader.GetValue(0));
                emp.Data_Hora_Login = DateTime.Parse(reader.GetValue(1).ToString());
                emp.Data_Hora_Logoff = DateTime.Parse(reader.GetValue(2).ToString());
                Lista.Add(emp);
            }
            con.Close();
            return emp;
        }

        //POST
        [HttpPost]
        public void Post([FromBody] Registo r)
        {
            string constr = configuration.GetValue<string>("MySettings:DbConnection");
            con = new SqlConnection(constr);

            com = new SqlCommand("AddRegisto", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@id", r.Id);
            com.Parameters.AddWithValue("@userid", r.Userid);
            com.Parameters.AddWithValue("@Data_Hora_Login", r.Data_Hora_Login);
            com.Parameters.AddWithValue("@Data_Hora_Logoff", r.Data_Hora_Logoff);

            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        // PUT api/registo/15
        [HttpPut("{id}")]
        public void Put(int id, DateTime date)
        {
            string constr = configuration.GetValue<string>("MySettings:DbConnection");
            con = new SqlConnection(constr);

            com = new SqlCommand("UpdateRegisto", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@Data_Hora_Logoff", date);

            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string constr = configuration.GetValue<string>("MySettings:DbConnection");
            con = new SqlConnection(constr);

            com = new SqlCommand("DeleteRegisto", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@id", id);
            
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

    }
}
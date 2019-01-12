using API_ISI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API_ISI.Controllers
{
    public class RegistoController : ApiController
    {
        // GET: Registo


        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private SqlConnection con;
        private SqlCommand com;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["registoatividade"].ToString();
            con = new SqlConnection(constr);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("Registo")]
        public Registo Get(int id)
        {
            //return listEmp.First(e => e.ID == id);  
            SqlDataReader reader = null;

            string constr = ConfigurationManager.ConnectionStrings["registoatividade"].ToString();
            con = new SqlConnection(constr);

            com = new SqlCommand("ShowRegistos", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@id", id);

            //SqlCommand sqlCmd = new SqlCommand();
            //sqlCmd.CommandType = CommandType.Text;
            //sqlCmd.CommandText = "Select * from registo where Userid=" + id;
            //sqlCmd.Connection = con;
            con.Open();
            reader = com.ExecuteReader();

            Registo emp = null;
            while (reader.Read())
            {
                emp = new Registo();
                emp.Id = Convert.ToInt32(reader.GetValue(0));
                emp.Data_Hora_Login = DateTime.Parse(reader.GetValue(1).ToString());
                emp.Data_Hora_Logoff = DateTime.Parse(reader.GetValue(2).ToString());
            }
            con.Close();
            return emp;           
        }

    }
}
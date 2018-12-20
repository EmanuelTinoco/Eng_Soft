using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentidadeManager
{
    public class IdentidadeUser
    {
        static List<Identidade> ListaUser=new List<Identidade>();

        public static bool AddIdentidadeUser(string nome, string id)
        {
            Identidade user = new Identidade(nome, id);
            user.Nome = nome;
            user.Id = id;

            ListaUser.Add(user);
            return true;
        }

        public static string GetNome()
        {
            return ListaUser[0].Nome;
        }

        public static string GetId()
        {
            return ListaUser[0].Id;
        }

    }
}

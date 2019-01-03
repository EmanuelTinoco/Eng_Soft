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

        public static bool AddIdentidadeUser(string nome, string id, bool [] perm)
        {
            Identidade user = new Identidade(nome, id, perm);
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

        public static bool isAdmin()
        {
            return ListaUser[0].Admin;
        }

        public static void Change_ID_Registo(int id_registo)
        {
            ListaUser[0].ID_Registo = id_registo;
        }

        public static int ID_Registo()
        {
            return ListaUser[0].ID_Registo;
        }

        public static bool isResidente()
        {
            return ListaUser[0].Residente;
        }

        public static bool isMembro()
        {
            return ListaUser[0].Membro;
        }
    }
}

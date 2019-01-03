using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentidadeManager
{

    public class Identidade
    {
        private string nome;
        private string id;
        private bool admin;
        private bool residente;
        private bool membro;
        private int id_registo = -1;

        public Identidade(string nome, string id, bool[]perm)
        {
            this.nome = nome;
            this.id = id;
            admin = perm[0];
            residente = perm[1];
            membro = perm[2];
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }

        public bool Residente
        {
            get { return residente; }
            set { residente = value; }
        }

        public bool Membro
        {
            get { return membro; }
            set { membro = value; }
        }

        public int ID_Registo
        {
            get { return id_registo; }
            set { id_registo = value; }
        }
    }

}

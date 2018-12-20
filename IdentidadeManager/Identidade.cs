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

        public Identidade(string nome, string id)
        {
            this.nome = nome;
            this.id = id;
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
    }

}

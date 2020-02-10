using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class LocacaoFilme
    {
        public int Id { get; set; }
        public int LocacaoID { get; set; }
        public Locacao Locacao { get; set; }
        public int FilmeID { get; set; }
        public Filme Filme { get; set; }
    }
}

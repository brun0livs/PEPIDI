using System.Collections.Generic;

namespace PEPIDI.Models
{
    public class LinhaPedidoInfo
    {
        public string CodigoEpi { get; set; }
        public string Cor { get; set; }
        public string Modelo { get; set; }
        public string TamanhoAtual { get; set; }
        public string Familia { get; set; }
        public List<string> TamanhosDisponiveis { get; set; } = new List<string>();
        public List<string> ModelosDisponiveis { get; set; } = new List<string>();
    }
}

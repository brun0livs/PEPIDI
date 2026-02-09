using System.Collections.Generic;

namespace PEPIDI_0._5.Models
{
    public class LinhaPedidoInfo
    {
        public int IdEpi { get; set; }
        public string Modelo { get; set; }
        public string TamanhoAtual { get; set; }
        public List<string> TamanhosDisponiveis { get; set; } = new List<string>();
    }
}

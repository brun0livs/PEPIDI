using System;

namespace PEPIDI.Utils // Ajusta se o teu namespace for diferente
{
    // A classe tem de ser "static" para guardar os valores enquanto o programa estiver aberto
    public static class Sessao
    {
        public static int IdFuncionarioAtual { get; set; }
        public static int NivelAcessoAtual { get; set; }

        // Bónus: É sempre fixe guardar o nome para depois meteres no "Bem-vindo, Bruno!" do Dashboard
        public static string NomeFuncionarioAtual { get; set; }
    }
}
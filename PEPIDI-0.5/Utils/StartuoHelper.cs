using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PEPIDI // Se estivesse dentro de uma pasta, muda para PEPIDI.Utils
{
    public static class StartupHelper
    {
        // O caminho do Registo do Windows onde ficam os programas de arranque
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        // O nome que vai aparecer no Gestor de Tarefas do Windows (no separador Arranque)
        private const string AppName = "AgentePEPIDI";

        public static void RegistarAgenteNoArranque()
        {
            try
            {
                // O caminho onde o executável vai estar na versão final
                string exePath = Path.Combine(Application.StartupPath, "AgentePEPIDI.exe");

                // Abre o registo do Windows com permissão de escrita (true)
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        // Adiciona as aspas ao redor do caminho para evitar problemas com espaços nas pastas
                        key.SetValue(AppName, "\"" + exePath + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostra o erro mas não rebenta o programa
                MessageBox.Show("Aviso: Não foi possível ativar o arranque automático com o Windows.\n" + ex.Message,
                                "Registo do Windows", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void RemoverAgenteDoArranque()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        // Remove a chave, mas primeiro verifica se ela existe para não dar erro
                        if (key.GetValue(AppName) != null)
                        {
                            key.DeleteValue(AppName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aviso: Não foi possível desativar o arranque automático.\n" + ex.Message,
                                "Registo do Windows", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
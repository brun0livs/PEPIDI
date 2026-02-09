using PEPIDI.Organizers;
using System.Data;
using System.Data.SqlClient;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormFuncao : Form
    {
        private readonly int id;
        private readonly string Nome;
        private readonly int idGestor;
        private readonly string Hex;

        public FormFuncao(string _Nome, int _id, int _idgestor, string _Hex)
        {
            id = _id;
            Nome = _Nome;
            idGestor = _idgestor;
            Hex = _Hex;
            InitializeComponent();
            txtNome.Text = Nome.ToString();
            txtCorHex.Text = Hex.ToString();

        }

        private void LblFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EscolherCor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    var Cor = colorDialog.Color;
                    string hex = $"#{Cor.R:X2}{Cor.G:X2}{Cor.B:X2}";
                    txtCorHex.Text = hex;
                }
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InserirFuncao", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (id != 0) cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 100).Value = txtNome.Text;
                cmd.Parameters.Add("@CriadoPor", SqlDbType.Int).Value = idGestor;
                cmd.Parameters.Add("@CorHex", SqlDbType.NVarChar, 7).Value = txtCorHex.Text;

                conn.Open();
                object result = cmd.ExecuteScalar();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FormFuncao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void FormFuncao_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }
    }
}

using System;
using System.Windows.Forms;
using Guna.UI2.WinForms; // Certifica-te que a DLL do Guna está referenciada

namespace PEPIDI.Models
{
    // Removi o ": Component" daqui
    public partial class ComboTeste
    {
        public ComboTeste()
        {
            //InitializeComponent(); 
        }
    }

    // 1. A COLUNA
    // 1. A COLUNA
    public class GunaDataGridViewComboBoxColumn : DataGridViewComboBoxColumn
    {
        public GunaDataGridViewComboBoxColumn() : base() // O base agora está vazio
        {
            // Definimos o molde da célula aqui dentro
            this.CellTemplate = new GunaDataGridViewComboBoxCell();
        }

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                // Verificação de segurança para garantir que apenas aceita a tua célula Guna
                if (value != null && !value.GetType().IsAssignableFrom(typeof(GunaDataGridViewComboBoxCell)))
                {
                    throw new InvalidCastException("Deve ser uma GunaDataGridViewComboBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    // 2. A CÉLULA
    public class GunaDataGridViewComboBoxCell : DataGridViewComboBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            // Aqui é onde a "mágica" acontece: acedemos ao controlo de edição
            Guna2ComboBox combo = DataGridView.EditingControl as Guna2ComboBox;

            if (combo != null)
            {
                // Podes personalizar o estilo da GunaCombo aqui
                combo.FillColor = System.Drawing.Color.White;
                combo.BorderRadius = 5;
            }
        }

        public override Type EditType => typeof(GunaDataGridViewComboBoxEditingControl);
    }

    // 3. O CONTROLO DE EDIÇÃO (A GunaComboBox real)
    public class GunaDataGridViewComboBoxEditingControl : Guna2ComboBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        // AVISO 1 RESOLVIDO
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public object EditingControlFormattedValue
        {
            get => this.SelectedItem?.ToString();
            set { if (value is string s) this.SelectedItem = s; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) { }

        // AVISO 2 RESOLVIDO
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int EditingControlRowIndex { get => rowIndex; set => rowIndex = value; }

        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey) => true;
        public void PrepareEditingControlForEdit(bool selectAll) { }
        public bool RepositionEditingControlOnValueChange => false;

        // AVISO 3 RESOLVIDO
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public DataGridView EditingControlDataGridView { get => dataGridView; set => dataGridView = value; }

        // AVISO 4 RESOLVIDO
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool EditingControlValueChanged { get => valueChanged; set => valueChanged = value; }

        public Cursor EditingPanelCursor => base.Cursor;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            valueChanged = true;
            if (this.EditingControlDataGridView != null)
            {
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
            base.OnSelectedIndexChanged(e);
        }
    }
}
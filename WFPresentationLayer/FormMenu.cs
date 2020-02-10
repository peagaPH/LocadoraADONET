using BusinessLogicalLayer.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFPresentationLayer
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        Timer timer = new Timer();

        private void gênerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGenero frm = new FormGenero();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void filmesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilme frm = new FormFilme();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void funcionáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFuncionario frm = new FormFuncionario();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }



        private void locaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLocacao frm = new FormLocacao();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCliente frm = new FormCliente();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }
    }
}

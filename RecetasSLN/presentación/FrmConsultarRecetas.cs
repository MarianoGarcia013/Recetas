using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecetasSLN.presentación
{
    public partial class FrmConsultarRecetas : Form
    {
        public FrmConsultarRecetas()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmNuevaReceta FrmNueva = new FrmNuevaReceta();
            FrmNueva.ShowDialog();
        }

        private void FrmConsultarRecetas_Load(object sender, EventArgs e)
        {

        }
    }
}

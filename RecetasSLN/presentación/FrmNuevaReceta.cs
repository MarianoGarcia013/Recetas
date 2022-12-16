using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecetasSLN.datos;
using RecetasSLN.dominio;

namespace RecetasSLN.presentación
{
    public partial class FrmNuevaReceta : Form
    {
        private static helperDb helperDb;

        private Receta nuevaReceta;
        public FrmNuevaReceta()
        {
            InitializeComponent();
            nuevaReceta = new Receta();
        }

        private void lblTotalIngre_Click(object sender, EventArgs e)
        {

        }

        private void FrmNuevaReceta_Load(object sender, EventArgs e)
        {
            cargarCombo();
        }


        public void cargarCombo()
        {
            cboIngredientes.DataSource = helperDb.obtenerInstancia().listarProductos();
            cboIngredientes.DisplayMember = "n_ingrediente";
            cboIngredientes.ValueMember = "id_ingrediente";
            cboIngredientes.SelectedIndex = -1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboIngredientes.Text.Equals(String.Empty))
            {
                MessageBox.Show("Incompleto", "Debe seleccionar un ingrediente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["colIngrediente"].Value.ToString().Equals(cboIngredientes.Text))
                {
                    MessageBox.Show("PRODUCTO: " + cboIngredientes.Text + " ya se encuentra como detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;

                }
            }

            DataRowView ingrediente = (DataRowView)cboIngredientes.SelectedItem;
            int id = Convert.ToInt32(ingrediente.Row.ItemArray[0]);
            string nombre = (string)(ingrediente.Row.ItemArray[1]);
            string unidadMedida = (string)(ingrediente.Row.ItemArray[2]);

            Ingrediente ingrediente1 = new Ingrediente(id, nombre, unidadMedida);

            decimal cantidad = (decimal)txtCant.Value;

            DetalleReceta detalle = new DetalleReceta(ingrediente1, cantidad);

            nuevaReceta.agregarDetalle(detalle);

            dataGridView1.Rows.Add(new object[] { id, nombre, unidadMedida, (string)cantidad.ToString() });

            refreshCantidadIng();
            txtCant.Value = 1;
            cboIngredientes.SelectedIndex = -1;


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                nuevaReceta.quitarDetalle(dataGridView1.CurrentRow.Index);
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                refreshCantidadIng();
            }
        }

        private void refreshCantidadIng()
        {
            int acum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                acum++;
            }
            lblTotalIngre.Text = $"Total de ingredientes: {acum}";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text == "")
            {
                MessageBox.Show("Complete el nombre de la receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(txtCheff.Text == "")
            {
                MessageBox.Show("Complete el nombre del Cheff", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(cboTipoReceta.Text == "")
            {
                MessageBox.Show("Complete el tipo de la receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos un ingrediente!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            nuevaReceta.nombre = txtNombre.Text;
            nuevaReceta.cheff = txtCheff.Text;
            nuevaReceta.tipoReceta = Convert.ToInt32(cboTipoReceta.SelectedValue);
            guardarReceta(nuevaReceta);


        }

        private void guardarReceta(Receta oReceta)
        {
            if (helperDb.confirmarReceta(oReceta))
            {
                MessageBox.Show("Receta registrada", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("ERROR. No se pudo registrar la receta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}

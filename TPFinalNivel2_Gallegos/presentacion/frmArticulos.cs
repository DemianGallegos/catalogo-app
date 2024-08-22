using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulo> listaArticulo;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Marca");

        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Codigo"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                ocultarColumnas();
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load(@"C:\catalogo-app\placeholder.png");
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //validación para que no se rompa si no hay artículos seleccionados
            if (dgvArticulos.CurrentRow != null)
            {
                frmAltaArticulo alta = new frmAltaArticulo();
                alta.ShowDialog();
                cargar();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //validación para que no se rompa si no hay artículos seleccionados
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //validación para que no se rompa si no hay artículos seleccionados
            if (dgvArticulos.CurrentRow != null)
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo seleccionado;

                try
                {
                    DialogResult respuesta = MessageBox.Show("¿Seguro quiere eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);

                        cargar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            //validación para que no se rompa si no hay artículos seleccionados
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmDetalle detalle = new frmDetalle(seleccionado);
                detalle.ShowDialog();
            }
            
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;


            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ocultarColumnas();

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //validación par que no se rompa al borrar filtro avanzado
            if(!(cboCampo.SelectedIndex == -1))
            {
                string opcion = cboCampo.SelectedItem.ToString();

                if (opcion == "Precio")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");
                }
                else
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Comienza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contine");
                }

            }
            
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                cboCampo.BackColor = Color.GreenYellow;
                return true;
            }
            else
            {
                cboCampo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            }

            if (cboCriterio.SelectedIndex < 0)
            {
                cboCriterio.BackColor = Color.GreenYellow;
                return true;
            }
            else
            {
                cboCriterio.BackColor = System.Drawing.SystemColors.ControlLightLight;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    txtFiltroAvanzado.BackColor = Color.GreenYellow;
                    return true;
                }
                else
                {
                    txtFiltroAvanzado.BackColor = System.Drawing.SystemColors.ControlLightLight;
                }
            
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo nros para filtrar por Precio...");
                    return true;
                }
            }

            else
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    txtFiltroAvanzado.BackColor = Color.GreenYellow;
                    return true;
                }
                else
                {
                    txtFiltroAvanzado.BackColor = System.Drawing.SystemColors.ControlLightLight;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnBorrarFiltroAvanzado_Click(object sender, EventArgs e)
        {
            cboCriterio.Items.Clear();
            cboCriterio.BackColor = System.Drawing.SystemColors.ControlLightLight;
            cboCampo.SelectedIndex = -1;
            cboCampo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            txtFiltroAvanzado.Text = "";
            txtFiltroAvanzado.BackColor = System.Drawing.SystemColors.ControlLightLight;

            txtFiltro.Text = "";

        }
    }
}

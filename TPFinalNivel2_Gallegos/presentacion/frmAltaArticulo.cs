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
using System.IO;
using System.Configuration;

namespace presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();

            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(btnAgregarImagen, "Levantar imagen local");
            toolTip1.SetToolTip(btnGuardarImagen, "Guardar imagen en c: catalogo-app");

        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";

            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(btnAgregarImagen, "Levantar imagen local");
            toolTip1.SetToolTip(btnGuardarImagen, "Guardar imagen en c: catalogo-app");

        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFrmAltaArticulo())
                    return;
                //Agregar articulo
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                //Modificar
                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }


                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

            }
        }

        private void btnGuardarImagen_Click(object sender, EventArgs e)

        {
            if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
            {
                try
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["catalogo-app"] + archivo.SafeFileName);
                    txtImagenUrl.Text = ConfigurationManager.AppSettings["catalogo-app"] + archivo.SafeFileName;

                    MessageBox.Show("Imagen guardada exitosamente en disco C: catalogo-app...");
                }

                catch (IOException ex)
                {

                    txtImagenUrl.Text = ConfigurationManager.AppSettings["catalogo-app"] + archivo.SafeFileName;

                    MessageBox.Show("La Imagen ya existe en disco C: catalogo-app y será asociada al Artículo...");
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }


        private bool validarFrmAltaArticulo()
        {
            if (txtCodigo.Text == "")
            {
                txtCodigo.BackColor = Color.GreenYellow;
                return true;
            }
            else
            {
                txtCodigo.BackColor = System.Drawing.SystemColors.Control;
            }

            if (txtNombre.Text == "")
            {
                txtNombre.BackColor = Color.GreenYellow;
                return true;
            }
            else
            {
                txtNombre.BackColor = System.Drawing.SystemColors.Control;
            }

            if (txtPrecio.Text == "")
            {
                txtPrecio.BackColor = Color.GreenYellow;
                return true;
            }
            else
            {
                txtPrecio.BackColor = System.Drawing.SystemColors.Control;
            }

            if (!(soloNumeros(txtPrecio.Text)))
            {
                MessageBox.Show("Ingresar sólo números");
                return true;
            }

            return false;

        }
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter) || char.IsPunctuation(caracter)))
                    return false;
            }
            return true;
        }

        
    }    
}

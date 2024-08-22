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
    public partial class frmDetalle : Form
    {
        private Articulo articulo;
        public frmDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDetalle_Load(object sender, EventArgs e)
        {
            lblCategoria.Text = articulo.Categoria.Descripcion;
            lblMarca.Text = articulo.Marca.Descripcion;
            lblCodigo.Text = articulo.Codigo;
            lblNombre.Text = articulo.Nombre;
            lblDescripcion.Text = articulo.Descripcion;
            cargarImagen(articulo.ImagenUrl);
            lblPrecio.Text = articulo.Precio.ToString("0.00");
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
    }

}

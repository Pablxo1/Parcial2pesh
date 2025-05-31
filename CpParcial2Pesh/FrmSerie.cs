using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CadParcial2Pesh;
using ClnParcial2Pesh;

namespace CpParcial2Pesh
{
    public partial class FrmSerie : Form
    {
        private bool modoEdicion = false;
        public FrmSerie()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var lista = SeriesCln.listar();
            dgvSeries.DataSource = lista;
            dgvSeries.Columns["id"].Visible = false;
            dgvSeries.Columns["estado"].Visible = false;
            dgvSeries.Columns["titulo"].HeaderText = "Titulo";
            dgvSeries.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvSeries.Columns["director"].HeaderText = "Director";
            dgvSeries.Columns["episodios"].HeaderText = "Episodios";
            dgvSeries.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";
            dgvSeries.Columns["urlPortada"].HeaderText = "Url Portada";
            dgvSeries.Columns["idiomaOriginal"].HeaderText = "Idioma Original";
			if (lista.Count > 0) dgvSeries.CurrentCell = dgvSeries.Rows[0].Cells["titulo"];
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

		private bool validar()
		{
			bool esValido = true;
			erpTitulo.SetError(txtTitulo, "");
			erpSinopsis.SetError(txtSinopsis, "");
			erpDirector.SetError(txtDirector, "");
			erpEpisodios.SetError(nudEpisodios, "");
			erpFechaEstreno.SetError(dtpFechaEstreno, "");
			erpUrlPortada.SetError(txtUrlPortada, "");

			if (string.IsNullOrEmpty(txtTitulo.Text))
			{
				erpTitulo.SetError(txtTitulo, "El campo Titulo es obligatorio");
				esValido = false;
			}
			if (string.IsNullOrEmpty(txtSinopsis.Text))
			{
				erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
				esValido = false;
			}
			if (string.IsNullOrEmpty(txtDirector.Text))
			{
				erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
				esValido = false;
			}

			if (nudEpisodios.Value <= 0)
			{
				erpEpisodios.SetError(nudEpisodios, "El campo Episodios no puede ser menor a 0 o ser 0");
				esValido = false;
			}
			DateTime fechaInventoTV = new DateTime(1926, 1, 26);

			if (dtpFechaEstreno.Value < fechaInventoTV)
			{
				erpFechaEstreno.SetError(dtpFechaEstreno, "La Fecha de Estreno no puede ser anterior al 26/01/1926 (invención de la televisión)");
				esValido = false;
			}
			if (string.IsNullOrEmpty(txtUrlPortada.Text))
			{
				erpUrlPortada.SetError(txtUrlPortada, "El campo Url Portada es obligatorio");
				esValido = false;
			}

			return esValido;
		}

        private void limpiar()
        {
            txtTitulo.Clear();
            txtSinopsis.Clear();
            txtDirector.Clear();
            txtUrlPortada.Clear();
            cbxIdiomaOriginal.SelectedIndex = -1;
			nudEpisodios.Value = 0;
            dtpFechaEstreno.Value = DateTime.Now;
            modoEdicion = false;
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("¿Está seguro de salir?",
                "::: Series - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = (int)nudEpisodios.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                serie.urlPortada = txtUrlPortada.Text.Trim();
                serie.idiomaOriginal = cbxIdiomaOriginal.Text.Trim();
				if (!modoEdicion)
                {
                    serie.estado = 1;
                    SeriesCln.insertar(serie);
                }
                else
                {
                    int index = dgvSeries.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvSeries.Rows[index].Cells["id"].Value);
                    SeriesCln.actualizar(serie);
                }
                limpiar();
                listar();
                MessageBox.Show("Serie guardada correctamente", "::: Series :::",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
 
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int index = dgvSeries.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvSeries.Rows[index].Cells["id"].Value);
            var serie = SeriesCln.obtenerUno(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            nudEpisodios.Value = serie.episodios;
            dtpFechaEstreno.Value = serie.fechaEstreno;
            txtUrlPortada.Text = serie.urlPortada;
            cbxIdiomaOriginal.Text = serie.idiomaOriginal;
			txtTitulo.Focus();
            modoEdicion = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvSeries.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvSeries.Rows[index].Cells["id"].Value);
            string titulo = dgvSeries.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar la Serie {titulo}?",
                "::: Series - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                SeriesCln.eliminar(id);
                listar();
                MessageBox.Show("La Serie se ha eliminado correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

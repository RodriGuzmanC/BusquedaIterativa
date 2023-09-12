using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ejercicio06
{
    public partial class Form1 : Form
    {
        DataSet datosTemporales = new DataSet();
        DataView contenido;
        public void metodoFiltrar(string consultaSql, ref DataSet resultadoTemporal, string tabla)
        {
            try
            {
                string miRuta = "Data Source=DESKTOP-2E54QI4;Initial Catalog=miBase16;Integrated Security=True";
                SqlConnection miConexion = new SqlConnection(miRuta);
                SqlCommand nuevaInstancia = new SqlCommand(consultaSql, miConexion);
                miConexion.Open();
                SqlDataAdapter datos = new SqlDataAdapter(nuevaInstancia);
                datos.Fill(resultadoTemporal, tabla);
                datos.Dispose(); //libera los recursos utilizados
                miConexion.Close();
            }
            catch(Exception mensajeSistema)
            {
                MessageBox.Show(mensajeSistema.Message);
            }
        }
        //Inicializador
        public Form1()
        {
            InitializeComponent();
        }

        //botones
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string datosEncontrados = "";
            string[] textoBusqueda = this.textBox1.Text.Split(' ');
            foreach (string palabra in textoBusqueda)
            {
                if (datosEncontrados.Length == 0)
                {
                    //Verificar si la palabra es un número
                    if (int.TryParse(palabra, out int codigoNumero))
                    {
                        //Si es un número, aplicar la condición de igualdad en lugar de LIKE
                        datosEncontrados = "(codigo = " + codigoNumero + " OR nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                    }
                    else
                    {
                        //Si no es un número, usar LIKE
                        datosEncontrados = "(nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                    }
                    //datosEncontrados = "(codigo LIKE '%" + palabra + "%' OR nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                }
                else
                {
                    if (int.TryParse(palabra, out int codigoNumero))
                    {
                        datosEncontrados += " AND (codigo = " + codigoNumero + " OR nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                    }
                    else
                    {
                        datosEncontrados += " AND (nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                    }
                    //datosEncontrados += " AND(codigo LIKE '%" + palabra + "%' OR nombre LIKE '%" + palabra + "%' OR apellido LIKE '%" + palabra + "%')";
                }
            }
            this.contenido.RowFilter = datosEncontrados;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.metodoFiltrar("SELECT * FROM Agenda", ref datosTemporales, "Agenda");
            this.contenido = ((DataTable)datosTemporales.Tables["Agenda"]).DefaultView;
            this.dataGridView1.DataSource = contenido;
        }
    }
}

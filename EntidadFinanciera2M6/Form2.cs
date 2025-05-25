using EntidadFinanciera2M6.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

    namespace EntidadFinanciera2M6
    {

    /// <summary>
    /// Formulario que muestra todas las transacciones registradas
    /// </summary>

    public partial class Form2 : Form
        {

        /// <summary>
        /// Constructor que inicializa componentes y carga transacciones
        /// </summary>
        ///     
        private EntidadFinancieraContext ef = new EntidadFinancieraContext();
            public Form2()
            {
                InitializeComponent();
                Cargar();
            }

        /// <summary>
        /// Carga y muestra las transacciones en el DataGridView
        /// </summary>
        private void Cargar()
            {
                dataGridView1.DataSource = ef.Transacciones.ToList();
            }

        }
    }

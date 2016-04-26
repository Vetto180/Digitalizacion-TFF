using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace InterfazArduino_V._0._0
{
    public partial class Form1 : Form
    {
        public bool accesso = true;
        public int NumeroDeMuestras = 1;
        int[] valores = new int[3000];
        SerialPort PuertoDeEntrada = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void ObtenerValores()
        {
            try
            {
                accesso = true; NumeroDeMuestras = 1;
                while (accesso != false)
                {
                    if (NumeroDeMuestras <= 3000)
                    {
                        string cadena = PuertoDeEntrada.ReadLine();
                        double valor = (Convert.ToDouble(cadena)) * (0.0083612);
                        if (valor > 5) {valor = 5;}
                        else if (valor < 0) { valor = 0;}
                        valor = Math.Round(valor, 3);
                        valores[NumeroDeMuestras-1] = Convert.ToInt32(valor * 1000);
                        NumeroDeMuestras++;
                    }
                    else
                    {
                        PuertoDeEntrada.Close();
                        accesso = false;
                        button1.Enabled = true;
                    }
                    if(NumeroDeMuestras % 30 == 0) progressBar1.Increment(3);
                }
                progressBar1.Visible = false;
            } catch (Exception)
            {
                MessageBox.Show("Error de valor en la comunicacion serial");
                button1.Enabled = true;
                progressBar1.Visible = false;
            }
        }

        private void GraficarValores()
        {
            Graphics papel = pictureBox1.CreateGraphics();
            Pen LapizOrilla = new Pen(Color.Red,2);//CadeBlue
            Pen LapizRelleno = new Pen(Color.SteelBlue);
            for (int x = 0; x <= 2997; x++)
            {
                papel.DrawLine(LapizRelleno, x / 3, pictureBox1.Height, x / 3, pictureBox1.Height - (valores[x] / 10) + 1);
                papel.DrawLine(LapizOrilla, x / 3, pictureBox1.Height - (valores[x] / 10), x / 3, pictureBox1.Height - (valores[x] / 10) + 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            button1.Enabled = false;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            PuertoDeEntrada.BaudRate = 250000;
            try
            {
                PuertoDeEntrada.PortName = "COM5";
                PuertoDeEntrada.Open();
                PuertoDeEntrada.WriteLine("S");
                ObtenerValores();
                GraficarValores();
            } catch (Exception)
            {
                MessageBox.Show("Error de puerto.");
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    /* Conseguir un componente que aplique la transaformada de fouirer para
    para una serie de valores, que seria el vector de las muestras y que la grafique */
}

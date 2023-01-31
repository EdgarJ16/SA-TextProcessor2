using StandarizacionStrings.Logica.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StandarizacionStrings
{
    public partial class LDAPVerForm : Form
    {
        public LDAPVerForm()
        {
           
            InitializeComponent();


            textBox1.Text = Procesador.GenerateLDAPlogons();
            textBox2.Text = Procesador.GenerateLDAPdisplayName();

        }

     

        private void button2_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(textBox1.Text); }
           
               catch (System.ArgumentNullException s)
            {
                MessageBox.Show("Error: no text coppied");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(textBox2.Text); }
           
               catch (System.ArgumentNullException s)
            {
                MessageBox.Show("Error: no text coppied");

            }
        }
    }
}

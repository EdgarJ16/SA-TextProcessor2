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
    public partial class EditUser : Form
    {

        public StandarizacionStrings.Logica.Clases.User editUser = new Logica.Clases.User();
        public EditUser(StandarizacionStrings.Logica.Clases.User user)
        {
            editUser = user;
            InitializeComponent();
            txtEditName.Text = editUser.FirstName + " " + editUser.LastName;
            txtEditDP.Text = editUser.DisplayName;
            txtEditID.Text = editUser.UserName;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            editUser.DisplayName = txtEditDP.Text;
            editUser.UserName = txtEditID.Text;
           
            StandarizacionStrings.Logica.Clases.Procesador.UpdateData(editUser);


            this.Close();

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

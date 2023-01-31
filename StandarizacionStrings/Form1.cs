using StandarizacionStrings.Logica.Clases;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace StandarizacionStrings
{
    public partial class Form1 : Form
    {
        public string rawdata;
        public Form1()
        {


           InitializeComponent();
            

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string data = getDataGridview(dataGridView1);




            // The below code show the processed data to the user 
            if (StandarizacionStrings.Logica.Clases.Procesador.ListaErrores.Count == 0)
            {

                textBox1.Text = data;
                Nombres.Text = Logica.Clases.Procesador.getLogons();
                textBox2.Text = Logica.Datos.ListaDatos.getDocumentacion();
            }
            else
            {
                string mensaje = StandarizacionStrings.Logica.Clases.Procesador.getErrores();

                MessageBox.Show("Format Error, Please check the following users:\n\n" + mensaje, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            VerifyBtn.Enabled = true;
            button1.Enabled = false;
        }

        private void txtBoxRawData_TextChanged(object sender, EventArgs e)
        {

        }


        // the Below button will generate the LDAP script to verify existing users 
        private void VerifyBtn_Click(object sender, EventArgs e)
        {
            
            rawdata = txtBoxRawData.Text;

            List<string> list = new List<string>();

            StandarizacionStrings.Logica.Datos.ListaDatos Datos = new Logica.Datos.ListaDatos(StandarizacionStrings.Logica.Clases.Procesador.procesarString(rawdata));
            // 
            if (StandarizacionStrings.Logica.Clases.Procesador.ListaErrores.Count == 0)
            {
                    dataGridView1.Rows.Clear();

                LDAPVerForm LDAPQuery = new LDAPVerForm();
                LDAPQuery.ShowDialog();

                //Below is the code that filles the data gridview 
                    
                    PopulateDataGridView();
                
                

                rawdata = dataGridView1.ToString();

                button1.Enabled = true;
                VerifyBtn.Enabled = false;

            }
            else
            {
                string mensaje = StandarizacionStrings.Logica.Clases.Procesador.getErrores();

                MessageBox.Show("Format Error, Please check the following users:\n\n" + mensaje, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }


        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);


            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "First Name";
            dataGridView1.Columns[1].Name = "Last Name";
            dataGridView1.Columns[2].Name = "Display Name";
            dataGridView1.Columns[3].Name = "User Name";
            dataGridView1.Columns[4].Name = "Password";
            dataGridView1.Columns[5].Name = "Company";
            dataGridView1.Columns[6].Name = "State";
            dataGridView1.Columns[7].Name = "Email";

            dataGridView1.Rows.Clear();

        }

        private string getDataGridview(DataGridView dataGrid)
        {
            Procesador.listaLogons.Clear();
            string data = " ";
            StringBuilder sb = new StringBuilder();
            if (dataGrid == null)
            {
            }
            else
            {
                int count = 0;
                int userCount = 0;
                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null)
                        {

                        }
                        else
                        {

                            string tempCell = cell.Value.ToString();



                            switch (count) {
                                case 2:
                                    userCount++;
                                    sb.Append(cell.Value.ToString() + "\t");
                                    Procesador.listaNames.Add(userCount + "- NAME: " + tempCell);
                                    break;

                                case 3:
                                    sb.Append(cell.Value.ToString() + "\t");
                                    Procesador.listaNames.Add(" USER ID: " + tempCell+"\n");
                                    Procesador.listaLogons.Add(tempCell);
                                    break;

                                case 8:
                                    sb.Append("\n");
                                    count = 0;
                                    sb.Append(cell.Value.ToString() + "\t");
                                    break;

                                default:
                                    sb.Append(cell.Value.ToString() + "\t");
                                    break;

                            }
                            count++;


                            
                        }

                    }



                }
            }





            data = sb.ToString();


            return data;
        }

        public void PopulateDataGridView()
        {


            
            dataGridView1.Rows.Clear();
            
            SetupDataGridView();
                
                int tempRow = 0;

                foreach (User u in Procesador.ProcessedUsers)
                {

                    dataGridView1.Rows.Add();
                   
                    dataGridView1.Rows[tempRow].Cells[0].Value = u.FirstName;
                    dataGridView1.Rows[tempRow].Cells[1].Value = u.LastName;
                    dataGridView1.Rows[tempRow].Cells[2].Value = u.DisplayName;
                    dataGridView1.Rows[tempRow].Cells[3].Value = u.UserName;
                    dataGridView1.Rows[tempRow].Cells[4].Value = u.Password;
                    dataGridView1.Rows[tempRow].Cells[5].Value = u.Company;
                    dataGridView1.Rows[tempRow].Cells[6].Value = u.State;
                    dataGridView1.Rows[tempRow].Cells[7].Value = u.Email;

                    tempRow++;

                }

                
         

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {

                Clipboard.SetText(textBox1.Text);

            }
            catch(System.ArgumentNullException s) {
                MessageBox.Show("Error: no text coppied");

            }
     
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(Nombres.Text); 
            }
            catch (System.ArgumentNullException s)
            {
                MessageBox.Show("Error: no text coppied");

            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {Clipboard.SetText(textBox2.Text); }
              catch (System.ArgumentNullException s)
            {
                MessageBox.Show("Error: no text coppied");

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {

                


                User user = new User();


                user.FirstName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                user.LastName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                user.DisplayName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                user.UserName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

               user.Password = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                user.Company = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                user.State = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                user.Email = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                using (EditUser editUser = new EditUser(user) { }) {

                    if (editUser.ShowDialog() == DialogResult.OK) {

                        PopulateDataGridView();


                    }
                
                }
                    
            }
        }
    }



}
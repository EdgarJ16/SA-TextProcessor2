
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;


namespace StandarizacionStrings.Logica.Clases
{
    public class Procesador
    {
        // Nuevo incremento 


        //1. mediante la lista de users procesados se generan los dos LDAP Queries
        //2. Se despliegan los datos en el data grid view para que puedan ser editados por el end user
        //3. Una vez verificados los datos del data grid view se reprocesan los datos que se optienen de el data grid veiw 


      

        // This class controls the logic behind the data processing

        //List below will acumualte the data to display to user 
        public static List<String> listaLogons = new List<string>();
        //Acumulate Logons
        public static List<String> listaNames = new List<string>();
        //Acumulate Names and logons for documentation
        public static List<String> ListaErrores = new List<string>();
        //Acumulate lines that present an error 

        public static List<String> listaLogonsLDAP = new List<string>();


        public static List<String> listaDisplayLDAP = new List<string>();

       public static List<String> listaProcesada = new List<string>();


        // this list will store verified users 
        public static List<User> ProcessedUsers = new List<User>();

        //ProcesarString will split the raw data in lines and then will tokenize each line to add the require info

        public static List<String> procesarString(string l)
        {

            ListaErrores.Clear();
            listaLogons.Clear();
            listaProcesada.Clear();
            ProcessedUsers.Clear();
            listaLogonsLDAP.Clear();
            listaDisplayLDAP.Clear();

            string[] lineasTemp = l.Split("\n");
            String regexemail = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            Regex rx = new Regex("(\\d{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx3 = new Regex(regexemail, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx2 = new Regex("((TASK|INC|RITM|AR)\\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match;

            bool flagTicket = false;

            if (rx2.IsMatch(l))
            {
                flagTicket = true;
            }

            StringBuilder sbTemp = new StringBuilder();
            StringBuilder testsb = new StringBuilder();
            string temp = "";
            string temp2 = "";

            int i = 1;

            if (l.Length > 0)
            {

                foreach (string str in lineasTemp) // recorre las lineas 
                {

                    sbTemp.Clear();
                    temp2 = str;


                    if (temp2.Length <= 1)
                    {


                    }
                    else {

                        string[] tokensTemp = temp2.Split("\t");// tokenize the line 

                        bool Valido = false;

                        //validation if the code has 7 tabs 
                        match = rx2.Match(str);
                        if (match.Success)  // if line has ticket number info tab number should be 8
                        {
                            // tabs must be 8
                            if (validaLinea(temp2.Trim()) == 8 && rx.IsMatch(tokensTemp[4]))
                            {

                                Valido = true;
                            }



                        }
                        else
                        {// If line does NOT have the ticket number tab count must be 7


                            if (validaLinea(temp2.Trim()) == 7 && rx.IsMatch(tokensTemp[4]))
                            {

                                Valido = true;
                            }

                        }
                        if (tokensTemp.Length != 1 && Valido)
                        {

                            if (tokensTemp.Length >= 7)
                            {

                                //listaLogons.Add(tokensTemp.ElementAt(3).Trim());
                                //listaNames.Add("NAME: " + tokensTemp.ElementAt(2).Trim() + " | LOGON NAME: " + tokensTemp.ElementAt(3).Trim());
                                listaLogonsLDAP.Add("(userPrincipalName=*" + tokensTemp.ElementAt(3).Trim() + "*)");
                                listaDisplayLDAP.Add("(displayName=*" + tokensTemp.ElementAt(2).Trim() + "*)");
                            }


                            foreach (string str2 in tokensTemp) // Will iterate through each token
                            {
                                temp = str2;
                                temp = temp.Trim();

                                if (temp == null)
                                {


                                }

                                if (rx2.IsMatch(str2))
                                {

                                    temp = "\r\n";
                                    break;

                                }
                                else if (rx3.IsMatch(temp) && !flagTicket)
                                {
                                    temp = "\r\n";
                                    break;

                                }
                                else if (rx.IsMatch(str2))
                                {

                                    temp = "SA" + temp + "cpl";

                                }

                                sbTemp.Append(temp + "\t");


                            }

                            listaProcesada.Add(sbTemp.ToString() + "\n");

                        }
                        else
                        {

                            ListaErrores.Add("----------------------------------------------------------------------\nUser: " + tokensTemp[0] + " | In line: " + i + "\n----------------------------------------------------------------------");

                        }


                        i++;//count the number of lines processed 
                    }



                }

            }

           
                generateUsersList(listaProcesada);

            
            
            return listaProcesada;
        }


        //This methos is a User creater from the processed data.
        public static List<User> generateUsersList(List<String> list) {
            string tempData = "";
            string[] tempUserData;
            int counter = 0;    
            //Will iterate for each user 
            foreach (string str in list) { 
            string strTemp = str;
            tempUserData = strTemp.Split("\t");

                User user = new User();

                user.FirstName = tempUserData[0];
                user.LastName = tempUserData[1];
                user.DisplayName = tempUserData[2];
                user.UserName = tempUserData[3];
                user.Password = tempUserData[4];
                user.Company = tempUserData[7];
                user.State = tempUserData[6];
                user.Email = tempUserData[5];


                //This will iterate in each piece of info from each user 
                ProcessedUsers.Add(user);
              


            
            }
    



        return ProcessedUsers;
         }



        // Getters for the Lists to display data on UI
        public static string getLogons()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String x in listaLogons)
            {
                sb.Append(x + "\n");

            }


            return sb.ToString();
        }
        public static string getLogonsLDAP()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String x in listaLogonsLDAP)
            {
                sb.Append(x);

            }


            return sb.ToString();
        }

        public static string getDisplayLDAP()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String x in listaDisplayLDAP)
            {
                sb.Append(x);

            }


            return sb.ToString();
        }
        public static string getErrores()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String x in ListaErrores)
            {
                sb.Append(x + "\n");

            }


            return sb.ToString();
        }
        public static string getDocInfo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String x in listaNames)
            {
                sb.Append(x + "\n");

            }
            listaNames.Clear();

            return sb.ToString();
        }

        public static string getListaProcesada()
        {
            StringBuilder sb = new StringBuilder();
     
            foreach (String x in listaProcesada) 
            {
                sb.Append(x);

            }


            return sb.ToString();
        }


        //This methos will count the number of tabs in each line
        public static int validaLinea(string l) {
            Regex rx = new Regex("(\\d{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxstate = new Regex("(([A-Z]){2})", RegexOptions.Compiled );


            StringBuilder sb = new StringBuilder();
          StringBuilder sbdef = new StringBuilder();

           

            bool flag = false;
            int contador = 0;
            string test = "";



            //Cuenta las tabulaciones
            foreach (char x in l.ToCharArray()) {
                if (x == '\t') { 
                
                contador++;
                }
                
            
            }

        
        return contador;
        }


        public static string GenerateLDAPlogons()
        {

            string temp = getLogonsLDAP();


            string ldapScript = "(&(|" + temp + "))";


       return ldapScript;
        }

        public static string GenerateLDAPdisplayName()
        {

            string temp = getDisplayLDAP();


            string ldapScript = "(&(|" + temp + "))";


            return ldapScript;
        }

        public static void UpdateData(User s)
        {

            int Counter = 0;
            User tempUser = new User();

                foreach (User x in ProcessedUsers)
            {
                

                if (x.Email == s.Email && s != null )
                {
                  
                    x.DisplayName = s.DisplayName;
                    x.UserName = s.UserName;

                    tempUser = x;
                    break;
                   
                }
                Counter++;

            }
            ProcessedUsers.Remove(ProcessedUsers.ElementAt(Counter));
            ProcessedUsers.Add(tempUser);

            


        }
        
        // Populates data gridview


        // Once data have been verified it creates objects with users unfo 
        // This can also be used to display this object on the data grid view





    }


}
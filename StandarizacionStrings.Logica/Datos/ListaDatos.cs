using StandarizacionStrings.Logica.Clases;

using System.Text;


namespace StandarizacionStrings.Logica.Datos
{
    public class ListaDatos
    {
        public static List<String> cuentasSA;

        public ListaDatos(List<String> SA) {
            
            if (SA == null)
            {
                
               
            }
            else {
                cuentasSA = SA;
            }
          
        
        }

        public static string getdatosSa()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string x in cuentasSA) { 
            sb.Append(x);
            
            }

            return sb.ToString();
        }

        public static string getDocumentacion() { 
        StringBuilder sb = new StringBuilder(); 

        string documentacion = "Completed TWP/SA Stewart Access Account Setup  for the below users:\n\n";
            documentacion += Procesador.getDocInfo();
            return documentacion;
        }


    }
}

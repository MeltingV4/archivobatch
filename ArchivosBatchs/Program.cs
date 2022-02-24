using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivosBatchs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        
        public static void Main()
        {
            var config = new JobHostConfiguration();
            string conexionstring = "server=localhost;database=ViralCel;integrated security=true";
            //string conexionstring = "Server=tcp:40.84.183.177,1433;Initial Catalog=ViralCel;User ID=viraladmin;Password=Viralcel2021$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
            SqlConnection conexion = new SqlConnection(conexionstring);
            StreamWriter sw = new StreamWriter("D:\\archivo_batch.txt");
            /*if (config.IsDevelopment)
            {*/

            //config.UseDevelopmentSettings();
            try
            {
                DateTime localDate = DateTime.Now;
                //Console.WriteLine("Hoy es " + localDate);
                //Console.WriteLine("Hace 5 dias fue " + localDate.AddDays(-5));
                //Console.WriteLine("-------------------------------------");
                string query = "SELECT Orders.OrderNumber,AspNetUsers.OpenPayCustomerId,TotalAmountPrice,'MXN' AS moneda,'Pago recurrente' AS descripcion,AspNetUsers.Name,AspNetUsers.FullName,AspNetUsers.Email,SIMPurchased.MSISDN FROM Orders LEFT JOIN OrderDetails ON OrderDetails.OrderId = Orders.OrderId LEFT JOIN AspNetUsers ON AspNetUsers.Id = Orders.UserId LEFT JOIN Product ON Product.ProductId = OrderDetails.ProductId LEFT JOIN SIMPurchased ON SIMPurchased.UserId = AspNetUsers.Id WHERE OrderDetails.Recurrent = 1 AND Product.ProductTypeId = 1 ORDER BY orders.OrderDate;";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataSet dts = new DataSet();
                data.Fill(dts, "consulta");
                DataTable dt = new DataTable();
                dt = dts.Tables["consulta"];
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    string linea = "DTL|" + i + "|";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        linea+=dr[dc.ColumnName] + "|";
                        Console.Write(dr[dc.ColumnName]);
                        Console.Write(","); //Separa las columnas
                    }
                    Console.WriteLine(); //Salto de línea
                    linea = linea.TrimEnd('|');
                    sw.WriteLine(linea);
                    i++;
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            //}

            /*var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();*/
        }
    }
}

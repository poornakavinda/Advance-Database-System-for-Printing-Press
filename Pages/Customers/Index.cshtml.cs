using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace nma_graphics.Pages.Customers
{
    public class IndexModel : PageModel
    {
        public List<CustomerInfo> ListCustomers = new List<CustomerInfo>();
        public String errorMessage = "";
        public String successMessage = "";
        public String search = "";
        public void OnGet()
        {
            search = Request.Query["search"];
            try
            {
                //String connectionString = "Data Source=DESKTOP-DKT6IOK\\SQLEXPRESS;Initial Catalog=NMA_Graphics;Integrated Security=True";
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING"); 
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM customers WHERE name LIKE '%"+@search+ "%'";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerInfo ci = new CustomerInfo();
                                ci.id = "" + reader.GetInt32(0);
                                ci.name = reader.GetString(1);
                                ci.contactno = reader.GetString(2);
                                ci.address = reader.GetString(3);
                                ci.email = reader.GetString(4);

                                ListCustomers.Add(ci);

                            }
                            if(ListCustomers.Count() == 0)
                            {
                                errorMessage = "Any Customer was not found with name " + search;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class CustomerInfo
    {
        public String id;
        public String name;
        public String contactno;
        public String address;
        public String email;
    }
}

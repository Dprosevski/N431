using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Capstone2nd
{
    public partial class EditProgram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }

            //connect to the database as soon as the page loads so that security questions can be loaded
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            //always use try/catch for db connections
            try
            {

                //get a list of security questions, create list items and add to the drop down list
                //See below using a data access component to replace repetitive codes

                if (!IsPostBack)
                {
                    con.Open();
                    SqlCommand cmd;
                    String uname = Session["email"].ToString();
                    int findUserID = 0;

                    if (Session["userType"].ToString() == "ProgramManager")
                    {
                        cmd = new SqlCommand("SELECT progManagerID FROM ProgramManager WHERE email = @ManagerEmail;", con);
                        cmd.Parameters.Add(new SqlParameter("@ManagerEmail", uname));
                        findUserID = (int)cmd.ExecuteScalar();
                    }

                    if (Session["userType"].ToString() == "Admin")
                    {
                        cmd = new SqlCommand("SELECT adminID FROM Admin WHERE email = @adminEmail;", con);
                        cmd.Parameters.Add(new SqlParameter("@adminEmail", uname));
                        findUserID = (int)cmd.ExecuteScalar();
                    }

                    cmd = new SqlCommand("SELECT * FROM Program WHERE progManagerID = @manager", con);
                    cmd.Parameters.Add(new SqlParameter("@manager", findUserID));

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["name"].ToString());
                        ProgList.Items.Add(NewItem);
                    }
                    con.Close();

                }

            }

            catch (Exception err)
            {
                Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                Response.Write(err.Message);
                lblMessage.Text = "Cannot submit information now. Please try again later.";

            }
            finally //must make sure the connection is properly closed
            { //the finally block will always run whether there is an error or not
                con.Close();
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            String uname = Session["email"].ToString();
            
            Session["programToEdit"] = ProgList.SelectedValue;
            Session["email"] = uname;
            Response.Redirect("EditProgram1.aspx");
        }
    }
}
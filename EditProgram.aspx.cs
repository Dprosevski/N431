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
                if (!IsPostBack)
                {
                    con.Open();
                    if (Session["userType"].ToString() == "ProgramManager")
                    {
                        SqlCommand cmd = new SqlCommand("SELECT progManagerID FROM ProgramManager WHERE email = @ManagerEmail;", con);
                        cmd.Parameters.Add(new SqlParameter("@ManagerEmail", Session["email"]));
                        int findUserID = (int)cmd.ExecuteScalar();

                        cmd = new SqlCommand("SELECT name FROM Program WHERE progManagerID = @manager", con);
                        cmd.Parameters.Add(new SqlParameter("@manager", findUserID));

                        SqlDataReader reader = cmd.ExecuteReader();
                        ProgList.Items.Clear();
                        while (reader.Read())
                        {
                            ListItem NewItem = new ListItem(reader["name"].ToString());
                            ProgList.Items.Add(NewItem);
                        }
                        reader.Close();
                    }

                    if (Session["userType"].ToString() == "Admin")
                    {
                        SqlCommand cmd = new SqlCommand("SELECT name FROM Program", con);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ListItem NewItem = new ListItem(reader["name"].ToString());
                            ProgList.Items.Add(NewItem);
                        }
                        reader.Close();
                    }
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

        protected void ProgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            //always use try/catch for db connections
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT approved FROM Program WHERE name = @ProgList", con);
                cmd.Parameters.Add(new SqlParameter("@ProgList", ProgList.SelectedValue.ToString()));
                string approved = "";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListItem NewItem = new ListItem(reader["approved"].ToString());
                    approved = NewItem.ToString();
                }
                lblApproved.Text = "Currently: " + approved;
                reader.Close();

                //set last updated
                string currentTime = DateTime.Now.ToString(); 
                cmd = new SqlCommand("UPDATE Program SET lastUpdated = @lastUpdated WHERE name = @ProgList", con);
                cmd.Parameters.Add(new SqlParameter("@ProgList", ProgList.SelectedValue.ToString()));
                cmd.Parameters.Add(new SqlParameter("@lastUpdated", currentTime));
                cmd.ExecuteNonQuery();
            }


            catch (Exception err)
            {
                Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                Response.Write(err.Message);
                lblMessage.Text = "Cannot submit information now. Please try again later.";

            }
            finally
            {
                con.Close();
            }
        }
    }
}
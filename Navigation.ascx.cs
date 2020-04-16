using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Capstone2nd
{
    public partial class Navigation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null) //if user is not logged in
            {
                adminPageID.Visible = false;
                newProgID.Visible = false;
                loginID.Visible = true;
                logoutID.Visible = false;
                changePwdID.Visible = false;
            }
            else //if user is logged in
            {
                if (Session["userType"].ToString() == "Admin")
                {
                    newProgID.Visible = true;
                    adminPageID.Visible = true;
                    changePwdID.Visible = true;
                }
                if (Session["userType"].ToString() == "ProgramManager")
                {
                    adminPageID.Visible = false;

                    //Whenever a manager is in the Disapproved status
                    //they can't add a new program and cannot change profile or password
                    string email = Session["email"].ToString();

                    string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(cs);
                    try
                    {
                        con.Open();
                        string sql = "SELECT * FROM ProgramManager WHERE email = '" + email + "'";
                        System.Diagnostics.Debug.WriteLine(sql);

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["approved"].ToString() == "no")
                                    {
                                        newProgID.Visible = false;
                                        changePwdID.Visible = false;
                                    }
                                    else
                                    {
                                        changePwdID.Visible = true;
                                        //inactive program managers can't add/edit programs
                                        if (reader["status"].ToString() == "inactive")
                                        {
                                            newProgID.Visible = false;
                                            //edit program stuff.Visible = false;
                                        }
                                        else
                                        {
                                            newProgID.Visible = true;
                                            //edit program stuff.Visible = true;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        con.Close();
                    }

                    catch (Exception err)
                    {
                        Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                        Response.Write(err.Message);
                    }
                }
                if (Session["userType"].ToString() == "Guest")
                {
                    newProgID.Visible = false;
                    adminPageID.Visible = false;
                    changePwdID.Visible = true;
                }
                loginID.Visible = false;
                logoutID.Visible = true;
            }
        }
    }
}
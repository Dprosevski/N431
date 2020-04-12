using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//must include this namespace
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Capstone2nd
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }
            //else prompt home page
            lblHello.Text = "Hello " + Session["firstName"].ToString() + " " + Session["middleName"].ToString() + " " + Session["lastName"].ToString() + "!";*/
            



        }

   
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                string headSql = "";
                string whereSql = "WHERE ";
                string typeSql = "";
                string modelSql = "";
                string nameSql = "";
                string orderSql = "";

                headSql = "SELECT * FROM Program ";


                if (name.Text.Trim() != "")
                {
                    if (typeSql == "" && modelSql == "") //if the first two strings are empty, this is the start of the where part, no need "And"
                        nameSql = "Lower(name) LIKE '%" + name.Text.Trim().ToLower() + "%' ";
                    else nameSql = " and Lower(name) LIKE '%" + name.Text.Trim().ToLower() + "%' ";

                }


               
                whereSql = whereSql + nameSql;
                lblMessage.Text = headSql + whereSql + orderSql;
                Session["headSQL"] = headSql;
                Session["whereSQL"] = whereSql;
                Response.Redirect("SearchResult.aspx");
            }
            catch (Exception err)
            {
                lblMessage.Text = null;
                lblMessage.Text = "Cannot submit information now. Please try again later.";

            }
            finally
            {
                con.Close();
            }
        }
    }
}
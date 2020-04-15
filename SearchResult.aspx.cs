using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//must include this namespace
using System.Data.SqlClient;
using System.Web.Configuration;


namespace Capstone2nd
{
    public partial class SearchResult : System.Web.UI.Page
    {
        public static SqlConnection con;
        public static String cs;

        static SearchResult()
        {
            cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            con = new SqlConnection(cs);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }*/
            //using data binding features to replace this function
            if (Session["headSQL"] != null || Session["whereSQL"] != null)
                DisplayTransaction(Session["headSQL"].ToString(), Session["whereSQL"].ToString());

            else Response.Redirect("Default.aspx");

        }



        protected void DisplayTransaction(string headSql, string whereSql)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            int count = 0;
            string sql = "SELECT COUNT(*) FROM Program " + whereSql;
            SqlCommand cmd = new SqlCommand(sql, con);
            //cmd.Parameters.Add(new SqlParameter("@whereSql", whereSql));
            count = (int)cmd.ExecuteScalar();

            if (count == 0)
            {
                lblMessage.Text = "No result is found";
            }

            else
            {
                ArrayList res = new ArrayList();
                res = GetRows(headSql + whereSql);

                lblMessage.Text = headSql + whereSql;

                for (int j = 0; j < res.Count; j++)
                {
                    /*[progID], [progManagerID], [name], [acronym], [contactID], [progLocID], [fieldID], [gradesID], [resiID], [programCostID], [durationID], 
                    [seasonID], [serviceAreaID], [stipendID], [addNotes], [appDeadline], [affCollege], [affCompany], [eligibilityRes], [streetAddress], [progWebsite], 
                    [ProgDescription], [status]*/

                    ArrayList oneRow = new ArrayList();
                    oneRow = (ArrayList)res[j];
                    Panel panel = new Panel();
                    panel.ID = "pnlResult" + oneRow[0].ToString();

                    string uniqueRowID = oneRow[0].ToString();

                    string[] listOfColumns = { "progID", "progManagerID", "name", "acronym", "contactID", "progLocID", "fieldID", "gradesID", "resiID", "programCostID",
                            "durationID", "seasonID", "serviceAreaID", "stipendID", "addNotes", "appDeadline", "affCollege", "affCompany", "eligibilityRes", "streetAddress",
                            "progWebsite", "ProgDescription", "status" };

                    string[] listOfColumnsToPrompt = { "progID", "progManagerID", "name", "acronym", "contactID", "Program Location ID: ", "fieldID", "gradesID", "resiID", "programCostID",
                            "durationID", "seasonID", "serviceAreaID", "stipendID", "addNotes", "appDeadline", "affCollege", "affCompany", "eligibilityRes", "streetAddress",
                            "progWebsite", "ProgDescription", "status" };

                    for (int k = 0; k < oneRow.Count; k++)
                    {
                        panel.Controls.Add(createLabelName(listOfColumns[k], uniqueRowID, listOfColumnsToPrompt[k]));
                        panel.Controls.Add(createLabelValue(listOfColumns[k] + "Val", uniqueRowID, oneRow[k].ToString()));
                        //add blank
                        Label lblBlank = new Label();
                        lblBlank.Text = "<br />";
                        panel.Controls.Add(lblBlank);
                    }

                        panel.BorderWidth = Unit.Pixel(5);

                    PnlTrans.Controls.Add(panel);
                }

            }
            con.Close();
        }

        protected Label createLabelName(string lblID, string onerow, string text)
        {
            Label newLbl = new Label();
            newLbl.ID = lblID + onerow;
            newLbl.Height = 14;
            newLbl.Font.Size = 12;
            newLbl.ForeColor = System.Drawing.Color.Black;
            newLbl.Font.Bold = true;
            newLbl.Text = text;

            return newLbl;
        }

        protected Label createLabelValue(string lblID, string onerow, string text)
        {
            Label newLbl = new Label();
            newLbl.ID = lblID + onerow;
            newLbl.Height = 12;
            newLbl.Font.Size = 10;
            newLbl.ForeColor = System.Drawing.Color.Black;
            newLbl.Text = text;

            return newLbl;
        }

        public static ArrayList GetRows(string sql)
        {

            try
            {
                ArrayList res = new ArrayList();
                con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);


                SqlDataReader reader = cmd.ExecuteReader();
                // int rowCount = 0;
                while (reader.Read())
                {
                    ArrayList row = new ArrayList();
                    for (int field = 0; field < reader.FieldCount; field++)
                    {
                        string oneValue = reader.GetValue(field).ToString();

                        row.Add(oneValue);
                    }
                    //add the row to the table
                    res.Add(row);
                    //rowCount++;
                }
                reader.Close();
                con.Close();

                return res;
            }
            catch (Exception err)
            {
                err.ToString();
                ArrayList r = new ArrayList();
                return r;
            }
            finally
            {
                con.Close();

            }
        }
    }//public partial
}//namespace
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
            if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }

            //else prompt home page
            lblHello.Text = "Hello " + Session["firstName"].ToString() + " " + Session["middleName"].ToString() + " " + Session["lastName"].ToString() + "!";

            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataReader reader;
            SqlCommand cmd;

            //populate the dropdown lists 
            try
            {
                con.Open();
                lblMessage.Text = null;
                if (!IsPostBack)
                {
                    //prompt a message to PM if approved or not
                    if (Session["userType"].ToString() == "ProgramManager")
                    {
                        cmd = new SqlCommand("SELECT approved FROM ProgramManager WHERE email = @email", con);
                        cmd.Parameters.Add(new SqlParameter("@email", Session["email"]));
                        string approved = "";
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ListItem NewItem = new ListItem(reader["approved"].ToString());
                            approved = NewItem.ToString();
                        }
                        reader.Close();

                        if (approved == "yes")
                        {
                            lblMessage.Text = "Currently your account is approved!";
                            lblMessage.ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            lblMessage.Text = "Currently your account is NOT approved! It will go through an approval process and someone will you contact by" +
                                " email once it is approved.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }


                    //populate field dropdown
                    int selectedIndex = fieldList.SelectedIndex;

                    fieldList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM FieldOfStudy", con);
                    reader = cmd.ExecuteReader();
                    fieldList.Items.Add(new ListItem("Any", "-1"));//add Any option

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        fieldList.Items.Add(NewItem);
                    }

                    fieldList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate grade dropdown
                    selectedIndex = gradeList.SelectedIndex;

                    gradeList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Grades", con);
                    gradeList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        gradeList.Items.Add(NewItem);
                    }
                    gradeList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate residential dropdown
                    selectedIndex = residentialList.SelectedIndex;

                    residentialList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Residental", con);
                    residentialList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        residentialList.Items.Add(NewItem);
                    }

                    residentialList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate cost dropdown
                    selectedIndex = costList.SelectedIndex;

                    costList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM ProgramCost", con);
                    costList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        costList.Items.Add(NewItem);
                    }
                    costList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate stipend dropdown
                    selectedIndex = stipendList.SelectedIndex;

                    stipendList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Stipend", con);
                    stipendList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        stipendList.Items.Add(NewItem);
                    }
                    stipendList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate duration dropdown
                    selectedIndex = durationList.SelectedIndex;

                    durationList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Duration", con);
                    durationList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        durationList.Items.Add(NewItem);
                    }
                    durationList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate season dropdown
                    selectedIndex = seasonList.SelectedIndex;

                    seasonList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Season", con);
                    seasonList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        seasonList.Items.Add(NewItem);
                    }
                    seasonList.SelectedIndex = selectedIndex;
                    reader.Close();


                    //populate area dropdown
                    selectedIndex = areaList.SelectedIndex;

                    areaList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM ServiceArea", con);
                    areaList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        areaList.Items.Add(NewItem);
                    }
                    areaList.SelectedIndex = selectedIndex;
                    reader.Close();


                    //populate restrictions dropdown
                    /*selectedIndex = restrictionsList.SelectedIndex;

                    restrictionsList.Items.Clear();
                    cmd = new SqlCommand("SELECT restrictions FROM Program", con);
                    restrictionsList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["restrictions"].ToString());
                        restrictionsList.Items.Add(NewItem);
                    }
                    restrictionsList.SelectedIndex = selectedIndex;
                    reader.Close();*/

                    //populate state dropdown
                    selectedIndex = stateList.SelectedIndex;

                    stateList.Items.Clear();
                    cmd = new SqlCommand("SELECT name FROM State", con);
                    stateList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["name"].ToString());
                        stateList.Items.Add(NewItem);
                    }
                    stateList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate county dropdown
                    selectedIndex = countyList.SelectedIndex;

                    countyList.Items.Clear();
                    cmd = new SqlCommand("SELECT county FROM ProgLoc", con);
                    countyList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["county"].ToString());
                        countyList.Items.Add(NewItem);
                    }
                    countyList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate city dropdown
                    selectedIndex = cityList.SelectedIndex;

                    cityList.Items.Clear();
                    cmd = new SqlCommand("SELECT city FROM ProgLoc", con);
                    cityList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["city"].ToString());
                        cityList.Items.Add(NewItem);
                    }
                    cityList.SelectedIndex = selectedIndex;
                    reader.Close();
                    con.Close();
                }
            }

            catch (Exception err)
            {
                lblMessage.Text = null;
                lblMessage.Text = "Cannot submit information now. Please try again later.";
            }
            finally
            {
            }
        }


        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                bool whereEmpty = true; //to show if WHERE statement has any parameters
                string headSql = "";
                string whereSql = "WHERE ";
                string nameSql = "";
                string fieldSql = "";
                string gradeSql = "";
                string residentalSql = "";
                string costSql = "";
                string stipendSql = "";
                string durationSql = "";
                string seasonSql = "";
                string areaSql = "";
                string startDateSql = "";
                string appDeadlineSql = "";
                string affiliationSql = "";
                string restrictionsSql = "";
                string stateSql = "";
                string countySql = "";
                string citySql = "";


                headSql = "SELECT * FROM Search ";

                //Search by name
                if (name.Text.Trim() != "")
                {
                    if (whereEmpty == true)
                    {
                        nameSql = "Lower(progName) LIKE '%" + name.Text.Trim().ToLower() + "%' ";
                        whereEmpty = false;
                    }
                    else
                        nameSql = " and Lower(progName) LIKE '%" + name.Text.Trim().ToLower() + "%' ";
                }

                //Search by field of study
                if (fieldList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        fieldSql = "fieldOfStudy = '" + fieldList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        fieldSql = " and fieldOfStudy = '" + fieldList.SelectedItem.Value + "'";
                }

                //Search by grades
                if (gradeList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        gradeSql = "grade = '" + gradeList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        gradeSql = " and grade = '" + gradeList.SelectedItem.Value + "'";
                }

                //Search by residental
                if (residentialList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        residentalSql = "residental = '" + residentialList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        residentalSql = " and residental = '" + residentialList.SelectedItem.Value + "'";
                }

                //Search by cost
                if (costList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        costSql = "cost = '" + costList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        costSql = " and cost = '" + costList.SelectedItem.Value + "'";
                }

                //Search by stipend
                if (stipendList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        stipendSql = "stipend = '" + stipendList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        stipendSql = " and stipend = '" + stipendList.SelectedItem.Value + "'";
                }

                //Search by duration
                if (durationList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        durationSql = "duration = '" + durationList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        durationSql = " and duration = '" + durationList.SelectedItem.Value + "'";
                }

                //Search by season
                if (seasonList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        seasonSql = "season = '" + seasonList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        seasonSql = " and season = '" + seasonList.SelectedItem.Value + "'";
                }

                //Search by service area
                if (areaList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        areaSql = "serviceArea = '" + areaList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        areaSql = " and serviceArea = '" + areaList.SelectedItem.Value + "'";
                }

                //Search by start date
                if (startDate.Text != "")
                {
                    if (whereEmpty == true)
                    {
                        startDateSql = "startDate = '" + startDate.Text + "'";
                        whereEmpty = false;
                    }
                    else
                        startDateSql = " and startDate = '" + startDate.Text + "'";
                }

                if (startDate.Text == "")
                {
                    startDateSql = "";
                }

                //Search by app deadline
                if (appDeadline.Text != "")
                {
                    if (whereEmpty == true)
                    {
                        appDeadlineSql = "appDeadline = '" + appDeadline.Text + "'";
                        whereEmpty = false;
                    }
                    else
                        appDeadlineSql = " and  appDeadline = '" + appDeadline.Text + "'";
                }

                if (appDeadline.Text == "")
                {
                    appDeadlineSql = "";
                }


                //Search by affiliation
                if (affiliation.Text.Trim() != "")
                {
                    if (whereEmpty == true)
                    {
                        affiliationSql = "Lower(affiliationDescription) LIKE '%" + affiliation.Text.Trim().ToLower() + "%' ";
                        whereEmpty = false;
                    }
                    else
                        affiliationSql = " and Lower(affiliationDescription) LIKE '%" + affiliation.Text.Trim().ToLower() + "%' ";
                }


                //Search by restrictions
                if (restrictions.Text.Trim() != "")
                {
                    if (whereEmpty == true)
                    {
                        restrictionsSql = "Lower(restrictionsDescription) LIKE '%" + restrictions.Text.Trim().ToLower() + "%' ";
                        whereEmpty = false;
                    }
                    else
                        restrictionsSql = " and Lower(restrictionsDescription) LIKE '%" + restrictions.Text.Trim().ToLower() + "%' ";
                }


                //Search by State
                if (stateList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        stateSql = "stateName = '" + stateList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        stateSql = " and stateName = '" + stateList.SelectedItem.Value + "'";
                }


                //Search by County
                if (countyList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        countySql = "county = '" + countyList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        countySql = " and county = '" + countyList.SelectedItem.Value + "'";
                }


                //Search by City
                if (cityList.SelectedItem.Value != "-1")
                {
                    if (whereEmpty == true)
                    {
                        citySql = "city = '" + cityList.SelectedItem.Value + "'";
                        whereEmpty = false;
                    }
                    else
                        citySql = " and city = '" + cityList.SelectedItem.Value + "'";
                }


                whereSql = whereSql + nameSql + fieldSql + gradeSql + residentalSql + costSql + stipendSql + durationSql + seasonSql + areaSql
                    + startDateSql + appDeadlineSql + residentalSql + affiliationSql + restrictionsSql + stateSql + countySql + citySql;
                lblMessage.Text = headSql + whereSql;
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

        protected void stateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataReader reader;
            SqlCommand cmd;
            con.Open();

            if (stateList.SelectedValue == "-1")
            {
                return;
            }

            foreach (ListItem li in stateList.Items)
            {
                if (li.Selected)
                {
                    String state = li.Value;
                    cmd = new SqlCommand("SELECT stateID FROM State WHERE name = @state", con);
                    cmd.Parameters.Add(new SqlParameter("@state", state));
                    int findStateID = (int)cmd.ExecuteScalar();

                    cmd = new SqlCommand("SELECT DISTINCT county FROM ProgLoc WHERE stateID = @stateID and county != ''", con);
                    cmd.Parameters.Add(new SqlParameter("@stateID", findStateID));

                    reader = cmd.ExecuteReader();
                    countyList.Items.Clear();
                    countyList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["county"].ToString());
                        countyList.Items.Add(NewItem);
                    }
                    reader.Close();
                    break;
                }
            }
            con.Close();
        }

        protected void countyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataReader reader;
            SqlCommand cmd;
            con.Open();
            if (countyList.SelectedValue == "-1")
            {
                return;
            }

            foreach (ListItem li in countyList.Items)
            {
                if (li.Selected)
                {

                    String State = stateList.SelectedValue;
                    cmd = new SqlCommand("SELECT stateID FROM State WHERE name = @state", con);
                    cmd.Parameters.Add(new SqlParameter("@state", State));
                    int findStateID = (int)cmd.ExecuteScalar();


                    cmd = new SqlCommand("SELECT city FROM ProgLoc WHERE stateID = @stateID AND county = @county", con);
                    cmd.Parameters.Add(new SqlParameter("@stateID", findStateID));
                    cmd.Parameters.Add(new SqlParameter("@county", li.Value));

                    reader = cmd.ExecuteReader();
                    cityList.Items.Clear();
                    cityList.Items.Add(new ListItem("Any", "-1"));//add Any option
                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["city"].ToString());
                        cityList.Items.Add(NewItem);
                    }
                    reader.Close();
                    break;
                }
            }
            con.Close();
        }//county list
    }
}
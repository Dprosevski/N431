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
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }
            //if super admin
            if (Session["accessLevel"].ToString() == "s")
            {
                addNewAdminID.Visible = true;
                lblFindAdminID.Visible = true;
                adminList.Visible = true;
                editAdminID.Visible = true;
            }

            if (Session["message"] != null)
            {
                lblMessage.Text = Session["message"].ToString();
                Session["message"] = null;
            }

            populateData(false);
        }

        protected void editAdminID_Click(object sender, EventArgs e)
        {
            Session["adminToEdit"] = adminList.SelectedItem.Text;
            Response.Redirect("EditAdmin.aspx");
        }

        protected void Change_Order(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                DropDownList selectedList = sender as DropDownList;
                string senderID = selectedList.ID;
                string newOrder = selectedList.SelectedItem.Value;
                string tableName = String.Empty;
                string idName = String.Empty;

                if (senderID == "fieldOrder")
                {
                    tableName = "FieldOfStudy";
                    idName = "fieldID";
                }

                //last part of the statement simply lets us update everything at once
                //since every entry should have the same kind of ordering
                String sql = "UPDATE " + tableName + " SET \"order\" = @order WHERE @idName IS NOT NULL;";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@order", newOrder));
                cmd.Parameters.Add(new SqlParameter("@idName", idName));
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
                
                //if they chose custom ordering for the first time, the "customOrder" field must be initialized 
                //initialize to ID position by default
                if (newOrder == "custom")
                {
                    sql = "SELECT * FROM " + tableName;
                    cmd = new SqlCommand(sql, con);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (DBNull.Value.Equals(reader["customOrder"]))
                            {
                                SqlConnection con2 = new SqlConnection(cs);
                                con2.Open();
                                sql = "UPDATE " + tableName + " SET customOrder = " + idName + " WHERE @idName IS NOT NULL;";
                                SqlCommand cmd2 = new SqlCommand(sql, con2);
                                cmd2.Parameters.Add(new SqlParameter("@idName", idName));
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                                con2.Close();
                            }

                            else
                            {
                                System.Diagnostics.Debug.WriteLine("NOT NULL\n\n\n\n\n");
                            }

                            cmd.Dispose();
                            break;
                        }
                    }
                }
            }

            catch (Exception err)
            {
                Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                Response.Write(err.Message);
            }

            finally
            {
                con.Close();
                populateData(true);
            }
        }

        //if the entry is new it will postback
        protected void populateData(bool isNew)
        {
            if (!IsPostBack || isNew)
            {
                string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);

                //populate the dropdown lists 
                try
                {
                    con.Open();

                    //iterate through the page to fill each list
                    List<DropDownList> dropDowns = new List<DropDownList>();
                    dropDowns.Add(fieldList);
                    dropDowns.Add(roleList);
                    dropDowns.Add(gradeList);
                    dropDowns.Add(residentialList);
                    dropDowns.Add(costList);
                    dropDowns.Add(stipendList);
                    dropDowns.Add(durationList);
                    dropDowns.Add(seasonList);
                    dropDowns.Add(areaList);
                    dropDowns.Add(adminList);

                    //need to add each DB name to a list as well
                    //be mindful of order
                    List<string> DBNames = new List<string>();
                    DBNames.Add("FieldOfStudy");
                    DBNames.Add("ManagerRole");
                    DBNames.Add("Grades");
                    DBNames.Add("Residental");
                    DBNames.Add("ProgramCost");
                    DBNames.Add("Stipend");
                    DBNames.Add("Duration");
                    DBNames.Add("Season");
                    DBNames.Add("ServiceArea");
                    DBNames.Add("Admin");

                    //store primary keys for if we're sorting by date
                    List<string> primKeys = new List<string>();
                    primKeys.Add("fieldID");
                    primKeys.Add("roleID");
                    primKeys.Add("gradesID");
                    primKeys.Add("resiID");
                    primKeys.Add("programCostID");
                    primKeys.Add("stipendID");
                    primKeys.Add("durationID");
                    primKeys.Add("seasonID");
                    primKeys.Add("serviceAreaID");
                    primKeys.Add("adminID");

                    List<DropDownList> orderLists = new List<DropDownList>();
                    orderLists.Add(fieldOrder);

                    //must make these visible if user selected custom
                    List<Label> customLabels = new List<Label>();
                    customLabels.Add(fieldCustomLbl);

                    //custom ordering drop downs
                    List<DropDownList> customLists = new List<DropDownList>();
                    customLists.Add(fieldCustomOrder);

                    for (int i = 0; i < dropDowns.Count; i++)
                    {
                        DropDownList list = dropDowns[i];
                        string DBName = DBNames[i];
                        DropDownList orderList = orderLists[i];
                        Label customLbl = customLabels[i];
                        DropDownList customList = customLists[i];

                        int selectedIndex = list.SelectedIndex;
                        list.Items.Clear();

                        System.Diagnostics.Debug.WriteLine("DBNAME : " + DBName);
                        System.Diagnostics.Debug.WriteLine("LIST : " + list.ID);

                        //not every value column name is the same
                        //hopefully all of them will be "value" at some point but I don't want to break anything rn
                        string colName = String.Empty;
                        if (DBName == "ManagerRole")
                        {
                            colName = "roleName";
                        }
                        else if (DBName == "Admin")
                        {
                            colName = "email";
                        }
                        else
                        {
                            colName = "value";
                        }

                        //generate a SQL query based on ordering scheme
                        String sql = "SELECT \"order\" FROM " + DBName;
                        String orderType = String.Empty;
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //all "order" entries should be the same so only need to read one
                                while (reader.Read())
                                {
                                    orderType = reader["order"].ToString();
                                    break;
                                }
                            }

                            if (orderType == "alpha")
                            {
                                orderList.SelectedIndex = 0;
                                sql = "SELECT * FROM " + DBName + " ORDER BY " + colName;
                            }

                            else if (orderType == "custom")
                            {
                                orderList.SelectedIndex = 2;
                                customList.Visible = true;
                                customLbl.Visible = true;
                                customList.Items.Clear();
                                customList.Items.Add(new ListItem(String.Empty, String.Empty));

                                sql = "SELECT * FROM " + DBName + " ORDER BY customOrder";
                            }

                            //otherwise order by date added aka id
                            else
                            {
                                orderList.SelectedIndex = 1;
                                sql = "SELECT * FROM " + DBName + " ORDER BY " + primKeys[i];
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("SQL: " + sql);
                        System.Diagnostics.Debug.WriteLine("ORDER TYPE : " + orderType);

                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //populate the dropdown
                                while (reader.Read())
                                {
                                    ListItem newItem = new ListItem(reader[colName].ToString());
                                    list.Items.Add(newItem);

                                    if (orderType == "custom")
                                    {
                                        customList.Items.Add(reader["customOrder"].ToString());
                                    }
                                }

                                list.SelectedIndex = selectedIndex;
                                customList.SelectedIndex = 0;
                                System.Diagnostics.Debug.WriteLine("OMG XDDDDDD " + customList.SelectedIndex);
                            }
                        }
                    }

                    /*
                    cmd = new SqlCommand(sql, con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        fieldList.Items.Add(NewItem);
                    }

                    fieldList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate role dropdown
                    selectedIndex = roleList.SelectedIndex;
                    roleList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM ManagerRole", con);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["roleName"].ToString());
                        roleList.Items.Add(NewItem);
                    }

                    roleList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate grade dropdown
                    selectedIndex = gradeList.SelectedIndex;
                    gradeList.Items.Clear();
                    cmd = new SqlCommand("SELECT * FROM Grades", con);
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
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["value"].ToString());
                        areaList.Items.Add(NewItem);
                    }
                    areaList.SelectedIndex = selectedIndex;
                    reader.Close();

                    //populate admin dropdown
                    selectedIndex = adminList.SelectedIndex;

                    adminList.Items.Clear();
                    cmd = new SqlCommand("SELECT email FROM Admin", con);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListItem NewItem = new ListItem(reader["email"].ToString());
                        adminList.Items.Add(NewItem);
                    }
                    adminList.SelectedIndex = selectedIndex;
                    reader.Close();
                    */
                }

                catch (Exception err)
                {
                    lblMessage.Text = null;
                    lblMessage.Text = "Cannot submit information now. Please try again later.";
                }
                finally
                {
                    con.Close();
                    //Session["message"] = "Changes successfully applied!";
                }

                Index_Change(null, null);
            }
        }

        //set the checkbox to the appropriate value
        protected void Index_Change(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM FieldOfStudy;", con);

                SqlDataReader reader = cmd.ExecuteReader();
                string field = fieldList.SelectedItem.Value;

                while (reader.Read())
                {
                    if (field == reader["value"].ToString())
                    {
                        if (reader["status"].ToString() == "active")
                        {
                            fieldActive.Checked = true;
                        }
                        else
                        {
                            fieldActive.Checked = false;
                        }
                    }
                }
                reader.Close();
            }

            catch (Exception err)
            {
                lblMessage.Text = null;
                lblMessage.Text = "Cannot submit information now. Please try again later.";
            }
            finally
            {
                con.Close();
                //Session["message"] = "Changes successfully applied!";
            }
        }

        protected void submitNew(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string isActive = "active";

                //find which button called this function and set the table name appropriately
                Button clicked = sender as Button;
                string senderID = clicked.ID;
                string tableName = "";
                string value = "";
                string valName = "";

                if (senderID == "submitNewField")
                {
                    tableName = "FieldOfStudy";
                    value = newField.Text;
                    valName = "value";
                    newField.Text = string.Empty;

                    if (newFieldActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewRole")
                {
                    tableName = "ManagerRole";
                    value = newRole.Text;
                    valName = "roleName";
                    newRole.Text = string.Empty;

                    /*
                    if (newRoleActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                    */
                }

                else if (senderID == "submitNewGrade")
                {
                    tableName = "Grades";
                    value = newGrade.Text;
                    valName = "value";
                    newGrade.Text = string.Empty;

                    if (newGradeActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewResidential")
                {
                    tableName = "Residental";
                    value = newResidential.Text;
                    valName = "value";
                    newResidential.Text = string.Empty;

                    if (newResidentialActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewCost")
                {
                    tableName = "ProgramCost";
                    value = newCost.Text;
                    valName = "value";
                    newCost.Text = string.Empty;

                    if (newCostActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewStipend")
                {
                    tableName = "Stipend";
                    value = newStipend.Text;
                    valName = "value";
                    newStipend.Text = string.Empty;

                    if (newStipendActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewDuration")
                {
                    tableName = "Duration";
                    value = newDuration.Text;
                    valName = "value";
                    newDuration.Text = string.Empty;

                    if (newDurationActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewSeason")
                {
                    tableName = "Season";
                    value = newSeason.Text;
                    valName = "value";
                    newSeason.Text = string.Empty;

                    if (newSeasonActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                else if (senderID == "submitNewArea")
                {
                    tableName = "ServiceArea";
                    value = newArea.Text;
                    valName = "value";
                    newArea.Text = string.Empty;

                    if (newAreaActive.Checked == false)
                    {
                        isActive = "inactive";
                    }
                }

                string sql = "INSERT INTO " + tableName + " (" + valName + ", status) values(@value, @isActive);";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@value", value));
                cmd.Parameters.Add(new SqlParameter("@isActive", isActive));
                cmd.ExecuteNonQuery();
            }

            catch (Exception err)
            {
                lblMessage.Text = null;
                lblMessage.Text = "Cannot submit information now. Please try again later.";
            }
            finally
            {
                con.Close();
                Session["message"] = "Changes successfully applied!";
            }
            populateData(true);
        }

        protected void editSelected(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string isActive = "active";
                string value = String.Empty;
                string newValue = String.Empty;
                string status = String.Empty;
                string editValue = String.Empty;
                string newStatus = "";
                string sql = "";

                //find which button called this function and set the table name appropriately
                Button clicked = sender as Button;
                string senderID = clicked.ID;
                string tableName = "";
                string idName = "";
                if (senderID == "fieldEdit")
                {
                    tableName = "FieldOfStudy";
                    value = fieldList.SelectedItem.Value;
                    newValue = editProgField.Text;
                    idName = "value";

                    if (fieldActive.Checked)
                    {
                        newStatus = "active";
                    }

                    else
                    {
                        newStatus = "inactive";
                    }

                    editValue = editProgField.Text;
                    editProgField.Text = string.Empty;

                    //find if this field uses custom ordering
                    sql = "SELECT \"order\" FROM " + tableName;
                    String orderType = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            //all "order" entries should be the same so only need to read one
                            while (reader.Read())
                            {
                                orderType = reader["order"].ToString();
                                break;
                            }
                        }
                    }

                    //only put through changes if the empty option (selected by default) is not chosen
                    if (orderType == "custom")
                    {
                        if (fieldCustomOrder.SelectedValue != String.Empty)
                        {
                            int oldCustomOrder = -1;
                            int newCustomOrder = int.Parse(fieldCustomOrder.SelectedValue);

                            //first get the current custom order of the selected entry
                            sql = "SELECT * FROM  " + tableName + " WHERE " + idName + " = '" + value + "'";
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        oldCustomOrder = int.Parse(reader["customOrder"].ToString());
                                        System.Diagnostics.Debug.WriteLine("OLD CUST ORDER " + oldCustomOrder);
                                        break;
                                    }
                                }
                            }

                            sql = "SELECT * FROM " + tableName;
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        int curCustomOrder = int.Parse(reader["customOrder"].ToString());
                                        int curID = int.Parse(reader["fieldID"].ToString());

                                        if (reader["value"].ToString() == value)
                                        {
                                            using (SqlConnection con2 = new SqlConnection(cs))
                                            {
                                                con2.Open();
                                                string sql2 = "UPDATE " + tableName + " SET customOrder = " + newCustomOrder + " WHERE fieldID = " + curID + ";";
                                                using (SqlCommand cmd2 = new SqlCommand(sql2, con2))
                                                {
                                                    cmd2.ExecuteNonQuery();
                                                }
                                                con2.Close();
                                            }
                                        }
                                        else if ((curCustomOrder > oldCustomOrder && curCustomOrder > newCustomOrder) || (curCustomOrder < oldCustomOrder && curCustomOrder < newCustomOrder))
                                        {
                                            //don't need to change anything lol
                                        }
                                        else
                                        {
                                            if (newCustomOrder > oldCustomOrder)
                                            {
                                                curCustomOrder -= 1;
                                            }

                                            else if (newCustomOrder < oldCustomOrder)
                                            {
                                                curCustomOrder += 1;
                                            }

                                            using (SqlConnection con2 = new SqlConnection(cs))
                                            {
                                                con2.Open();
                                                string sql2 = "UPDATE " + tableName + " SET customOrder = " + curCustomOrder + " WHERE fieldID = " + curID + ";";
                                                using (SqlCommand cmd2 = new SqlCommand(sql2, con2))
                                                {
                                                    cmd2.ExecuteNonQuery();
                                                }
                                                con2.Close();
                                            }
                                        }
                                        System.Diagnostics.Debug.WriteLine(newCustomOrder);
                                        populateData(false);
                                    }
                                }
                            }
                        }
                    }

                    if (senderID == "roleEditBtn")
                    {
                        tableName = "ManagerRole";
                        value = roleList.SelectedItem.Value;
                        newValue = roleEdit.Text;
                        idName = "roleName";

                        if (roleActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = roleEdit.Text;
                        roleEdit.Text = string.Empty;
                    }

                    if (senderID == "gradeEditBtn")
                    {
                        tableName = "Grades";
                        value = gradeList.SelectedItem.Value;
                        newValue = gradeEdit.Text;
                        idName = "value";

                        if (gradeActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = gradeEdit.Text;
                        gradeEdit.Text = string.Empty;
                    }

                    if (senderID == "residentialEditBtn")
                    {
                        tableName = "Residental";
                        value = residentialList.SelectedItem.Value;
                        newValue = residentialEdit.Text;
                        idName = "value";

                        if (residentialActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = residentialEdit.Text;
                        residentialEdit.Text = string.Empty;
                    }

                    if (senderID == "costEditBtn")
                    {
                        tableName = "ProgramCost";
                        value = costList.SelectedItem.Value;
                        newValue = costEdit.Text;
                        idName = "value";

                        if (costActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = costEdit.Text;
                        costEdit.Text = string.Empty;
                    }

                    if (senderID == "stipendEditBtn")
                    {
                        tableName = "Stipend";
                        value = stipendList.SelectedItem.Value;
                        newValue = stipendEdit.Text;
                        idName = "value";

                        if (stipendActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = stipendEdit.Text;
                        stipendEdit.Text = string.Empty;
                    }

                    if (senderID == "durationEditBtn")
                    {
                        tableName = "Duration";
                        value = durationList.SelectedItem.Value;
                        newValue = durationEdit.Text;
                        idName = "value";

                        if (durationActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = durationEdit.Text;
                        durationEdit.Text = string.Empty;
                    }

                    if (senderID == "seasonEditBtn")
                    {
                        tableName = "Season";
                        value = seasonList.SelectedItem.Value;
                        newValue = seasonEdit.Text;
                        idName = "value";

                        if (seasonActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = seasonEdit.Text;
                        seasonEdit.Text = string.Empty;
                    }

                    if (senderID == "areaEditBtn")
                    {
                        tableName = "ServiceArea";
                        value = areaList.SelectedItem.Value;
                        newValue = areaEdit.Text;
                        idName = "value";

                        if (areaActive.Checked)
                        {
                            newStatus = "active";
                        }

                        else
                        {
                            newStatus = "inactive";
                        }

                        editValue = areaEdit.Text;
                        areaEdit.Text = string.Empty;
                    }

                    if (value != String.Empty)
                    {
                        SqlCommand cmd = new SqlCommand("SELECT status FROM " + tableName + " WHERE " + idName + "= @value;", con);
                        cmd.Parameters.Add(new SqlParameter("@value", value));

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            status = reader["status"].ToString();
                        }

                        reader.Close();

                        if (status != String.Empty)
                        {
                            sql = "UPDATE " + tableName + " SET status= @status WHERE " + idName + "= @value;";
                            cmd = new SqlCommand(sql, con);
                            cmd.Parameters.Add(new SqlParameter("@value", value));
                            cmd.Parameters.Add(new SqlParameter("@status", newStatus));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        //update field name then clear the text box
                        if (editValue != String.Empty)
                        {
                            sql = "UPDATE " + tableName + " SET " + idName + "= @newValue WHERE " + idName + "= @value;";
                            cmd = new SqlCommand(sql, con);
                            cmd.Parameters.Add(new SqlParameter("@value", value));
                            cmd.Parameters.Add(new SqlParameter("@newValue", newValue));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
            }

            catch (Exception err)
            {
                Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                Response.Write(err.Message);
            }

            //refresh drop down lists
            populateData(true);
        }
    }
}

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
                else if (senderID == "managerOrder")
                {
                    tableName = "ManagerRole";
                    idName = "roleID";
                }
                else if (senderID == "gradeOrder")
                {
                    tableName = "Grades";
                    idName = "gradesID";
                }
                else if (senderID == "residentialOrder")
                {
                    tableName = "Residental";
                    idName = "resiID";
                }
                else if (senderID == "costOrder")
                {
                    tableName = "ProgramCost";
                    idName = "ProgramCostID";
                }
                else if (senderID == "stipendOrder")
                {
                    tableName = "Stipend";
                    idName = "stipendID";
                }
                else if (senderID == "durationOrder")
                {
                    tableName = "Duration";
                    idName = "durationID";
                }
                else if (senderID == "seasonOrder")
                {
                    tableName = "Season";
                    idName = "seasonID";
                }
                else if (senderID == "areaOrder")
                {
                    tableName = "ServiceArea";
                    idName = "serviceAreaID";
                }

                //last part of the statement simply lets us update everything at once
                //since every entry should have the same kind of ordering
                String sql = "UPDATE " + tableName + " SET myOrder = @order WHERE @idName IS NOT NULL;";
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
                    //primKeys.Add("adminID");

                    List<DropDownList> orderLists = new List<DropDownList>();
                    orderLists.Add(fieldOrder);
                    orderLists.Add(managerOrder);
                    orderLists.Add(gradeOrder);
                    orderLists.Add(residentialOrder);
                    orderLists.Add(costOrder);
                    orderLists.Add(stipendOrder);
                    orderLists.Add(durationOrder);
                    orderLists.Add(seasonOrder);
                    orderLists.Add(areaOrder);

                    //must make these visible if user selected custom
                    List<Label> customLabels = new List<Label>();
                    customLabels.Add(fieldCustomLbl);
                    customLabels.Add(managerCustomLbl);
                    customLabels.Add(gradeCustomLbl);
                    customLabels.Add(residentialCustomLbl);
                    customLabels.Add(costCustomLbl);
                    customLabels.Add(stipendCustomLbl);
                    customLabels.Add(durationCustomLbl);
                    customLabels.Add(seasonCustomLbl);
                    customLabels.Add(areaCustomLbl);

                    //custom ordering drop downs
                    List<DropDownList> customLists = new List<DropDownList>();
                    customLists.Add(fieldCustomOrder);
                    customLists.Add(managerCustomOrder);
                    customLists.Add(gradeCustomOrder);
                    customLists.Add(residentialCustomOrder);
                    customLists.Add(costCustomOrder);
                    customLists.Add(stipendCustomOrder);
                    customLists.Add(durationCustomOrder);
                    customLists.Add(seasonCustomOrder);
                    customLists.Add(areaCustomOrder);

                    //program manager dropdowns
                    string sql = "SELECT * FROM ProgramManager";
                    approvedManagers.Items.Clear();
                    unapprovedManagers.Items.Clear();
                    activeManagers.Items.Clear();
                    inactiveManagers.Items.Clear();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListItem newItem = new ListItem(reader["email"].ToString());
                                if (reader["approved"].ToString() == "no")
                                {
                                    unapprovedManagers.Items.Add(newItem);
                                }
                                else
                                {
                                    approvedManagers.Items.Add(newItem);
                                }
                                if (reader["status"].ToString() == "active")
                                {
                                    activeManagers.Items.Add(newItem);
                                }
                                else
                                {
                                    inactiveManagers.Items.Add(newItem);
                                }
                            }
                        }
                    }
                    sql = "SELECT * FROM Admin";
                    adminList.Items.Clear();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListItem newItem = new ListItem(reader["email"].ToString());
                                adminList.Items.Add(newItem);
                            }
                        }
                    }

                    for (int i = 0; i < dropDowns.Count; i++)
                    {
                        DropDownList list = dropDowns[i];
                        string DBName = DBNames[i];

                        DropDownList orderList = null;
                        Label customLbl = null;
                        DropDownList customList = null;
                        if (DBName != "Admin")
                        {
                            orderList = orderLists[i];
                            customLbl = customLabels[i];
                            customList = customLists[i];
                        }

                        string selectedVal = list.SelectedValue;
                        int selectedIndex = list.SelectedIndex;
                        list.Items.Clear();
                       
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
                        sql = "SELECT myOrder FROM " + DBName;
                        System.Diagnostics.Debug.WriteLine(sql);
                        String orderType = String.Empty;
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //all "order" entries should be the same so only need to read one
                                while (reader.Read())
                                {
                                    orderType = reader["myOrder"].ToString();
                                    break;
                                }
                            }

                            if (orderType == "custom")
                            {
                                string customTxt = customLbl.Text;
                                string curCustVal = String.Empty;
                                sql = "SELECT * FROM " + DBName + " WHERE " + colName + " = " + selectedVal;

                                /*
                                    * custom order labeling stuff:
                                if (selectedVal != string.Empty)
                                {
                                    using (SqlConnection con2 = new SqlConnection(cs))
                                    {
                                        con2.Open();

                                        System.Diagnostics.Debug.WriteLine(sql);
                                        using (SqlCommand cmd2 = new SqlCommand(sql, con2))
                                        {
                                            using (SqlDataReader reader = cmd2.ExecuteReader())
                                            {
                                                curCustVal = reader["customOrder"].ToString();
                                            }
                                            customTxt += ", current position = " + curCustVal;
                                            customLbl.Text = customTxt;
                                        }
                                        con2.Close();
                                    }
                                }
                                */
                                orderList.SelectedIndex = 2;
                                customList.Visible = true;
                                customLbl.Visible = true;
                                customList.Items.Clear();
                                customList.Items.Add(new ListItem(String.Empty, String.Empty));

                                sql = "SELECT * FROM " + DBName + " ORDER BY customOrder";
                            }

                            else
                            {
                                customList.Visible = false;
                                customLbl.Visible = false;
                                if (orderType == "alpha")
                                {
                                    orderList.SelectedIndex = 0;
                                    sql = "SELECT * FROM " + DBName + " ORDER BY " + colName;
                                }

                                //otherwise order by date added aka id
                                else
                                {
                                    orderList.SelectedIndex = 1;
                                    sql = "SELECT * FROM " + DBName + " ORDER BY " + primKeys[i];
                                }
                            }
                        }

                        System.Diagnostics.Debug.WriteLine(sql);
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                //populate the dropdown
                                while (reader.Read())
                                {
                                    ListItem newItem = new ListItem(reader[colName].ToString());
                                    System.Diagnostics.Debug.WriteLine(newItem.Text);
                                    list.Items.Add(newItem);

                                    if (orderType == "custom")
                                    {
                                        customList.Items.Add(reader["customOrder"].ToString());
                                    }
                                }

                                list.SelectedIndex = selectedIndex;
                                customList.SelectedIndex = 0;
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("AT THE END");
                    }
                }

                catch (Exception err)
                {
                    lblMessage.Text = null;
                    //lblMessage.Text = "Cannot submit information now. Please try again later.";
                
            }
                finally
                {
                    con.Close();
                    //Session["message"] = "Changes successfully applied!";
                }

                //Index_Change(null, null);
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
                string valName = "value";

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

                List<TextBox> newEntries = new List<TextBox>();
                newEntries.Add(newField);
                newEntries.Add(newRole);
                newEntries.Add(newGrade);
                newEntries.Add(newResidential);
                newEntries.Add(newCost);
                newEntries.Add(newStipend);
                newEntries.Add(newDuration);
                newEntries.Add(newSeason);
                newEntries.Add(newArea);

                List<CheckBox> newEntriesActive = new List<CheckBox>();
                newEntriesActive.Add(newFieldActive);
                newEntriesActive.Add(newRoleActive);
                newEntriesActive.Add(newGradeActive);
                newEntriesActive.Add(newResidentialActive);
                newEntriesActive.Add(newCostActive);
                newEntriesActive.Add(newStipendActive);
                newEntriesActive.Add(newDurationActive);
                newEntriesActive.Add(newSeasonActive);
                newEntriesActive.Add(newAreaActive);

                int index = -1;

                if (senderID == "submitNewField")
                {
                    index = 0;
                }

                else if (senderID == "submitNewRole")
                {
                    index = 1;
                    valName = "roleName";
                }

                else if (senderID == "submitNewGrade")
                {
                    index = 2;
                }

                else if (senderID == "submitNewResidential")
                {
                    index = 3;
                }

                else if (senderID == "submitNewCost")
                {
                    index = 4;
                }

                else if (senderID == "submitNewStipend")
                {
                    index = 5;
                }

                else if (senderID == "submitNewDuration")
                {
                    index = 6;
                }

                else if (senderID == "submitNewSeason")
                {
                    index = 7;
                }

                else if (senderID == "submitNewArea")
                {
                    index = 8;
                }

                tableName = DBNames[index];
                value = newEntries[index].Text;
                newEntries[index].Text = string.Empty;
                if (newEntriesActive[index].Checked == false)
                {
                    isActive = "inactive";
                }

                bool customOrder = false;
                //check if we are using custom order
                string sql = "SELECT * FROM " + tableName;
                string order = string.Empty;
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order = reader["myOrder"].ToString();
                            if (reader["myOrder"].ToString() == "custom")
                            {
                                customOrder = true;
                            }
                            break;
                        }
                    }
                }

                int endOfList = -1;
                //if we are using custom ordering, need to find the highest custom order value to insert at the end
                if (customOrder)
                {
                    sql = "SELECT TOP 1 customOrder FROM " + tableName + " ORDER BY customOrder DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                endOfList = int.Parse(reader["customOrder"].ToString());
                                break;
                            }
                        }
                    }
                }
                
                sql = "INSERT INTO " + tableName + " (" + valName + ", status, myOrder, customOrder) values(@value, @isActive, @order, @custOrder);";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(new SqlParameter("@value", value));
                    cmd.Parameters.Add(new SqlParameter("@isActive", isActive));
                    cmd.Parameters.Add(new SqlParameter("@order", order));
                    if (customOrder)
                    {
                        cmd.Parameters.Add(new SqlParameter("@custOrder", endOfList + 1));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@custOrder", "NULL"));
                    }
                    cmd.ExecuteNonQuery();
                }
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

        protected void editManager (object sender, EventArgs e)
        {
            Button senderBtn = sender as Button;
            string senderID = senderBtn.ID;
            string email = String.Empty;
            string sql = String.Empty;

            if (senderID == "approveManagerBtn")
            {
                email = unapprovedManagers.SelectedValue.ToString();
                sql = "UPDATE ProgramManager SET approved = 'yes' WHERE email = @email";
            }

            else if (senderID == "disapproveManagerBtn")
            {
                email = approvedManagers.SelectedValue.ToString();
                sql = "UPDATE ProgramManager SET approved = 'no' WHERE email = @email";
            }

            else if (senderID == "makeInactiveManagerBtn")
            {
                email = activeManagers.SelectedValue.ToString();
                sql = "UPDATE ProgramManager SET status = 'inactive' WHERE email = @email";
            }

            else if (senderID == "makeActiveManagerBtn")
            {
                email = inactiveManagers.SelectedValue.ToString();
                sql = "UPDATE ProgramManager SET status = 'active' WHERE email = @email";
            }

            string cs = WebConfigurationManager.ConnectionStrings["localConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            //populate the dropdown lists 
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add(new SqlParameter("@email", email));
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            catch (Exception err)
            {
                Response.Write("<script>alert(\"" + err.Message + "\");</script>");
                Response.Write(err.Message);
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
                string newStatus = "inactive";
                string sql = String.Empty;

                //find which button called this function and set the table name appropriately
                Button clicked = sender as Button;
                string senderID = clicked.ID;
                string tableName = String.Empty;
                string primKey = String.Empty;

                //populate lists to build SQL queries
                List<string> tableNames = new List<string>();
                tableNames.Add("FieldOfStudy");
                tableNames.Add("ManagerRole");
                tableNames.Add("Grades");
                tableNames.Add("Residental");
                tableNames.Add("ProgramCost");
                tableNames.Add("Stipend");
                tableNames.Add("Duration");
                tableNames.Add("Season");
                tableNames.Add("ServiceArea");
                tableNames.Add("Admin");

                List<DropDownList> toEdit = new List<DropDownList>();
                toEdit.Add(fieldList);
                toEdit.Add(roleList);
                toEdit.Add(gradeList);
                toEdit.Add(residentialList);
                toEdit.Add(costList);
                toEdit.Add(stipendList);
                toEdit.Add(durationList);
                toEdit.Add(seasonList);
                toEdit.Add(areaList);

                List<TextBox> editValues = new List<TextBox>();
                editValues.Add(editProgField);
                editValues.Add(roleEdit);
                editValues.Add(gradeEdit);
                editValues.Add(residentialEdit);
                editValues.Add(costEdit);
                editValues.Add(stipendEdit);
                editValues.Add(durationEdit);
                editValues.Add(seasonEdit);
                editValues.Add(areaEdit);

                List<CheckBox> activeBoxes = new List<CheckBox>();
                activeBoxes.Add(fieldActive);
                activeBoxes.Add(roleActive);
                activeBoxes.Add(gradeActive);
                activeBoxes.Add(residentialActive);
                activeBoxes.Add(costActive);
                activeBoxes.Add(stipendActive);
                activeBoxes.Add(durationActive);
                activeBoxes.Add(seasonActive);
                activeBoxes.Add(areaActive);

                //used in custom ordering
                List<DropDownList> customOrderLists = new List<DropDownList>();
                customOrderLists.Add(fieldCustomOrder);
                customOrderLists.Add(managerCustomOrder);
                customOrderLists.Add(gradeCustomOrder);
                customOrderLists.Add(residentialCustomOrder);
                customOrderLists.Add(costCustomOrder);
                customOrderLists.Add(stipendCustomOrder);
                customOrderLists.Add(durationCustomOrder);
                customOrderLists.Add(seasonCustomOrder);
                customOrderLists.Add(areaCustomOrder);

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

                //as in the other methods, I have to use idName as not every table uses "value" to store its main value right now
                string idName = "value";
                int index = -1;

                //find which field is being edited by using sender button's ID
                //select the index to use for each list based on that
                if (senderID == "fieldEdit")
                {
                    index = 0;
                }

                else if (senderID == "roleEditBtn")
                {
                    index = 1;
                    idName = "roleName";
                }

                else if (senderID == "gradeEditBtn")
                {
                    index = 2;
                    System.Diagnostics.Debug.WriteLine("grade edit btn");
                }

                else if (senderID == "residentialEditBtn")
                {
                    index = 3;
                }

                else if (senderID == "costEditBtn")
                {
                    index = 4;
                }

                else if (senderID == "stipendEditBtn")
                {
                    index = 5;
                }

                else if (senderID == "durationEditBtn")
                {
                    index = 6;
                }

                else if (senderID == "seasonEditBtn")
                {
                    index = 7;
                }

                else if (senderID == "areaEditBtn")
                {
                    index = 8;
                }
                
                tableName = tableNames[index];
                value = toEdit[index].SelectedItem.Value;
                editValues[index].Text = string.Empty;
                primKey = primKeys[index];
                //newStatus is "inactive" by default
                if (activeBoxes[index].Checked)
                {
                    newStatus = "active";
                }
                
                //find if this field uses custom ordering
                sql = "SELECT myOrder FROM " + tableName;
                String orderType = String.Empty;
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //all "order" entries should be the same so only need to read one
                        while (reader.Read())
                        {
                            orderType = reader["myOrder"].ToString();
                            break;
                        }
                    }
                }

                //only put through changes if the empty option (selected by default) is not chosen
                if (orderType == "custom")
                {
                    DropDownList customOrderList = customOrderLists[index];
                    if (customOrderList.SelectedValue != String.Empty)
                    {
                        int oldCustomOrder = -1;
                        int newCustomOrder = int.Parse(customOrderList.SelectedValue);

                        //first get the current custom order of the selected entry
                        sql = "SELECT * FROM  " + tableName + " WHERE " + idName + " = '" + value + "'";
                        
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    oldCustomOrder = int.Parse(reader["customOrder"].ToString());
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
                                    int curID = int.Parse(reader[primKey].ToString());

                                    if (reader[idName].ToString() == value)
                                    {
                                        using (SqlConnection con2 = new SqlConnection(cs))
                                        {
                                            con2.Open();
                                            string sql2 = "UPDATE " + tableName + " SET customOrder = " + newCustomOrder + " WHERE " + primKey + " = " + curID + ";";

                                            System.Diagnostics.Debug.WriteLine(sql2);
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
                                            string sql2 = "UPDATE " + tableName + " SET customOrder = " + curCustomOrder + " WHERE " + primKey + " = " + curID + ";";

                                            System.Diagnostics.Debug.WriteLine(sql2);
                                            using (SqlCommand cmd2 = new SqlCommand(sql2, con2))
                                            {
                                                cmd2.ExecuteNonQuery();
                                            }
                                            con2.Close();
                                        }
                                    }
                                    populateData(false);
                                }
                            }
                        }
                    }
                }

                //update the entry
                if (value != String.Empty)
                {
                    //get the current "active" status of the selected value
                    using (SqlCommand cmd = new SqlCommand("SELECT status FROM " + tableName + " WHERE " + idName + "= @value;", con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@value", value));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                status = reader["status"].ToString();
                            }
                        }
                    }

                    if (status != String.Empty)
                    {
                        sql = "UPDATE " + tableName + " SET status= @status WHERE " + idName + "= @value;";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add(new SqlParameter("@value", value));
                            cmd.Parameters.Add(new SqlParameter("@status", newStatus));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }

                    //update field name then clear the text box
                    if (newValue != String.Empty)
                    {
                        sql = "UPDATE " + tableName + " SET " + idName + "= @newValue WHERE " + idName + "= @value;";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
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

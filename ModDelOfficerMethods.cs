using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class ModDelOfficerMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        public bool IsEmpty(TextBox[] textBoxes, ComboBox[] comboBoxes, RadioButton[] radioButtons)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.TextLength == 0)
                    return true;
            }

            foreach (ComboBox comboBox in comboBoxes)
            {
                if (comboBox.Text == "")
                {
                    return true;
                }
            }

            if (!radioButtons[0].Checked && !radioButtons[1].Checked && !radioButtons[2].Checked)
                return true;

            return false;
        }

        //Method to clear textboxes, comboboxes and radio buttons
        public void Clear(TextBox[] textBoxes, ComboBox[] comboBoxes, RadioButton[] radioButtons)
        {
            foreach (TextBox txt in textBoxes)
                txt.Clear();

            foreach (ComboBox cb in comboBoxes)
                cb.SelectedIndex = -1;

            foreach (RadioButton rb in radioButtons)
                rb.Checked = false;
        }

        //Load officer details into list view
        public void LoadOfficerDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)",150, HorizontalAlignment.Left);
            lv.Columns.Add("Position", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Contact", 70, HorizontalAlignment.Left);

            string strSQL = "SELECT * FROM Officer";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Officer");

            DataTable dataTable = dataSet.Tables["Officer"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["officerID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["surname"].ToString());
                    lvi.SubItems.Add(dataRow["otherNames"].ToString());
                    lvi.SubItems.Add(dataRow["position"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["contact"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        //Fill UI Controls
        public void FillUIControls(ListView lv, RadioButton[] radioButtons, TextBox txtSurname, TextBox txtOtherNames, TextBox txtContact, ComboBox cbPosition, ComboBox cbUnit, TextBox txtUsername, TextBox txtPassword, TextBox txtConfirmPassword)
        {
            string officerID = lv.SelectedItems[0].SubItems[0].Text;
            int intOfficerID = Int32.Parse(officerID);
            
            //Fetch username and password using officerID (primary key)
            string strSQL = "SELECT * from Officer where officerID = " + intOfficerID;
            SqlCommand cmd = con.cmd();

            cmd.CommandText = strSQL;

            SqlDataReader dataRead = cmd.ExecuteReader();

            while (dataRead.Read())
            {
                txtUsername.Text = dataRead["username"].ToString();
                txtPassword.Text = dataRead["psword"].ToString();
                txtConfirmPassword.Text = dataRead["psword"].ToString();
                txtContact.Text = dataRead["contact"].ToString();
                cbPosition.Text = dataRead["position"].ToString();
            }
                 
            dataRead.Close();
            con.connectionClose();

            txtSurname.Text = lv.SelectedItems[0].SubItems[1].Text;
            txtOtherNames.Text = lv.SelectedItems[0].SubItems[2].Text;
            //cbPosition.Text = lv.SelectedItems[0].SubItems[3].Text;
            //txtContact.Text = lv.SelectedItems[0].SubItems[5].Text;

            string unit = lv.SelectedItems[0].SubItems[4].Text;

            if (unit.Length > 5) //Sub-Unit is included
            {
                foreach (RadioButton rb in radioButtons)
                {
                    if (unit.Contains(rb.Text))
                        rb.Checked = true;
                }

                foreach (string item in cbUnit.Items)
                {
                    if (unit.Contains(item))
                        cbUnit.Text = item;
                }
            }

            else
            {
                foreach (RadioButton rb in radioButtons)
                {
                    if (rb.Text == unit)
                        rb.Checked = true;
                }
            }
        }
        
        //Modify Officer details
        public void ModifyCFOOfficer(string surname, string otherNames, string contact, string unit, string position, string username, string password, string officerID)
        {
            int intOfficerID = Int32.Parse(officerID);
            string strSurname = surname;
            string strOtherNames = otherNames;
            string strPosition = position;
            string strUsername = username;
            string strPassword = password;
            string strUnit = unit;
            string strContact = contact;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE ModifyCFOOfficer @surname, @otherNames, @position, @username, @psword, @unit, @contact, @officerID";

            cmd.Parameters.Add("@surname", SqlDbType.VarChar, 60).Value = strSurname;
            cmd.Parameters.Add("@otherNames", SqlDbType.VarChar, 60).Value = strOtherNames;
            cmd.Parameters.Add("@position", SqlDbType.VarChar, 50).Value = strPosition;
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
            cmd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPassword;
            cmd.Parameters.Add("@unit", SqlDbType.VarChar, 100).Value = strUnit;
            cmd.Parameters.Add("@contact", SqlDbType.VarChar, 10).Value = strContact;
            cmd.Parameters.Add("@officerID", SqlDbType.Int).Value = intOfficerID;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        public void ModifyOtherOfficer(string surname, string otherNames, string contact, string unit, string position, string officerID)
        {
            int intOfficerID = Int32.Parse(officerID);
            string strSurname = surname;
            string strOtherNames = otherNames;
            string strPosition = position;
            string strUnit = unit;
            string strContact = contact;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE ModifyOtherOfficer @surname, @otherNames, @position, @unit, @contact, @officerID";

            cmd.Parameters.Add("@surname", SqlDbType.VarChar, 60).Value = strSurname;
            cmd.Parameters.Add("@otherNames", SqlDbType.VarChar, 60).Value = strOtherNames;
            cmd.Parameters.Add("@position", SqlDbType.VarChar, 50).Value = strPosition;
            cmd.Parameters.Add("@unit", SqlDbType.VarChar, 100).Value = strUnit;
            cmd.Parameters.Add("@contact", SqlDbType.VarChar, 10).Value = strContact;
            cmd.Parameters.Add("@officerID", SqlDbType.Int).Value = intOfficerID;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }


        //Delete Officer
        public void DeleteOfficer(string officerID)
        {
            int intOfficerID = Int32.Parse(officerID);

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "EXECUTE DeleteOfficer @officerID";

            cmd.Parameters.Add("@officerID", SqlDbType.Int).Value = intOfficerID;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        //Load list view using search criterion
        public void SearchOfficerBySurname(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Officer where surname like '" +search+ "%'";

            lv.Items.Clear();
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Position", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Contact", 70, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Officer");

            DataTable dataTable = dataSet.Tables["Officer"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["officerID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["surname"].ToString());
                    lvi.SubItems.Add(dataRow["otherNames"].ToString());
                    lvi.SubItems.Add(dataRow["position"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["contact"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        //Load elements into combo box
        public void ComboBoxLoad(ComboBox cbo, string[] a)
        {
            cbo.Items.AddRange(a);
        }

        public void SearchOfficerByOtherNames(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Officer where otherNames like '" + search + "%'";

            lv.Items.Clear();
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Position", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Contact", 70, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Officer");

            DataTable dataTable = dataSet.Tables["Officer"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["officerID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["surname"].ToString());
                    lvi.SubItems.Add(dataRow["otherNames"].ToString());
                    lvi.SubItems.Add(dataRow["position"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["contact"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        public void SearchOfficerByPosition(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Officer where position like '" + search + "%'";

            lv.Items.Clear();
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Position", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Contact", 70, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Officer");

            DataTable dataTable = dataSet.Tables["Officer"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["officerID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["surname"].ToString());
                    lvi.SubItems.Add(dataRow["otherNames"].ToString());
                    lvi.SubItems.Add(dataRow["position"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["contact"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        public void SearchOfficerByUnit(ListView lv, TextBox txtSearchBox)
        {
            string search = txtSearchBox.Text;

            SqlCommand cmd = con.cmd();
            string strSQL = "SELECT * FROM Officer where unit like '%" + search + "%' or unit like '" + search +"%'";

            lv.Items.Clear();
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Position", 100, HorizontalAlignment.Left);
            lv.Columns.Add("Unit/Sub-Unit", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Contact", 70, HorizontalAlignment.Left);

            cmd.CommandText = strSQL;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Officer");

            DataTable dataTable = dataSet.Tables["Officer"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];

                if (dataRow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(dataRow["officerID"].ToString(), 0);
                    lvi.SubItems.Add(dataRow["surname"].ToString());
                    lvi.SubItems.Add(dataRow["otherNames"].ToString());
                    lvi.SubItems.Add(dataRow["position"].ToString());
                    lvi.SubItems.Add(dataRow["unit"].ToString());
                    lvi.SubItems.Add(dataRow["contact"].ToString());

                    lv.Items.Add(lvi);
                }
            }

        }

        public string MainUnit(RadioButton[] radioButtons)
        {
            string unit = "";
            foreach (RadioButton rb in radioButtons)
            {
                if (rb.Checked)
                    unit = rb.Text.ToUpper();
            }

            return unit;
        }

        //Method to generate unit of an officer
        public string Unit(string subUnit, RadioButton[] radioButtons)
        {
            string unit;

            if (subUnit.Length == 0)
                unit = MainUnit(radioButtons);
            else
            {
                unit = subUnit + ", " + MainUnit(radioButtons);
            }

            return unit;
        }
    }
}

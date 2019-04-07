using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class NewOfficerMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        //Generate ID for table
        public int FetchLastID(string tableName, string idColumn)
        {
            int lastID = 0;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select "+ idColumn +" from " + tableName;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, tableName);
            DataTable dTable = dataSet.Tables[tableName];

            DataRow dRow = dTable.Rows[dTable.Rows.Count - 1];
            lastID = Int32.Parse(dRow[idColumn].ToString());
            
            return lastID;
        }
        
        public int GenerateID(string tableName, string idColumn)
        {
            string strTableName = tableName;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select * from " + strTableName;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, strTableName);
            DataTable dTable = dataSet.Tables[strTableName];

            int intID;

            if (dTable.Rows.Count == 0)
                intID = dTable.Rows.Count + 1;

            else
                intID = FetchLastID(tableName, idColumn) + 1;

            return intID;
        }

        //Insert new Employee
        #region InsertOfficer

        public void InsertCFOOfficer(string surname, string otherNames, string username, string psword, string contact, string unit, string position)
        {
            int intID = GenerateID("Officer", "officerID");
            string strSurname = surname;
            string strOtherNames = otherNames;
            string strUsername = username;
            string strPsword = psword;
            string strUnit = unit;
            string strPosition = position;
            string strContact = contact;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE InsertCFOOfficer @officerID, @surname, @otherNames, @username, @psword, @unit, @position, @contact";

            cmd.Parameters.Add("@officerID", SqlDbType.Int).Value = intID;
            cmd.Parameters.Add("@surname", SqlDbType.VarChar, 60).Value = strSurname;
            cmd.Parameters.Add("@otherNames", SqlDbType.VarChar, 60).Value = strOtherNames;
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
            cmd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPsword;
            cmd.Parameters.Add("@unit", SqlDbType.VarChar, 100).Value = strUnit;
            cmd.Parameters.Add("@position", SqlDbType.VarChar, 50).Value = strPosition;
            cmd.Parameters.Add("@contact", SqlDbType.VarChar, 10).Value = strContact;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        public void InsertOtherOfficer(string surname, string otherNames, string contact, string unit, string position)
        {
            int intID = GenerateID("Officer", "officerID");
            string strSurname = surname;
            string strOtherNames = otherNames;
            string strUnit = unit;
            string strPosition = position;
            string strContact = contact;

            SqlCommand cmd = con.cmd();

            cmd.CommandText = "EXECUTE InsertOtherOfficer @officerID, @surname, @otherNames, @unit, @position, @contact";

            cmd.Parameters.Add("@officerID", SqlDbType.Int).Value = intID;
            cmd.Parameters.Add("@surname", SqlDbType.VarChar, 60).Value = strSurname;
            cmd.Parameters.Add("@otherNames", SqlDbType.VarChar, 60).Value = strOtherNames;
            cmd.Parameters.Add("@unit", SqlDbType.VarChar, 100).Value = strUnit;
            cmd.Parameters.Add("@position", SqlDbType.VarChar, 50).Value = strPosition;
            cmd.Parameters.Add("@contact", SqlDbType.VarChar, 10).Value = strContact;

            cmd.ExecuteNonQuery();
            con.connectionClose();
        }

        #endregion

        //Clear TextBoxes and ComboBoxes
        public void Clear(TextBox[] textBoxes, ComboBox[] comboBoxes, RadioButton[] radioButtons)
        {
            foreach (TextBox txt in textBoxes)
                txt.Clear();

            foreach (ComboBox cb in comboBoxes)
                cb.SelectedIndex = -1;

            foreach (RadioButton rb in radioButtons)
                rb.Checked = false;
        }

        //Check if textboxes and combobox is empty or not
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

        //Load elements into combo box
        public void ComboBoxLoad(ComboBox cb, string[] a)
        {
            cb.Items.Clear();
            cb.Items.AddRange(a);
        }

        //Fill listview with officers list
        public void LoadOfficerDetails(ListView lv)
        {
            lv.Clear();
            lv.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lv.Columns.Add("Surname", 150, HorizontalAlignment.Left);
            lv.Columns.Add("Other Name(s)", 150, HorizontalAlignment.Left);
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

        //Fetch the unit of an officer using the selected radio button
        public string MainUnit(RadioButton[] radioButtons)
        {
            string unit = "";
            foreach (RadioButton rb in radioButtons)
            {
                if (rb.Checked)
                    unit = rb.Text;
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

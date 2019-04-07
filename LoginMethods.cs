using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CFO_Requisition_System
{
    class LoginMethods
    {
        //Object of the connection class (A connection to the database)
        Connection con = new Connection();

        //Method to clear textboxes
        public void Clear(TextBox[] textBoxes)
        {
            foreach (TextBox txt in textBoxes)
            {
                txt.Clear();
            }
        }

        //Method to test if login credentials are correct
        public bool IsCorrect(string username, string password)
        {
            string strPassword = "";
            string strUsername = "";

            //Special login for Administrator
            if (password == "admin2017" && username == "admin")
                return true;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select psword,username from Officer";
            SqlDataReader dataRead = cmd.ExecuteReader();
            
            while (dataRead.Read())
            {
                strPassword = dataRead["psword"].ToString();
                strUsername = dataRead["username"].ToString();

                if (strPassword == password && strUsername == username)
                {
                    return true;
                }
            }

            dataRead.Close();
            cmd.ExecuteNonQuery();
            con.connectionClose();

            return false;
        }

        //Generate ID for table
        public int FetchLastID(string tableName, string idColumn)
        {
            int lastID = 0;

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select " + idColumn + " from " + tableName;

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

        //Method to insert into the log file when user logs in
        public void InsertLoginFile(string password, string username)
        {
            string strPassword = password;
            string strUsername = username;
            string strSurname = "";
            string strOtherNames = "";
            string strLogDate = DateTime.Now.ToString("f");
            string strLogTime = DateTime.Now.ToString("h:mm:ss tt zz");
            string strLogType = "LOGIN";

            //Generate ID for insertion
            int intLogID = GenerateID("LogFile", "logID");

            if (strPassword == "admin2017" && strUsername == "admin")
            {
                SqlCommand cmdd = con.cmd();

                //Form a string of the full officer's name
                string strOfficerName = "ADMINISTRATOR";

                //Populate the logfile in the database
                cmdd.CommandText = "EXECUTE InsertLog @logID, @officerName, @username, @psword, @logDate, @logTime, @logType";

                cmdd.Parameters.Add("@logID", SqlDbType.Int).Value = intLogID;
                cmdd.Parameters.Add("@officerName", SqlDbType.VarChar, 100).Value = strOfficerName;
                cmdd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
                cmdd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPassword;
                cmdd.Parameters.Add("@logDate", SqlDbType.Date).Value = strLogDate;
                cmdd.Parameters.Add("@logTime", SqlDbType.VarChar, 11).Value = strLogTime;
                cmdd.Parameters.Add("@logType", SqlDbType.VarChar, 10).Value = strLogType;

                cmdd.ExecuteNonQuery();
                con.connectionClose();
            }

            else
            {
                SqlCommand cmd = con.cmd();

                cmd.CommandText = "select * from Officer where username = '" + strUsername + "' and psword = '" + strPassword + "'";
                SqlDataReader dataRead = cmd.ExecuteReader();

                while (dataRead.Read())
                {
                    strSurname = dataRead["surname"].ToString();
                    strOtherNames = dataRead["otherNames"].ToString();
                }

                dataRead.Close();
                cmd.ExecuteNonQuery();
                con.connectionClose();

                SqlCommand cmdd = con.cmd();

                //Form a string of the full officer's name
                string strOfficerName = strSurname + " " + strOtherNames;

                //Populate the logfile in the database
                cmdd.CommandText = "EXECUTE InsertLog @logID, @officerName, @username, @psword, @logDate, @logTime, @logType";

                cmdd.Parameters.Add("@logID", SqlDbType.Int).Value = intLogID;
                cmdd.Parameters.Add("@officerName", SqlDbType.VarChar, 100).Value = strOfficerName;
                cmdd.Parameters.Add("@username", SqlDbType.VarChar, 12).Value = strUsername;
                cmdd.Parameters.Add("@psword", SqlDbType.VarChar, 12).Value = strPassword;
                cmdd.Parameters.Add("@logDate", SqlDbType.Date).Value = strLogDate;
                cmdd.Parameters.Add("@logTime", SqlDbType.VarChar, 11).Value = strLogTime;
                cmdd.Parameters.Add("@logType", SqlDbType.VarChar, 10).Value = strLogType;

                cmdd.ExecuteNonQuery();
                con.connectionClose();
            }
        }

        //Test if administrator exists already
        public bool AdministratorExists()
        {
            string strPosition = "";

            SqlCommand cmd = con.cmd();
            cmd.CommandText = "select position from Officer";

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Officer");
            DataTable dTable = dataSet.Tables["Officer"];
            

            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                DataRow dRow = dTable.Rows[i];
                strPosition = dRow["position"].ToString();

                if (strPosition == "ADMINISTRATOR")
                    return true;
            }

            return false;
        }

        //Method to insert Administrator's details
        public void InsertAdministrator(string surname, string otherNames, string username, string psword, string unit, string position, string contact)
        {
            //If there is no administrator already, then add a new one
            if (!AdministratorExists())
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
        }
    }
}

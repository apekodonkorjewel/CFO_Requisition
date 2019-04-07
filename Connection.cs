using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CFO_Requisition_System
{
    class Connection
    {
        private string strconn = ConfigurationManager.ConnectionStrings["CFO"].ToString();

        public SqlConnection connectionOpen()
        {
            SqlConnection connectionOpen = new SqlConnection(strconn);
            connectionOpen.Open();
            return connectionOpen;
        }

        public SqlCommand cmd()
        {
            SqlConnection connectionOpen = new SqlConnection(strconn);
            connectionOpen.Open();

            SqlCommand cmd = new SqlCommand();
            cmd = connectionOpen.CreateCommand();
            return cmd;
        }

        public SqlConnection connectionClose()
        {
            SqlConnection connectionClose = new SqlConnection(strconn);
            connectionClose.Close();
            return connectionClose;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Database
{
    public class DBUtils
    {
        SqlConnection m_cnn = null;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public DBUtils()
        {

        }

        /// <summary>
        /// Open a database connection with default database credentials
        /// </summary>
        public void DBOpen()
        {
            string connectionString = @"Data Source=DESKTOP-SRU018M;Initial Catalog=SWIFT;USER ID=user1;Password=U1234";

            if (m_cnn != null)
                return;

            m_cnn = new SqlConnection(connectionString);
            try
            {
                m_cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Database Open Failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Open a database with a given connection string
        /// </summary>
        /// <param name="connetionString"></param>
        public void DBOpen(string connectionString)
        {
            if (m_cnn != null)
                return;

            m_cnn = new SqlConnection(connectionString);
            try
            {
                m_cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Database Open Failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Open a database connection by supplying database credentials
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user_id"></param>
        /// <param name="password"></param>
        public void DBOpen(string server, string database, string user_id, string password)
        {
            string connectionString = @"Data Source=" + server + ";Initial Catalog=" + database + ";USER ID=" + user_id + ";Password=" + password;

            if (m_cnn != null)
                return;

            m_cnn = new SqlConnection(connectionString);
            try
            {
                m_cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Database Open Failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Close a database connection
        /// </summary>
        public void DBClose()
        {
            if (m_cnn != null)
            {
                m_cnn.Close();
                m_cnn.Dispose();
            }
        }

        public void DBBegin(string id)
        {
            try
            {
                DBOpen();
                SqlCommand command = new SqlCommand("BEGIN tran T" + id, m_cnn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to begin commit block.\n" + ex.Message);
            }
        }

        public void DBCommit(string id)
        {
            try
            {
                DBOpen();
                SqlCommand command = new SqlCommand("COMMIT tran T" + id, m_cnn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to end commit block.\n" + ex.Message);
            }
        }

        public void DBRollback(string id)
        {
            try
            {
                DBOpen();
                SqlCommand command = new SqlCommand("ROLLBACK tran T" + id, m_cnn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to rollback commit block.\n" + ex.Message);
            }
        }

        public Int64 getNewReferenceId(string sqlQuery, int startingId)
        {
            Int64 refId = -1;
            Int64 currentRefId = -1;

            try
            {
                DBOpen();
                SqlCommand command = new SqlCommand(sqlQuery, m_cnn);
                SqlDataReader reader = command.ExecuteReader();

                // while there is another record present
                while (reader.Read())
                {
                    if (reader[0].ToString() == "")
                        currentRefId = 1;
                    else
                        currentRefId = Convert.ToInt64(reader[0]);
                }
                reader.Close();

                if (startingId < currentRefId)
                    refId = currentRefId + 1;
                else
                    refId = startingId;

                return refId;
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to generate new Reference ID.\n" + ex.Message);
            }
        }

        public void saveMTRecord(string sqlQuery)
        {
            try
            {
                DBOpen();
                SqlCommand command = new SqlCommand(sqlQuery, m_cnn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failure to save MT Record.\n" + ex.Message);
            }
        }
    }
}

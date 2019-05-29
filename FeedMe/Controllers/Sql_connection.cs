using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMe.Controllers
{
    public class Sql_connection
    {
        string ConnectionString = "localhost:1433;Database=FeedMe;User=sa;Password=test1234;";
        
        SqlConnection con;

        public void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            cmd.ExecuteNonQuery();
        }


        public SqlDataReader DataReader(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        //Return a datatable - This can be converted to an List<> and parsed to a view
        public DataTable ReturnDataInDatatable(string Query_)
        {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            DataTable dt = new DataTable();
            dr.Fill(dt);
            return dt;
        }


        public object ShowDataInGridView(string Query_)
        {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            object dataum = ds.Tables[0];
            return dataum;
        }

        public void InsertOrUpdate(string Query_)
        {
            using (con = new SqlConnection(ConnectionString))
            {
                string sql = Query_;
                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    command.CommandType = CommandType.Text;
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void Delete(string Query_)
        {
            using (con = new SqlConnection(ConnectionString))
            {
                string sql = Query_;
                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    con.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                    }
                    con.Close();
                }
            }
        }



    }
}

using BaseArchitect.Utility;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace BaseArchitect.Entities
{
    public class DataProvider
    {
        private readonly string connectionString = "Data Source=DESKTOP-CB7KBVF;Initial Catalog=INDOOR_Education;Integrated Security=True";

        #region Utility

        protected string BuildExactEqual(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return null;
            return string.Format("'{0}'", keyword.Trim());
        }

        protected string BuildLikeFilter(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return null;
            return string.Format("%{0}%", keyword.Trim());
        }

        protected string BuildInCondition(List<int> lstValue, string defaultValue = "null")
        {
            if (lstValue.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                return string.Join(", ", lstValue.ToArray());
            }
        }

        protected string BuildInCondition(List<string> lstValue, string defaultValue = "null")
        {
            if (lstValue.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                string result = string.Empty;
                string separator = string.Empty;

                foreach (string item in lstValue)
                {
                    result += separator + "'" + item.Trim().Replace("'", "''") + "'";
                    separator = ",";
                }
                return result;
            }
        }

        #endregion

        #region Data Access Function       

        public List<T> ExecutePaging<T>(string query, string orderby, Entities.PagingInfo pagingInfo) where T : new()
        {            
            DataTable data = new DataTable();
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();
                int pageOffset = (pagingInfo.PageIndex - 1) * pagingInfo.PageSize;
                query += $" ORDER BY {orderby} OFFSET {pageOffset} ROWS FETCH NEXT {pagingInfo.PageSize} ROWS ONLY";
                SqlCommand command = new SqlCommand(query, sqlConnect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                sqlConnect.Close();
                return Converter.ConvertDataTableToList<T>(data);
            }
        }

        public List<T> ExecuteQuery<T>(string query, string orderBy = null) where T : new()
        {
            DataTable data = new DataTable();
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                sqlConnect.Open();
                if (orderBy != null) query += $" ORDER BY {orderBy}";
                SqlCommand command = new SqlCommand(query, sqlConnect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                sqlConnect.Close();

                return Converter.ConvertDataTableToList<T>(data);
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                int row = 0;
                sqlConnect.Open();
                SqlCommand command = new SqlCommand(query, sqlConnect);
                row = command.ExecuteNonQuery();
                sqlConnect.Close();
                return row;
            }
        }

        public object ExecuteScalar(string query)
        {
            using (SqlConnection sqlConnect = new SqlConnection(connectionString))
            {
                object data = 0;
                sqlConnect.Open();
                SqlCommand command = new SqlCommand(query, sqlConnect);
                data = command.ExecuteScalar();
                sqlConnect.Close();
                return data;
            }
        }

        #endregion

    }
}

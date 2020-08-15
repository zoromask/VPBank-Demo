using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectInfo;
namespace DataAccessLayer
{
    public class ItemDA
    {
        public static DataSet ItemLoad(int i_itemStatus, decimal i_begin, decimal i_end)
        {
            var ds = new DataSet();
            try
            {
                using (var conn = new SqlConnection(CommonSystem.gConnectStringSQL))
                {
                    var sqlComm = new SqlCommand("[dbo].[proc_ItemLoad]", conn);
                    sqlComm.Parameters.AddWithValue("@p_itemStatus", i_itemStatus);
                    sqlComm.Parameters.AddWithValue("@p_begin", (object)i_begin ?? DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@p_end", (object)i_end ?? DBNull.Value);
                  
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var da = new SqlDataAdapter { SelectCommand = sqlComm };
                    da.Fill(ds);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Ghi log
            }
            return ds;
        }
        public static DataSet ItemIdInCart()
        {
            var ds = new DataSet();
            try
            {
                using (var conn = new SqlConnection(CommonSystem.gConnectStringSQL))
                {
                    var sqlComm = new SqlCommand("[dbo].[proc_ItemInCartIdLst]", conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var da = new SqlDataAdapter { SelectCommand = sqlComm };
                    da.Fill(ds);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Ghi log
            }
            return ds;
        }
        public static int ItemUpdate(ItemInfo Obj)
        {
            var result = -1;
            try
            {
                using (var conn = new SqlConnection(CommonSystem.gConnectStringSQL))
                {
                    var sqlComm = new SqlCommand("[dbo].[proc_ItemInCart]", conn);
                    sqlComm.Parameters.AddWithValue("@p_Item_Id", Obj.Item_Id);
                    sqlComm.Parameters.AddWithValue("@p_IsInCart", Obj.IsInCart);
                    sqlComm.Parameters.AddWithValue("@p_return", SqlDbType.Decimal).Direction =
                        ParameterDirection.Output;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var da = new SqlDataAdapter();
                    da.InsertCommand = sqlComm;
                    da.InsertCommand.ExecuteNonQuery();
                    result = Convert.ToInt32(sqlComm.Parameters["@p_return"].Value);
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public static int ItemInCartCount()
        {
            var result = -1;
            try
            {
                using (var conn = new SqlConnection(CommonSystem.gConnectStringSQL))
                {
                    var sqlComm = new SqlCommand("[dbo].[proc_CountItemInCart]", conn);
                    sqlComm.Parameters.AddWithValue("@p_return", SqlDbType.Decimal).Direction =
                        ParameterDirection.Output;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var da = new SqlDataAdapter();
                    da.InsertCommand = sqlComm;
                    da.InsertCommand.ExecuteNonQuery();
                    result = Convert.ToInt32(sqlComm.Parameters["@p_return"].Value);
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}

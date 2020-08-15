using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CBO
    {
        public static ArrayList FillCollectionFromDataSet(DataSet ds, Type objType)
        {
            try
            {
                ArrayList objFillCollection = new ArrayList();
                object objFillObject = null;

                // get properties for type
                Hashtable objProperties = GetPropertyInfo_DataSet(objType);

                // get ordinal positions in datareader
                Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

                // iterate datareader

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // fill business object
                    objFillObject = CreateObjectFromDataSet(objType, dr, objProperties, arrOrdinals);
                    // add to collection
                    objFillCollection.Add(objFillObject);
                }

                return objFillCollection;
            }
            catch
            {
                return new ArrayList();
            }
        }
        #region Private
        /// <summary>
        /// Gán thứ tự cột trong dataset theo tên thuộc tính
        /// </summary>
        /// <param name="hashProperties"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Hashtable GetOrdinalsFromDataSet(Hashtable hashProperties, DataSet dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            if ((dt.Tables.Count > 0))
            {

                for (int i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    if (hashProperties.ContainsKey(dt.Tables[0].Columns[i].ColumnName.ToUpper()))
                        arrOrdinals[dt.Tables[0].Columns[i].ColumnName.ToUpper()] = i;
                }
            }
            return arrOrdinals;
        }
        
        private static Hashtable GetPropertyInfo_DataSet(Type objType)
        {
            Hashtable hashProperties = new Hashtable();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                hashProperties[objProperty.Name.ToUpper()] = objProperty;
            }
            return hashProperties;
        }
        private static object CreateObjectFromDataSet(Type objType, DataRow dr, Hashtable objProperties, Hashtable arrOrdinals)
        {
            try
            {
                object objObject = Activator.CreateInstance(objType);

                // fill object with values from datareader
                string _fieldname = "";
                int _possition = -1;
                foreach (DictionaryEntry de in arrOrdinals)
                {
                    _fieldname = de.Key.ToString();
                    _possition = (int)de.Value;

                    if (((PropertyInfo)objProperties[_fieldname]).CanWrite)
                    {
                        if (dr[_fieldname] == System.DBNull.Value)
                        {
                            // translate Null value
                            ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Null.SetNull((PropertyInfo)objProperties[_fieldname]), null);
                        }
                        else
                        {
                            try
                            {
                                Type pType = ((PropertyInfo)objProperties[_fieldname]).PropertyType;

                                #region
                                switch (pType.FullName)
                                {
                                    case "System.Enum":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, System.Enum.ToObject(pType, dr[_possition]), null);
                                        break;
                                    case "System.String":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (string)dr[_possition], null);
                                        break;
                                    case "System.Boolean":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (Boolean)dr[_possition], null);
                                        break;
                                    case "System.Decimal":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToDecimal(dr[_possition].ToString()), null);
                                        break;
                                    case "System.Int16":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt16(dr[_possition].ToString()), null);
                                        break;
                                    case "System.Int32":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt32(dr[_possition].ToString()), null);
                                        break;
                                    case "System.Int64":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ToInt64(dr[_possition].ToString()), null);
                                        break;
                                    case "System.DateTime":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (DateTime)dr[_possition], null);
                                        break;
                                    case "System.Double":
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, (Double)dr[_possition], null);
                                        break;
                                    default:
                                        // try explicit conversion
                                        ((PropertyInfo)objProperties[_fieldname]).SetValue(objObject, Convert.ChangeType(dr[_possition], pType), null);
                                        break;
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }

                return objObject;
            }
            catch (Exception)
            {

                throw;
            }


        }
        // sets a field to an application encoded null value ( used in Presentation layer )
        #endregion
    }
    public class CBO<T>
    {
        public static List<T> FillCollectionFromDataSet(DataSet ds)
        {
            if (ds.Tables.Count <= 0) return new List<T>();

            List<T> _list_T = new List<T>();

            // get properties for type
            Hashtable objProperties = GetPropertyInfo(typeof(T));

            // get ordinal positions in datareader
            Hashtable arrOrdinals = GetOrdinalsFromDataSet(objProperties, ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // fill business object
                T objFillObject = (T)CreateObjectFromDataSet(typeof(T), dr, objProperties, arrOrdinals);

                // add to collection
                _list_T.Add(objFillObject);
            }

            return _list_T;

        }
        #region private

        /// <summary>
        /// Tạo một đối tượng từ datarow
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="dr"></param>
        /// <param name="objProperties"></param>
        /// <param name="arrOrdinals"></param>
        /// <returns></returns>
        private static object CreateObjectFromDataSet(Type objType, DataRow dr, Hashtable objProperties, Hashtable arrOrdinals)
        {

            try
            {
                object objObject = Activator.CreateInstance(objType);

                string _fieldname = "";
                int _possition = -1;
                foreach (DictionaryEntry de in arrOrdinals)
                {
                    _fieldname = de.Key.ToString();
                    _possition = (int)arrOrdinals[_fieldname];
                    PropertyInfo _PropertyInfo = (PropertyInfo)objProperties[_fieldname];

                    if (_PropertyInfo.CanWrite)
                    {
                        if (_possition == -1 || dr[_possition] == System.DBNull.Value)
                        {
                            //LinhNN sua 16/12/2013: không cần vì mặc định Activator.CreateInstance khi tạo đã set các thuộc tính về null
                            //   _PropertyInfo.SetValue(objObject, Null.SetNull(_PropertyInfo), null);
                        }
                        else
                        {
                            #region
                            switch (_PropertyInfo.PropertyType.FullName)
                            {
                                case "System.Enum":
                                    _PropertyInfo.SetValue(objObject, System.Enum.ToObject(_PropertyInfo.PropertyType, dr[_possition]), null);
                                    break;
                                case "System.String":
                                    _PropertyInfo.SetValue(objObject, (string)dr[_possition], null);
                                    break;
                                case "System.Boolean":
                                    _PropertyInfo.SetValue(objObject, (Boolean)dr[_possition], null);
                                    break;
                                case "System.Decimal":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDecimal(dr[_possition]), null);
                                    break;
                                case "System.Int16":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt16(dr[_possition]), null);
                                    break;
                                case "System.Int32":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt32(dr[_possition]), null);
                                    break;
                                case "System.Int64":
                                    _PropertyInfo.SetValue(objObject, Convert.ToInt64(dr[_possition]), null);
                                    break;
                                case "System.DateTime":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDateTime(dr[_possition]), null);
                                    break;
                                case "System.Double":
                                    _PropertyInfo.SetValue(objObject, Convert.ToDouble(dr[_possition]), null);
                                    break;
                                default:
                                    // try explicit conversion
                                    _PropertyInfo.SetValue(objObject, Convert.ChangeType(dr[_possition], _PropertyInfo.PropertyType), null);
                                    break;
                            }
                            #endregion
                        }
                    }
                }

                return objObject;
            }
            catch(Exception ex)
            {
                return Activator.CreateInstance(objType);
            }
        }

        /// <summary>
        /// Lấy tên thuộc tính của 1 kiểu dữ liệu
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static Hashtable GetPropertyInfo(Type objType)
        {
            Hashtable hashProperties = new Hashtable();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                hashProperties[objProperty.Name.ToUpper()] = objProperty;
            }
            return hashProperties;
        }

        /// <summary>
        /// Gán thứ tự cột trong dataset theo tên thuộc tính
        /// </summary>
        /// <param name="hashProperties"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Hashtable GetOrdinalsFromDataSet(Hashtable hashProperties, DataSet dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            if ((dt.Tables.Count > 0))
            {

                for (int i = 0; i < dt.Tables[0].Columns.Count; i++)
                {
                    if (hashProperties.ContainsKey(dt.Tables[0].Columns[i].ColumnName.ToUpper()))
                        arrOrdinals[dt.Tables[0].Columns[i].ColumnName.ToUpper()] = i;
                }
            }
            return arrOrdinals;
        }

        private static Hashtable GetOrdinalsFromDataTable(Hashtable hashProperties, DataTable dt)
        {

            Hashtable arrOrdinals = new Hashtable();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (hashProperties.ContainsKey(dt.Columns[i].ColumnName.ToUpper()))
                    arrOrdinals[dt.Columns[i].ColumnName.ToUpper()] = i;
            }

            return arrOrdinals;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace BaseArchitect.Utility
{
    public static class Converter
    {
        private static CultureInfo provider = CultureInfo.InvariantCulture;

        public static T ConvertDataRowToRecord<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    //columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    columnname = columnsName.Find(name => name == objProperty.Name);
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }

        public static List<T> ConvertDataTableToList<T>(DataTable data) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in data.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = data.AsEnumerable().ToList().ConvertAll<T>(row => ConvertDataRowToRecord<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }

        public static string OrdinalSuffix(int number)
        {
            var suffix = "";

            if (number / 10 % 10 == 1)
            {
                suffix = "th";
            }
            else if (number > 0)
            {
                switch (number % 10)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }
            return suffix;
        }

        public static int Obj2Int(object objInput)
        {
            if (objInput == null) return 0;
            int retVal;
            if (int.TryParse(objInput.ToString(), NumberStyles.Integer, provider, out retVal))
                return retVal;
            else
                return 0;
        }

        public static int? Obj2IntNull(object objInput)
        {
            if (objInput == null || objInput.ToString() == string.Empty) return null;
            int retVal;
            if (int.TryParse(objInput.ToString(), NumberStyles.Integer, provider, out retVal))
                return retVal;
            return null;
        }

        public static decimal Obj2Decimal(object objInput)
        {
            if (objInput == null) return 0;
            decimal retVal;
            if (decimal.TryParse(objInput.ToString(), out retVal))
                return retVal;
            else
                return 0;
        }

        public static decimal? Obj2DecimalNull(object objInput)
        {
            if (objInput == null || objInput.ToString() == string.Empty) return null;
            decimal retVal;
            if (decimal.TryParse(objInput.ToString(), out retVal))
                return retVal;
            return null;
        }

        public static DateTime Obj2DateTime(object strDate)
        {
            if (strDate == null || strDate.ToString() == string.Empty)
                return DateTime.MinValue;
            try
            {
                return (DateTime)strDate;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime Obj2Date(object strDate, string dateformattype)
        {
            if (strDate == null || strDate.ToString() == string.Empty)
                return DateTime.MinValue;
            DateTime retVal = DateTime.Today;
            if (DateTime.TryParseExact(strDate.ToString(), dateformattype, provider, DateTimeStyles.None, out retVal))
                return retVal;
            return DateTime.MinValue;
        }

        public static DateTime? Obj2DateNull(object strDate, string dateformattype)
        {
            if (strDate == null || strDate.ToString() == string.Empty)
                return null;
            DateTime retVal = DateTime.Today;
            if (DateTime.TryParseExact(strDate.ToString(), dateformattype, provider, DateTimeStyles.None, out retVal))
                return retVal;
            return null;
        }
    }
}

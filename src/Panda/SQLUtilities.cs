using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Panda
{
    public static class SQLUtilities
    {
        public static string SqlMap(this Type self)
        {
            switch (self.Name.ToLower())
            {
                case "int":
                case "int32":
                    return "int";
                case "string": return "nvarchar(50)";
                default: return "nvarchar(50)";
            }
        }
        public static string CreateTableQuery(this Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("CREATE TABLE {0}(", type.Name));
            foreach (PropertyInfo prop in type.GetProperties())
            {
                string additional = "";
                Attributes.AUTO_INCREMENT au = (Attributes.AUTO_INCREMENT)prop.GetCustomAttribute<Attributes.AUTO_INCREMENT>();
                if (au != null)
                    additional = au.Value;       
                sb.Append(string.Format("{0} {1} {2},", prop.Name, prop.PropertyType.SqlMap(), additional));
            }
            sb.Remove(sb.Length-1, 1); // remove the last comma.
            sb.Append(string.Format(");"));

            return sb.ToString();
        }
    }
}

using Massive;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda
{
    public enum RequestType { GET, POST, UPDATE, PUT, DELETE }
    public class DataProcessRequest
    {
        public Type EntityType { get; set; }
        public RequestType Type { get; set; }
        public dynamic Object { get; set; }
    }

    public static class DataManager
    {
        public static string ConnectionString { get; set; }
        public static List<Type> RegisteredTypes { get; set; } = new List<Type>();

        public static void BuildTables()
        {
            // plan is to have adaptive diff and generate accordingly.
            var db = new DynamicModel(ConnectionString);
            StringBuilder sb = new StringBuilder(); // we can store the scheme for later use.
            foreach (Type entity in RegisteredTypes)
            {
                string query = entity.CreateTableQuery();

                // drop before create.
                query = string.Format("DROP TABLE {0}; {1}", entity.Name, query);

                sb.Append(query);
            }
            SqlCommand cmd = new SqlCommand(sb.ToString());
            db.Execute(cmd);
        }

        public static void ValidateTables()
        {
            throw new NotImplementedException(); // validate the scheme and if we need to diff and update db tables.
        }

        public static dynamic Process(DataProcessRequest request)
        {
            dynamic result = null;
            var tbl = new DynamicModel(ConnectionString, tableName: request.EntityType.Name, primaryKeyField: "Id");
            switch (request.Type)
            {
                case RequestType.GET:
                    if(request.Object != null)
                    {
                        throw new NotImplementedException();
                        //result = tbl.
                    } else
                    {
                        result = tbl.All();
                    }
                    break;
                case RequestType.POST:
                    // Id is not allowed when doing an insert as this is the auto generated key as IDENTITY(1,1)
                    IDictionary<string, object> map = ObjectExtensions.ToDictionary(request.Object);
                    map.Remove("Id");
                    result = tbl.Insert(map);
                    break;
                case RequestType.UPDATE:
                    result = tbl.Update(request.Object);
                    break;
                case RequestType.PUT:
                    throw new NotImplementedException();
                    break;
                case RequestType.DELETE:
                    throw new NotImplementedException();
                    break;
                default:
                    break;
            }
            return result;
        }
    }

}

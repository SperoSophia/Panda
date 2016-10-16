using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

using SperoSophia;
using System.Reflection;
using System.IO;

namespace Panda
{
    public class UsePanda
    {
        private readonly RequestDelegate _next;

        public UsePanda(RequestDelegate next)
        {
            _next = next;
        }

        public void ProcessRequest(HttpContext context)
        {
            var verb = context.Request.Method.ToUpper();
            var reqUri = context.Request.Path.Value;
            var baseUri = context.Request.PathBase.Value;
            var path = Regex.Replace(reqUri.Substring(baseUri.Length), @"^(.+)/$", "$1");
            if (path.StartsWith("/")) path = path.Substring(1, path.Length - 1); // path must start with /

            path = path.ToLower();
            string[] seg = path.Split('/');
            if (seg[0] != "api")
                return; // is not an api call

            string EntityName = seg[1];

            Type EntityType = DataManager.RegisteredTypes.FirstOrDefault(x => x.Name.ToLower() == EntityName);
            if (EntityType == null)
                return; // requested entity does not exist

            StreamReader sr = new StreamReader(context.Request.Body);
            object hasObj = Newtonsoft.Json.JsonConvert.DeserializeObject(sr.ReadToEnd(), EntityType); 
            DataProcessRequest req = new DataProcessRequest
            {
                Type = (RequestType)Enum.Parse(typeof(RequestType), verb, true),
                EntityType = EntityType,
                Object = hasObj
            };

            dynamic val = DataManager.Process(req);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(val);

            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = json.Length;
            context.Response.WriteAsync(json);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            ProcessRequest(httpContext);
            //return _next(httpContext);
        }
    }

    public static class UsePandaExtensions
    {
        public static IApplicationBuilder UsePanda(this IApplicationBuilder builder, string connectionString)
        {
            DataManager.ConnectionString = connectionString;
            Type[] types = Assembly.GetEntryAssembly().AllTypesOf(typeof(IPandaEntity));
            DataManager.RegisteredTypes.AddRange(types);
            DataManager.BuildTables();
            return builder.UseMiddleware<UsePanda>();
        }
    }
}

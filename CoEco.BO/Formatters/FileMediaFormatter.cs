using CoEco.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CoEco.BO.Formatters
{
    public class FileMediaFormatter : BufferedMediaTypeFormatter
    {
        public const string SupportedMediaType = "text/html";
        public FileMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (ExportableResolver.Instance.Value.CanConvert(type))
                return true;

            if (!type.IsGenericType)
                return false;
            var arguments = type.GetGenericArguments();
            if (arguments.Length != 1)
                return false;
            var ienumType = typeof(IEnumerable<>).MakeGenericType(arguments[0]);

            if (!ienumType.IsAssignableFrom(type))
                return false;

            return arguments[0].IsClass;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            IEnumerable<object> enumValue;
            Type genericType = type;
            if (ExportableResolver.Instance.Value.CanConvert(value.GetType()))
            {
                enumValue = ExportableResolver.Instance.Value.Convert(value);
                genericType = enumValue.GetType();
            }
            else
            {
                enumValue = (value as IEnumerable<object>);
                if (enumValue.Count() > 0)
                {
                    genericType = typeof(List<>).MakeGenericType(enumValue.First().GetType());
                }
                else
                {
                    // source of baseentity is no important because its just makes an empty file
                    genericType = typeof(List<>).MakeGenericType(new CoEco.Data.EntityTypes.BaseEntity().GetType());
                }
                var a = genericType.Name;
            }

            if (enumValue == null)
            {
                base.WriteToStream(type, value, writeStream, content);
                return;
            }
            var list = enumValue as object[] ?? enumValue.ToArray();

            var fTypeObj = HttpUtility.ParseQueryString(HttpContext.Current.Request.RawUrl).Get("fType");
            if (fTypeObj != null)
            {
                string fType = fTypeObj.ToString();
                if (fType == "excel")
                {
                    var dataTable = DependencyResolver.Current.GetService<IExportXslService>().ConvertToDataTable(list.ToList(), genericType.GetGenericArguments()[0]);

                    //if (genericType.GetGenericArguments()[0].Name == "PartnerEmploymentForm")
                    //{
                    //    for (int i = 0; i < list.Length; i++)
                    //    {
                    //        using (MofetEntities sysEnts = new MofetEntities())
                    //        {
                    //            string mesharetID = ((PartnerEmploymentForm)list[i]).MesharetID.ToString();
                    //            string v_spouseEmploymentID = ((PartnerEmploymentForm)list[i]).SpouseEmploymentId.ToString();
                    //            Meshartim mesharet = sysEnts.Meshartims.Where(x => x.ID == mesharetID).First();

                    //            try
                    //            {
                    //                mesharetList.Add(v_spouseEmploymentID + "," + mesharet.TZ.ToString());
                    //            }
                    //            catch
                    //            {
                    //                mesharetList.Add(v_spouseEmploymentID + "," + mesharet.TZ.ToString());
                    //            }

                    //        }
                    //    }
                    //}
                    DependencyResolver.Current.GetService<IExportXslService>().WriteDataTableToStream(dataTable, string.Format("{0}", genericType.GetGenericArguments()[0].Name), writeStream, string.Empty);
                }
            }
        }


        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            if (CanWriteType(type))
            {
                string typeName = ExportableResolver.Instance.Value.CanConvert(type)
                ? type.Name
                : type.GetGenericArguments()[0].Name;

                //if object (its for the complex type tbls) then give the assembly name
                typeName = typeName == "Object" ? System.Reflection.Assembly.GetExecutingAssembly().GetName().Name : typeName;
                headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = string.Format("{0}_{1:yyyyMMddHHmmss}.xlsx", typeName, DateTime.Now) };

                var fTypeObj = HttpUtility.ParseQueryString(HttpContext.Current.Request.RawUrl).Get("fType");
                if (fTypeObj != null)
                {
                    string fType = fTypeObj.ToString();

                    if (fType == "excel")
                    {
                        headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                        headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = string.Format("{0}_{1:yyyyMMddHHmmss}.xlsx", typeName, DateTime.Now) };
                    }
                    else if (fType == "text")
                    {
                        headers.ContentType = new MediaTypeHeaderValue("text/plain");
                        headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = string.Format("{0}_{1:yyyyMMddHHmmss}.txt", typeName, DateTime.Now) };
                    }
                    return;
                }
            }
            base.SetDefaultContentHeaders(type, headers, mediaType);
        }

    }
}
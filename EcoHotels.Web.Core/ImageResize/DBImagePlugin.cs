using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Issues;
using ImageResizer.Plugins;

namespace EcoHotels.Web.Core.ImageResize
{
    public class DBImagePlugin : VirtualPathProvider, IPlugin, IIssueProvider, IVirtualImageProvider
    {
        private string imageBlobQuery = "SELECT data FROM assets WHERE id=@id AND name=@name";
        private string modifiedDateQuery = "SELECT Modified, Created From assets WHERE id=@id AND name=@name";
        private string imageExistsQuery = "SELECT COUNT(id) From assets WHERE id=@id AND name=@name";

        public IPlugin Install(Config c)
        {
            Trace.WriteLine("DBImagePlugin - Installing");

            c.Plugins.add_plugin(this);
            return this;
        }

        public bool Uninstall(Config c)
        {
            c.Plugins.remove_plugin(this);
            return true; //True for successfull uninstallation, false if we couldn't do a clean job of it.
        }

        public IEnumerable<IIssue> GetIssues()
        {
            throw new NotImplementedException();
        }

        public override bool FileExists(string virtualPath)
        {
            if(!virtualPath.StartsWith("/img/"))
            {
                return false;
            }

            var settings = DBImageSettings.CreateSettings(virtualPath);
            return this.RowExists(settings.Id, settings.Filename);

            // validate path
            //if (true)
            //{
            //    return this.RowExists(settings.Id, settings.Filename);
            //}

            //return base.Previous.FileExists(virtualPath);
        }

        public bool FileExists(string virtualPath, NameValueCollection queryString)
        {
            if (!virtualPath.StartsWith("/img/"))
            {
                return false;
            }

            var settings = DBImageSettings.CreateSettings(virtualPath);
            return this.RowExists(settings.Id, settings.Filename);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            Trace.WriteLine("DBImagePlugin - GetFile");

            if (!virtualPath.StartsWith("/img/"))
            {
                return base.Previous.GetFile(virtualPath); ;
            }

            return new DBImage(virtualPath, this);
        }

        public IVirtualFile GetFile(string virtualPath, NameValueCollection queryString)
        {
            if (!virtualPath.StartsWith("/img/"))
            {
                return null;
            }

            return new DBImage(virtualPath, this);
        }

        public Stream GetStream(int id, string filename)
        {
            Trace.WriteLine("DBImagePlugin - GetStream"); 

            var connectionObj = GetConnectionObj();
            connectionObj.Open();
            Stream stream;

            using (connectionObj)
            {
                var sqlDataReader = new SqlCommand(imageBlobQuery, connectionObj)
                {
                    Parameters = 
			        {
				        CreateParameter("id", id, SqlDbType.Int),
				        CreateParameter("name", filename, SqlDbType.NVarChar)
			        }
                }.ExecuteReader();

                using (sqlDataReader)
                {
                    if (!sqlDataReader.Read())
                    {
                        throw new FileNotFoundException("Failed to find the specified image " + id + " in the database");
                    }
                    stream = sqlDataReader.GetSqlBytes(0).Stream;
                }
            }

            return stream;
        }

        public DateTime GetDateModifiedUtc(int id, string filename)
        {
            //this.FireAuthorizeEvent(id);
            SqlConnection connectionObj = this.GetConnectionObj();
            connectionObj.Open();
            DateTime result;

            using (connectionObj)
            {
                SqlDataReader sqlDataReader = new SqlCommand(this.modifiedDateQuery, connectionObj)
                {
                    Parameters = 
			        {
				        CreateParameter("id", id, SqlDbType.Int),
				        CreateParameter("name", filename, SqlDbType.NVarChar)
			        }
                }.ExecuteReader();

                using (sqlDataReader)
                {
                    if (!sqlDataReader.Read())
                    {
                        result = DateTime.MinValue;
                        return result;
                    }
                    for (var i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        if (!sqlDataReader.IsDBNull(i) && sqlDataReader.GetValue(i) is DateTime)
                        {
                            result = (DateTime)sqlDataReader.GetValue(i);
                            return result;
                        }
                    }
                }
            }

            return DateTime.MinValue;
        }

        public bool RowExists(int id, string filename)
        {
            var connectionObj = this.GetConnectionObj();
            connectionObj.Open();

            using (connectionObj)
            {
                var num = (int)new SqlCommand(imageExistsQuery, connectionObj)
                {
                    Parameters = 
			        {
				        CreateParameter("id", id, SqlDbType.Int),
				        CreateParameter("name", filename, SqlDbType.NVarChar)
			        }
                }.ExecuteScalar();

                if (num > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private SqlParameter CreateParameter(string name, object value, SqlDbType sqlDbType)
        {
            var parameter = new SqlParameter(name, value);
            parameter.SqlDbType = sqlDbType;
            return parameter;
        }  

        private SqlConnection GetConnectionObj()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        }
    }


}

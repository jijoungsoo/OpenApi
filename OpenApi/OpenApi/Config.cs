using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi
{
    class Config
    {
         static  readonly string ftpUser = "jijs";
         static readonly string ftpPwd = Environment.GetEnvironmentVariable("FTP_PASSWORD");
        //public static readonly string connStr = "Server=192.168.0.30;Database=stockWeb_development;Uid=root;Pwd=" + Environment.GetEnvironmentVariable("FTP_PASSWORD") + ";CHARSET=utf8;";
        public static readonly string connStr = "host=localhost;username=postgres;password=pwd;database=postgres";
        

        private static readonly string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+"\\";

        

        public static  String GetFtpUser()
        {
            return ftpUser;
        }

        public static String GetFtpPwd()
        {
            return ftpPwd;
        }

        public static String GetDbConnStr()
        {
            return connStr;
        }

        public static String GetPath()
        {
            return path;
        }

        public static String GetBackUpPath()
        {
            return path+"BACKUP\\";
        }
    }
}

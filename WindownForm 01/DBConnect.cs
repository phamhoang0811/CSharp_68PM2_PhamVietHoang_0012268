using Microsoft.Data.SqlClient;

namespace WindownForm_01
{
    internal class DBconnect
    {
        public static SqlConnection GetConnection()
        {
            string connStr =
            @"Server=LAPTOP-A207SSPP\SQLEXPRESS05;
            Database=QLSinhVienCSharp;
            User Id=hoang;
            Password=123456Aa@;
            TrustServerCertificate=True;";

            return new SqlConnection(connStr);
        }
    }
}
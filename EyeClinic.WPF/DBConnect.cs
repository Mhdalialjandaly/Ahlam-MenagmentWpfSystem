
using Microsoft.Data.SqlClient;



namespace Connecting
{
    class DBConnect
    {
        //string contents = File.ReadAllText(@"C:\temp\test.txt");
        private SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-88DG7LL;Initial Catalog=EyeClinic;User ID=sa;Password=asd1234");

        public SqlConnection GetCon()
        {
            return connection;
        }
        public void OpenCon()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void CloseCon()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}

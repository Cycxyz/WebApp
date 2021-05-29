using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApp
{
    public static class Creation
    {
        private static SqlParameter[] Fill(int[] value)
        {
            SqlParameter[] sqls = new SqlParameter[3];
            sqls[0] = new SqlParameter("@Place", value[0]);
            sqls[1] = new SqlParameter("@Price", value[1]);
            sqls[2] = new SqlParameter("@ConcertId", value[2]);
            return sqls;
        }
        public static void Init()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string constring = config.GetConnectionString("DefaultConnection");
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = constring;
                cn.Open();
                List<int> Concerts_ = new List<int>();
                SqlCommand cmd = new SqlCommand(@"Select * from Concerts", cn);
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())

                        Concerts_.Add((int)rd["ConcertId"]);
                }
                foreach (var id in Concerts_)
                {
                    SqlCommand cm = cn.CreateCommand();
                    cm.Parameters.Add(new SqlParameter("@id", id));
                    cm.CommandText = @"Select count(1) from Tickets where Tickets.ConcertId=@id";
                    int res = (int)cm.ExecuteScalar();
                    if (res == 0)
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            SqlCommand ins = new SqlCommand(@"Insert into Tickets values (null, @Place, @Price, @ConcertId)", cn);
                            ins.Parameters.AddRange(Fill(new int[] { i + 1, i < 150 ? 100 : 200, id }));
                            ins.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
  

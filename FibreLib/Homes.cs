using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace FibreLib
{
    public class Homes
    {
        List<Home> HomeList = new List<Home>();
        public int ListSize { get => HomeList.Count; }

        #region List Management
        public void addHome(string id, string address, string owner, string fibreProvider, bool isCovered, int speed, string isp)
        {
            HomeList.Add(new Home(id, address, owner, fibreProvider, isCovered, speed, isp));
        }

        public void updateHome(int index, Home newHome)
        {
            HomeList[index].ID = newHome.ID;
            HomeList[index].Address = newHome.Address;
            HomeList[index].Owner = newHome.Owner;
            HomeList[index].FibreProvider = newHome.FibreProvider;
            HomeList[index].Speed = newHome.Speed;
            HomeList[index].IsCovered = newHome.IsCovered;
            HomeList[index].Isp = newHome.Isp;
        }

        public List<string> idList()
        {
            List<string> output = new List<string>();

            for (int i = 0; i < ListSize; i++)
            {
                output.Add(HomeList[i].ID + " | " + HomeList[i].Owner);
            }

            return output;
        }

        public void delHome(Home temp)
        {
            int i = this.getIndex(temp);
            HomeList.RemoveAt(i);
        }

        public Home getHome(int homeIndex)
        {
            return HomeList[homeIndex];
        }

        public int getIndex(Home home)
        {
            for (int i = 0; i < ListSize; i++)
            {
                if (home.ID == HomeList[i].ID)
                {
                    return i;
                }
            }

            return -1;
        }

        public Home search(string ID)
        {
            Home toReturn = null;

            for (int i = 0; i < ListSize; i++)
            {
                Home temp = HomeList[i];
                if (temp.ID == ID)
                {
                    toReturn = temp;
                }
            }

            return toReturn;
        }
        #endregion

        #region Text Files Managment
        public string ExistsOrCreate(string fileName)
        {
            switch (File.Exists(fileName))
            {
                case true:
                    return "exists";
                case false:
                    using (FileStream fs = File.Create(fileName))
                    {
                        fs.Close();
                    }
                    return "created";
            }
        }

        public void toTextFile()
        {
            string txtFile = ExistsOrCreate("homes.txt");

            using (StreamWriter sw = new StreamWriter("homes.txt"))
            {
                foreach (Home h in HomeList)
                {
                    sw.WriteLine(h.csvString());
                }
            }
        }

        public void ReadText()
        {
            string txtFile = ExistsOrCreate("homes.txt");

            List<Home> newList = new List<Home>();
            using (StreamReader sr = new StreamReader("homes.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] x = line.Split(',');
                    newList.Add(new Home(x[0], x[1], x[2], x[3], Convert.ToBoolean(x[4]), Convert.ToInt32(x[5]), x[6]));
                }

                HomeList = newList;
            }
        }
        #endregion

        #region Database Managment

        public SqlConnection connectDB(string connString)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                return conn;
            }
            catch (Exception)
            {
                return null;
            }
        } //Works

        public void updateHome(SqlConnection conn, string TBL_Name, Home home)
        {
            string updateSql =
                $"UPDATE {TBL_Name} " +
                $"SET Address='{home.Address}', OwnerName='{home.Owner}', FibreProvider='{home.FibreProvider}', IsCovered='{home.IsCovered}', Speed={home.Speed}, ISP='{home.Isp}' " +
                $"WHERE ID='{home.ID}';";

            conn.Open();
            runSQL(updateSql, conn);
            conn.Close();
        }

        public void addHome(SqlConnection conn, string TBL_Name, Home home)
        {
            string insertSql =
                $"INSERT INTO {TBL_Name} " +
                $"VALUES(@ID, @Address, @OwnerName, @FibreProvider, @IsCovered, @Speed, @ISP);";

            SqlCommand comm = new SqlCommand(insertSql, conn);
            comm.Parameters.AddWithValue("@ID", home.ID);
            comm.Parameters.AddWithValue("@Address", home.Address);
            comm.Parameters.AddWithValue("@OwnerName", home.Owner);
            comm.Parameters.AddWithValue("@FibreProvider", home.FibreProvider);
            comm.Parameters.AddWithValue("@IsCovered", home.IsCovered);
            comm.Parameters.AddWithValue("@Speed", home.Speed);
            comm.Parameters.AddWithValue("@ISP", home.Isp);

            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public void dbToList(SqlConnection conn, string TBL_Name)
        {
            HomeList = new List<Home>();

            conn.Open();

            string sql = "SELECT *" +
                $"FROM {TBL_Name};";

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string address = reader.GetString(1);
                        string owner = reader.GetString(2);
                        string fibreProv = reader.GetString(3);
                        bool isCovered = reader.GetBoolean(4);
                        int speed = reader.GetInt32(5);
                        string isp = reader.GetString(6);

                        addHome(id, address, owner, fibreProv, isCovered, speed, isp);
                    }
                }
            }

            conn.Close();
        } //Works

        public void dbToList(SqlConnection conn, string TBL_Name, string SQL)
        {
            HomeList = new List<Home>();

            conn.Open();

            string sql = SQL;

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string address = reader.GetString(1);
                        string owner = reader.GetString(2);
                        string fibreProv = reader.GetString(3);
                        bool isCovered = reader.GetBoolean(4);
                        int speed = reader.GetInt32(5);
                        string isp = reader.GetString(6);

                        addHome(id, address, owner, fibreProv, isCovered, speed, isp);
                    }
                }
            }

            conn.Close();
        } //Works

        public void listToDB(SqlConnection conn, string TBL_Name)
        {
            conn.Open();

            //Drop DB before rewriting
            string dropSql = $"drop table {TBL_Name};";
            runSQL(dropSql, conn);

            //Create Tabel after dropping it
            string createSql = $"create table {TBL_Name}(" +
                "[ID] varchar(13) NOT NULL PRIMARY KEY," +
                "[Address] varchar(255)," +
                "[OwnerName] varchar(255)," +
                "[FibreProvider] varchar(255)," +
                "[IsCovered] bit," +
                "[Speed] int," +
                "[ISP] varchar(255)" +
                ");";
            runSQL(createSql, conn);

            string insertSql = $"INSERT INTO {TBL_Name} VALUES(@ID, @Address, @OwnerName, @FibreProvider, @IsCovered, @Speed, @ISP);";

            for (int i = 0; i < HomeList.Count; i++)
            {
                SqlCommand com = new SqlCommand(insertSql, conn);
                //Load Record with Fields
                com.Parameters.AddWithValue("@ID", HomeList[i].ID);
                com.Parameters.AddWithValue("@Address", HomeList[i].Address);
                com.Parameters.AddWithValue("@OwnerName", HomeList[i].Owner);
                com.Parameters.AddWithValue("@FibreProvider", HomeList[i].FibreProvider);
                com.Parameters.AddWithValue("@IsCovered", HomeList[i].IsCovered);
                com.Parameters.AddWithValue("@Speed", HomeList[i].Speed);
                com.Parameters.AddWithValue("@ISP", HomeList[i].Isp);

                //Load Record into DB (Execute INSERT SQL)
                com.ExecuteNonQuery();
            }

            conn.Close();
        } //Works

        public void runSQL(string SQL, SqlConnection conn)
        {
            SqlCommand com = new SqlCommand(SQL, conn);
            com.ExecuteNonQuery();
        } //Works

        #endregion

        #region Field Validation
        public bool validString(string test)
        {
            bool isNullOrEmpty = String.IsNullOrWhiteSpace(test);

            if (!isNullOrEmpty)
            {
                string strTest = test;
                return true;
            }
            return false;
        }

        public bool validID(dynamic test)
        {
            string strTest = test;
            if (strTest.Length == 13)
            {
                for (int i = 0; i < strTest.Length; i++)
                {
                    try
                    {
                        int x = Convert.ToInt32(strTest[i]);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool validAddress(dynamic test)
        {
            return validString(test);
        }

        public bool validName(dynamic test)
        {
            return validString(test);
        }

        public bool validProvider(dynamic test)
        {
            return validString(test);
        }

        public bool validBool(dynamic test)
        {
            try
            {
                bool bTest = Convert.ToBoolean(test);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool validSpeed(dynamic test)
        {
            try
            {
                int iTest = Convert.ToInt32(test);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool validIsp(dynamic test)
        {
            return validString(test);
        }
        #endregion
    }
}
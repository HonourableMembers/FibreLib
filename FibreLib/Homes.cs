using System;
using System.Collections.Generic;
using System.IO;

namespace FibreLib
{
    public class Homes
    {
        List<Home> HomeList = new List<Home>();
        public int ListSize { get => HomeList.Count; }

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

        public void toTextFile()
        {
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

        #region Field Validation
        public bool validString(string test)
        {
            bool notNull = (test != null);
            bool notEmpty = (test != "");

            if (notEmpty && notNull)
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
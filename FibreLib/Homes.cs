using System;
using System.Collections.Generic;
using System.IO;

namespace FibreLib
{
    public class Homes
    {
        List<Home> HomeList = new List<Home>();
        public int ListSize { get => HomeList.Count; }
        private int searchIndex = 0;

        public void addHome(string id, string address, string owner, string fibreProvider, bool isCovered, int speed, string isp)
        {
            HomeList.Add(new Home(id, address, owner, fibreProvider, isCovered, speed, isp));
        }

        public void updateHome(int index, Home newHome)
        {
            HomeList[index] = newHome;
        }

        public List<string> stringList()
        {
            List<string> output = new List<string>();

            for (int i = 0; i < ListSize; i++)
            {
                output.Add(HomeList[i].ID);
            }

            return output;
        }

        public void delHome(Home temp)
        {
            Home find = search(temp.ID);
            HomeList.RemoveAt(searchIndex);
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

        public Home getHome(int index)
        {
            return HomeList[index];
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SQLite;
using System.IO;

namespace SampleAPI.Controllers
{
    public class rawdataController : ApiController
    {
        //Random Value function
        private readonly Random randomValue = new Random();


        // To submit the data based on the score
        [HttpPost]
        public bool SubmitData(string question, int score)
        {
            if (!File.Exists("database.sqlite"))
            {
                SQLiteConnection.CreateFile("database.sqlite");
            }
            using (SQLiteConnection con = new SQLiteConnection("Data Source= database.sqlite; Version = 3; New = True; Compress = True; "))
            {
                con.Open();

                var cmd = new SQLiteCommand(con);
                cmd.CommandText = "Drop Table if Exists rawdata";
                cmd.ExecuteScalar();

                cmd.CommandText = "CREATE TABLE rawdata(ID INTEGER PRIMARY KEY,Question varchar(255), score int)";
                cmd.ExecuteNonQuery();

                for (int i = 1; i <= 10; i++)
                {
                    score = randomNumber(1, 5);
                    string strQ = @"'" + question +"_"+ score + "'";
                    cmd.CommandText = "insert into rawdata(Question,score) Values (" + strQ + "," + score + ")";
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }


        // To pull the random number using min and max value
        public int randomNumber(int minVal, int maxValue)
        {
            return randomValue.Next(1, 5);
        }


        // To read the data from datatable

        [HttpGet]
        public string GetData()
        {
            decimal count1 = 0;
            decimal count2 = 0;
            decimal count3 = 0;
            decimal count4 = 0;
            decimal count5 = 0;

            float finalResult = 0;

            using (SQLiteConnection con = new SQLiteConnection("Data Source= database.sqlite; Version = 3; New = True; Compress = True; "))
            {
                con.Open();

                string stm = "SELECT * FROM rawdata";

                var cmd = new SQLiteCommand(stm, con);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    int currentValue = Convert.ToInt32(rdr["Score"].ToString());

                    switch (currentValue)
                    {
                        case 1:
                            count1 = count1 + 1;
                            break;
                        case 2:
                            count2 = count2 + 1;
                            break;
                        case 3:
                            count3 = count3 + 1;
                            break;
                        case 4:
                            count4 = count4 + 1;
                            break;
                        case 5:
                            count5 = count5 + 1;
                            break;
                    }
                }

                finalResult = (float)Convert.ToDouble(((count4 + count5) / (count1 + count2 + count3 + count4 + count5)) * 100);
            }
            return finalResult.ToString();
        }

        [HttpPost]
        public bool DropTable()
        {
            return true;
        }
    }
}

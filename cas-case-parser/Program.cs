using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;

namespace cas_case_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to being parsing file");
            Console.ReadKey();
            Queue claimsQueue = readFile();
            createDatabaseRecords(claimsQueue);
        }

        /// <summary>
        /// Read the csv file and create queue of claims
        /// </summary>
        /// <returns>Queue of claims</returns>
        static Queue readFile()
        {
            Queue claimsQueue = new Queue();
            const string fileLocation = @"C:\Users\Joel\Desktop\claims.csv";
            StreamReader fileReader = new StreamReader(fileLocation);
            //Throw out the first line containing the headers of the table
            fileReader.ReadLine();
            do
            {
                //Read line from the database and convert to string array
                string[] recordContents = fileReader.ReadLine().Split(',');
                //Instantiate claim and add to queue
                claimsQueue.Enqueue(new Claim(recordContents));
            } while (fileReader.Peek() != -1);
            return claimsQueue;
        }

        /// <summary>
        /// Create batches and add to the database
        /// </summary>
        /// <param name="claimsQueue">queue of claims to add to the database</param>
        static void createDatabaseRecords(Queue claimsQueue)
        {
            MySqlConnectionStringBuilder connStringBuilder = new MySqlConnectionStringBuilder
            {
                //Removed db connection
            };
            //Open database connection
            using (MySqlConnection dbConn = new MySqlConnection(connStringBuilder.ToString()))
            {
                dbConn.Open();
                do
                {
                    //Create the prepared statement command
                    using (MySqlCommand cmd = dbConn.CreateCommand())
                    {
                        //Add the records to the database 1000 at a time
                        for (int i = 0; i < 1000 && claimsQueue.Count != 0; i++)
                        {
                            //Get current claim from the queue
                            Claim thisClaim = (Claim)claimsQueue.Dequeue();
                            //create prepared statement
                            cmd.CommandText += string.Format("INSERT INTO claims (phoneType, phoneCase, yearsOld, userAge, areaRisk, claimValue) VALUES (@type{0}, @case{0}, @yro{0}, @usra{0}, @arearsk{0}, @claimval{0}); ", i);
                            cmd.Parameters.AddWithValue("@type" + i, thisClaim.getTypeId());
                            cmd.Parameters.AddWithValue("@case" + i, thisClaim.getHasCase());
                            cmd.Parameters.AddWithValue("@yro" + i, thisClaim.getYearsOld());
                            cmd.Parameters.AddWithValue("@usra" + i, thisClaim.getUserAge());
                            cmd.Parameters.AddWithValue("@arearsk" + i, thisClaim.getRiskId());
                            cmd.Parameters.AddWithValue("@claimval" + i, thisClaim.getClaimValue());
                        }
                        Console.WriteLine("Doing insert query");
                        cmd.ExecuteNonQuery();
                    }
                } while (claimsQueue.Count > 0);
            }
        }
    }
}

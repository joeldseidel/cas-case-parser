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
        }

        static Queue readFile()
        {
            Queue claimsQueue = new Queue();
            const string fileLocation = @"C:\Users\Joel\Desktop\claims.csv";
            StreamReader fileReader = new StreamReader(fileLocation);
            //Throw out the first line containing the headers of the table
            fileReader.ReadLine();
            do
            {
                string[] recordContents = fileReader.ReadLine().Split(',');
                claimsQueue.Enqueue(new Claim(recordContents));
            } while (fileReader.Peek() != -1);
            return claimsQueue;
        }
    }
}

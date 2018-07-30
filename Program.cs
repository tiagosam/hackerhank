using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Solution {

    /*
     * Retrieve virus indexes.
     */
    static void VirusIndices(string p, string v) {
        /*
         * Print the answer for this test case in a single line
         */

        // Get patient sequences of DNA to compare.
        Dictionary<int, string> sequences = GetPatientSequences(v.Length, p);

        // Scan the virus in all the sequences. 
        int[] indexes = Scan(v, sequences);

        // Print the indexes
        if(indexes.Length == 0)
            Console.WriteLine("No Match!");
        else
            Console.WriteLine(string.Join(" ", indexes));
    }

    /*
     * Method for scan the virus.
     */
    static int[] Scan(string v, Dictionary<int, string> sequences)
    {
        List<int> indexes = new List<int>();

        foreach(KeyValuePair<int, string> sequence in sequences)
        {
            int i = 0;
            int error = 0; 

            foreach(char c in sequence.Value.ToCharArray())
            {
                if(c != v[i++])
                    error += 1;
            }

            if(error <= 1)
                indexes.Add(sequence.Key);
        }

        return indexes.ToArray();        
    }

    /*
     *  Method for retrieve sequences of DNA to compare and related index position. 
     */
    static Dictionary<int, string> GetPatientSequences(int len, string p)
    {   
        Dictionary<int, String> sequences = new Dictionary<int, String>();
        
        for(int i=0; i + len < p.Length + 1; i++)
        {
            sequences.Add(i, p.Substring(i, len));
            Log(String.Format("Patient sequence [{0}] with value [{1}] ", 
                                sequences.Count,
                                sequences.Last()), false);
        }

        return sequences;
    }

    /*
     * Method for log in a friendly way the information of the program. 
     */
    static void Log(string message, bool isError)
    {
        string outputMessage = String.Format("{0} {1} - {2}",
                                                isError ? "[ERROR]" : "[INFO]",
                                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), 
                                                message);
        
        // Will be commented. 
        Console.WriteLine(outputMessage);
    }

    /*
     * Check input constraints. 
     */
    static bool ValidateInput(string p, string v)
    {
        bool result = true;

        if(p.Trim().Length == 0)
        { result = false; Log("p length must be >= 1", true); }

        if(v.Trim().Length > (10 ^ 5))
        { result = false; Log("v length has more than 100.000 characters", true); }

        if(!ValidateRegex(p))
        { result = false; Log("p chars must be in range [a-z]", true); }

        if(!ValidateRegex(v))
        { result = false; Log("v chars must be in range [a-z]", true); }

        return result;
    }

    /*
     * Check if values are in the range [a-z]. 
     */
    static bool ValidateRegex(string value)
    {
        return Regex.IsMatch(value, @"^[a-z]+$");
    }

    /*
     * Main method
     */
    static void Main(string[] args) 
    {
        Log("Insert total number of testcases (t):", false);
        
        string line = Console.ReadLine(); uint t;

        if (!uint.TryParse(line, out t)) 
        { 
            Log("Total value must be uint.", false);  
            return;
        }
        
        if (!(t > 0 && t <=10)) 
        { 
            Log("Total number of test cases (t) must be [1..10]", true); 
            return; 
        }

        for (int tItr = 0; tItr < t; tItr++) {

            Log("Insert p and v values", false);
            string[] pv = Console.ReadLine().Split(' ');

            if(pv.Length != 2)
                Log ("Invalid p or v submitted.", true);
            else
            {
                string p = pv[0].Trim().ToLower();
                string v = pv[1].Trim().ToLower();

                if(ValidateInput(p, v))
                    VirusIndices(p, v);
                else   
                    Log("Validation of p and v fail.", true);
            }
        }
    }
}

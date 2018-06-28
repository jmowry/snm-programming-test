using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace snm_programming_test
{
    class Program
    {
        static void Main(string[] args)
        {
            if( args.Any() )
            {
                string path = args[0];

                if( File.Exists( path ) )
                {
                    InputFileHandler inputFileHandler = new InputFileHandler();
                    inputFileHandler.ReadEmployees( path );
                    inputFileHandler.SortEmployeesByGrossPayAscending();
                    // TODO: Have this print method call the SortEmployeesByGrossPayAscending
                    //      method, since they are tightly coupled.
                    inputFileHandler.PrintEmployeePaycheckData();
                    inputFileHandler.PrintTopEarners();
                }
                else
                {
                    Console.WriteLine( "File provided not found." );
                }
            }
            else
            {
                Console.WriteLine( "No input file was provided." );
            }
        }
    }
}

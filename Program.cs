using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace snm_programming_test
{
    class Program
    {
        static DateTime todayDate = DateTime.Now;

        static void Main(string[] args)
        {
            if( args.Any() )
            {
                string path = args[0];

                if( File.Exists( path ) )
                {
                    InputFileHandler inputFileHandler = new InputFileHandler();
                    inputFileHandler.ReadEmployees( path );
                    
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace snm_programming_test
{
    class Employee
    {
        private string employeeId;
        public string firstName;
        public string lastName;
        public char payType;
        public double salary;
        // TODO: This could be DateTime with some data sanitizing.
        public string startDate;
        public string state;
        public int hoursPerPeriod;

        public string EmployeeId { get => employeeId; set => employeeId = value; }
    }    

    class InputFileHandler
    {
        List<Employee> employeeList = new List<Employee>();

        public void ReadEmployees( string inputFileName )
        {
            string[] inputText = File.ReadAllLines( inputFileName );

            foreach( string line in inputText )
            {
                string[] column = line.Split( new char[] {','} );
                StoreEmployee( line );
                PrintEmployeeData( employeeList.Last() );
                
                if( employeeList.Count > 10 )
                {
                    break;
                }
            }
        }

        public void StoreEmployee( string employeeData )
        {
            string[] data = employeeData.Split( new char[] {','} );
            Employee employee = new Employee();

            employee.EmployeeId = data[0];
            employee.firstName = data[1];
            employee.lastName = data[2];
            employee.payType = Char.Parse( data[3] );
            employee.salary = Double.Parse( data[4] );
            employee.startDate = data[5];
            employee.state = data[6];
            employee.hoursPerPeriod = Int32.Parse( data[7] );
            employeeList.Add( employee );
        }

        public void PrintEmployeeData( Employee employee )
        {
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}",
                employee.EmployeeId, employee.firstName,
                employee.lastName,
                employee.payType,
                employee.salary,
                employee.startDate,
                employee.state,
                employee.hoursPerPeriod);
        }
    }
}
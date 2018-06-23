using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public Nullable<DateTime> startDate;
        public string state;
        public int hoursPerPeriod;
        public double grossPay;

        public string EmployeeId { get => employeeId; set => employeeId = value; }
    }    

    class InputFileHandler
    {
        static DateTime todayDate = DateTime.Today;
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

        private void StoreEmployee( string employeeData )
        {
            string[] data = employeeData.Split( new char[] {','} );
            Employee employee = new Employee();
            string dateString = data[5];
            CultureInfo provider = CultureInfo.InvariantCulture;
            string[] formats= {
                "M/d/yy",
                "M/dd/yy",
                "MM/d/yy",
                "MM/dd/yy" };
                    
            DateTime result;

            if( DateTime.TryParseExact( dateString, formats,
                                        provider,
                                        DateTimeStyles.None,
                                        out result ))
                                        {
                                            employee.startDate = result;
                                        }
            else
            {
                Console.WriteLine( "{0} is not in the correct format.", dateString );
                employee.startDate = null;
            }

            employee.EmployeeId = data[0];
            employee.firstName = data[1];
            employee.lastName = data[2];
            employee.payType = Char.Parse( data[3] );
            employee.salary = Double.Parse( data[4] );
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
                employee.hoursPerPeriod );
        }

        private void CalculatePaychecks( List<Employee> employeeList )
        {
            foreach( Employee employee in employeeList )
            {
                string state = employee.state;
                double stateTax = 0;
                double fedTax = 0.15;

                if( state == "UT" || state == "WY" || state == "NV" )
                {
                    stateTax = .05;
                }
                else if( state == "CO" || state == "ID" || state == "AZ" || state == "OR" )
                {
                    stateTax = .65;
                }
                else if( state == "WA" || state == "NM" || state == "TX" )
                {
                    stateTax = .07;
                }
                
                if( employee.payType == 'H' )
                {
                    if( employee.startDate == null )
                    {
                        
                    }
                }
                else
                {
                    employee.grossPay = employee.salary;
                }

            }

        }

        // TODO: This method can be refactored to be more concise 
        //      and more clean in the future.
        private double CalculateHourlyPay( Employee employee )
        {
            // TODO: Convert this to try...catch later.
            if( employee.startDate == null )
            {
                Console.WriteLine( "Could not calculate gross pay for {0} {1}.", employee.firstName, employee.lastName );
                Console.WriteLine( "Start date not found." );
                return 0;
            }

            int hours = employee.hoursPerPeriod;
            double rate = employee.salary;
            double overtimePay = 0;

            // TODO: This logic sequence can be simplified with a bit more thought.
            if( hours > 80 )
            {
                double totalPay = ( hours * rate );
                hours =- 80;

                if( hours <= 10 )
                {
                    overtimePay = ( hours * ( 1.5 * rate ) );
                }
                else if( hours > 10 )
                {
                    overtimePay = ( hours * ( 1.5 * rate ) );
                    hours =- 10;

                    overtimePay += ( hours * ( 1.75 * rate ) );
                }

                return totalPay + overtimePay;
            }
            else
            {
                return ( hours * rate );
            }
        }

        private double CalculateSalariedPay( Employee employee )
        {
            if( employee.startDate == null )
            {
                Console.WriteLine( "Could not calculate gross pay for {0} {1}.", employee.firstName, employee.lastName );
                Console.WriteLine( "Start date not found." );
                return 0;
            }

            // Get total time worked, from start to today.
            TimeSpan ts = employee.startDate.Value.Subtract( todayDate );
            int dateDiff = ts.Days;
            int weeks = (int)dateDiff / 7;

            // NOTE: Strictly for simplicity, we're rounding to end out the pay period,
            //      rather than worry about fragmented pay. In real-world application, this
            //      would be unnacceptable.
            int payPeriods = (int)( weeks / 2 );

            // Payment per pay period * number of pay periods gives gross pay.
            double grossPay = ( employee.salary / 26 ) * payPeriods;
            return grossPay;
        }
    }
}
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
    public enum SortDirection { Ascending, Descending };
    class Employee
    {
        public string employeeId;
        public string firstName;
        public string lastName;
        public char payType;
        public decimal salary;
        public Nullable<DateTime> startDate;
        public string state;
        public int hoursPerPeriod;
        public decimal grossPay;
        public decimal fedTaxTotal;
        public decimal stateTaxTotal;
        public decimal netPay;
        public int yearsWorked;
    }    

    class InputFileHandler
    {
        static DateTime todayDate = DateTime.Today;
        List<Employee> employeeList = new List<Employee>();

        public void ReadEmployees( string inputFileName )
        {
            string[] inputText = File.ReadAllLines( inputFileName );

            // TODO: Remove debugging break cases.
            // int i = 0;
            foreach( string line in inputText )
            {
                // if( i > 100 )
                // {
                //     break;
                // }
                string[] column = line.Split( new char[] {','} );
                StoreEmployee( line );
                // i++;
            }
            CalculatePaychecks( employeeList );
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

            employee.employeeId = data[0];
            employee.firstName = data[1];
            employee.lastName = data[2];
            employee.payType = Char.Parse( data[3] );
            employee.salary = Decimal.Parse( data[4] );
            employee.state = data[6];
            employee.hoursPerPeriod = Int32.Parse( data[7] );
            employeeList.Add( employee );
        }

        public void PrintEmployeePaycheckData()
        {
            foreach( Employee employee in employeeList )
            {
                {
                    WriteEmployeePaycheckData( employee );
                }
            }
        }

        // TODO: Generalize printing functionality.
        private void WriteEmployeePaycheckData( Employee employee )
        {
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}",
                employee.employeeId, employee.firstName,
                employee.lastName,
                employee.grossPay.ToString("#.#0"),
                employee.fedTaxTotal.ToString("#.#0"),
                employee.stateTaxTotal.ToString("#.#0"),
                employee.netPay.ToString("#.#0"));
        }

        private void CalculatePaychecks( List<Employee> employeeList )
        {
            foreach( Employee employee in employeeList )
            {
                string state = employee.state;
                decimal stateTax = 0;
                decimal fedTax = 0.15m;

                if( state == "UT" || state == "WY" || state == "NV" )
                {
                    stateTax = .05m;
                }
                else if( state == "CO" || state == "ID" || state == "AZ" || state == "OR" )
                {
                    stateTax = .65m;
                }
                else if( state == "WA" || state == "NM" || state == "TX" )
                {
                    stateTax = .07m;
                }
                
                if( employee.payType == 'H' )
                {
                    // TODO: Guard against null startdate here?
                    employee.grossPay = CalculateHourlyPay( employee );
                }
                else
                {
                    employee.grossPay = CalculateSalariedPay( employee );
                }
                
                employee.fedTaxTotal = employee.grossPay * fedTax;
                employee.stateTaxTotal = employee.grossPay * stateTax;
                employee.netPay = ( employee.grossPay - ( employee.fedTaxTotal + employee.stateTaxTotal ) );
            }
        }

        public void SortEmployeesByGrossPayAscending()
        {
            employeeList.Sort( delegate( Employee e1, Employee e2) { return e1.grossPay.CompareTo( e2.grossPay ); });
        }
        
        // TODO: This method can be refactored to be more concise 
        //      and more clean in the future.
        private decimal CalculateHourlyPay( Employee employee )
        {
            // TODO: Convert this to try...catch later.
            if( employee.startDate == null )
            {
                Console.WriteLine( "Could not calculate gross pay for {0} {1}.", employee.firstName, employee.lastName );
                Console.WriteLine( "Start date not found." );
                return 0;
            }
            int payPeriods = GetNumberOfPayPeriods( employee );

            if( payPeriods < 1 )
            {
                Console.WriteLine( "No pay periods found for {0} {1}.", employee.firstName, employee.lastName );
                return 0;
            }
            decimal rate = employee.salary;
            decimal overtimePay = 0;
            int hours = employee.hoursPerPeriod;
            decimal totalPay = 0;

            // TODO: This logic sequence can be simplified with a bit more thought.
            if( hours > 80 )
            {
                totalPay = ( hours * rate );
                hours = hours - 80;

                if( hours <= 10 )
                {
                    overtimePay = ( hours * ( 1.5m * rate ) );
                }
                else if( hours > 10 )
                {
                    overtimePay = ( hours * ( 1.5m * rate ) );
                    hours = hours - 10;

                    overtimePay += ( hours * ( 1.75m * rate ) );
                }

                totalPay += overtimePay;
            }
            else
            {
                totalPay = hours * rate;
            }

            totalPay = totalPay * payPeriods;
            return totalPay;
        }

        private decimal CalculateSalariedPay( Employee employee )
        {
            if( employee.startDate == null )
            {
                Console.WriteLine( "Could not calculate gross pay for {0} {1}.", employee.firstName, employee.lastName );
                Console.WriteLine( "Start date not found." );
                return 0;
            }

            int payPeriods = GetNumberOfPayPeriods( employee );

            // Payment per pay period * number of pay periods gives gross pay.
            decimal grossPay = ( employee.salary / 26 ) * payPeriods;
            return grossPay;
        }

        private int GetNumberOfPayPeriods( Employee employee )
        {
            // Get total time worked, from start to today.
            // TimeSpan ts = employee.startDate.Value.Subtract( todayDate );
            TimeSpan ts = todayDate.Subtract( employee.startDate.Value );
            int dateDiff = ts.Days;
            int weeks = (int)( dateDiff / 7 );

            // NOTE: Strictly for simplicity, we're rounding to end out the pay period,
            //      rather than worry about fragmented pay. In real-world application, this
            //      would be unnacceptable.
            return (int)( weeks / 2 );
        }

        private int GetNumberOfYearsWorked( Employee employee )
        {
            TimeSpan ts = todayDate.Subtract( employee.startDate.Value );
            int dateDiff = ts.Days;
            int yearsWorked = (int)( dateDiff / 365 );
            return yearsWorked;
        }

        private List<Employee> GetTopEarners( List<Employee> employeeList )
        {
            int numTopEarners = (int)( employeeList.Count * 0.15 );
            var topEarnersList = employeeList.Take( numTopEarners ).ToList();
            SortTopEarners();
            return topEarnersList;
        }

        private void SortTopEarners()
        {
            foreach( Employee employee in employeeList )
            {
                employee.yearsWorked = GetNumberOfYearsWorked( employee );
            }
            employeeList = employeeList.OrderByDescending( x => x.yearsWorked ).ThenBy( x => x.lastName ).ToList();
        }

        public void PrintTopEarners()
        {
            var topEarnersList = GetTopEarners( employeeList );

            foreach( Employee employee in employeeList )
            {
                {
                    WriteTopEarnerData( employee );
                }
            }
        }
        private void WriteTopEarnerData( Employee employee )
        {
            
            Console.WriteLine("{0} {1} {2} {3}",
                employee.firstName,
                employee.lastName,
                employee.yearsWorked,
                employee.grossPay.ToString("#.#0"));
        }
    }
}
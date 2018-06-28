# snm-programming-test
This is an assessment from a potential employer involving financial data and accounting calculations.

## Background
The test file contains some basic information about some employees. You are given their *employee id*, *first name*, *last name*, *pay type* (H for hourly, S for salaried), *salary* (for hourly employees it is their hourly rate, for salaried it is their annual salary), *start date*, *state of residence*, and *number of hours* worked in a 2 week period of time.

Pay is calculated for hourly employees as hourly rate for the first 80 hours, the next 10 hours are at 150% of their rate, the rest is 175% of their rate.

 For salaried employees they are paid every 2 weeks assuming a 52 week year. They are paid for 2 weeks regardless of hours worked.

The federal tax for every employee is 15%.

The state tax for every employee is found are as follows:

* 5%          : UT,WY,NV

* 6.5%      : CO, ID, AZ, OR

* 7%          : WA, NM, TX

## Requirements

Generate a text file for each of the following scenarios (3 files total):

1. Calculate pay checks for every line in the text file. The output should be: employee id, first name, last name, gross pay, federal tax, state tax, net pay. Ordered by gross pay (highest to lowest)

2. A list of the top 15% earners sorted by the number or years worked at the company (highest to lowest), then alphabetically by last name then first. The output should be first name, last name, number of years worked, gross pay

3. A list of all states with median time worked, median net pay, and total state taxes paid. The output should be state, median time worked, median net pay, state taxes. Ordered by states alphabetically

 4. Write a method that given an employee id would efficiently bring back an employee’s data 

*Example:* Employee GetByEmployeeId(string employeeId)

**Note: All numbers should be rounded to 2 decimal places.**

---

## Developer Information


I have chosen to do this task in C# to best fit the needs of the position.

I will use Visual Studio Code to write, compile, and test with, and use GitHub (git) for source control.

The sample data file is stored locally on my machine, but may be uploaded later for reference.

As of 2018-06-28, this assessment is deemed "complete", but still unrefined. Many avenues for improvement still remain, and are marked by various "TODO" comments throughout the classes. The goal is for this to serve as a cursory snapshot of my organizational skills, development pattern, and overall software engineering process, NOT production-level code.

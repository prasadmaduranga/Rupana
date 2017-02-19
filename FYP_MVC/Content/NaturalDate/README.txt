NaturalDate Natural Language Date Parsing .NET Module
-----------------------------------------------------

Please contact Precision Software Design, LLC at 
contact@precisionsoftware.us with any questions or comments.


Usage
-----
  1. Extract all zip file contents.
  2. Copy the Assembly file, NaturalDate.dll to your project directory.
  3. Add it as a reference to your project.
  4. Add the PrecisionSoftware namespace to the using list in your C# or Visual Basic file.
  5. Code against the NaturalDate API.  Documentation is found in API Documentation/index.html



Examples of date string parsable by NaturalDate
-----------------------------------------------
  1/1/2009, 12:00PM
  1/1/2009, 2-3 PM
  Jan 1, 2009, 2 to 3 PM
  January 1
  August 31st
  January 1, 2009
  January, 2009
  June 10-July 1, 2012
  Mondays, 6PM
  Mondays at 3PM
  Every Monday, 6PM
  Every Friday
  First Monday of every month
  Last Sunday of the month
  Everyday
  Everyday, 2 to 3 PM
  1st of the Month
  31st of every Month




Example usages of NaturalDate
-----------------------------
  1. Attempt to parse a date string:
    NaturalDate date;
    if (! NaturalDate.TryParse("January 1, 2009, 10-11PM", out date))
    {
      Console.WriteLine(date.ErrorString);
    }

  2. Enumerate a recurring date:
    foreach (NaturalDate d in 
      NaturalDate.Enumerate("Every Friday", "January 1, 2009"))
    {
      Console.WriteLine(d);
    }

  3. Find the next occurrence of a recurring date:
    Console.WriteLine(
      NaturalDate.Next("Every Friday at 7PM", "January 1, 2009"));

  4. Using Linq (.NET 3.5), this example walks the week ahead, starting with
    today, and finds all events that overlap each day.  It assumes EventList
    is an list of objects containing a 'When' property which is a NaturalDate.
    
    foreach (var day in NaturalDate.Enumerate("everyday", "today").Take(7))
    {
      var daysEvents = from e in EventList where e.When.Overlaps(day)
        orderby e.When.Next(day) select e;
      ...
    }

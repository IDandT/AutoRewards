using System;
using System.Threading;



namespace AutoRewards
{
    class Program
    {
        static void Main(string[] args)
        {

            char letter;

            // Every day, one different letter (second char of day name).
            letter = DateTime.Today.DayOfWeek.ToString().ToUpper()[1];

            // Mobile points
            BingSearcher.GetPoints(true, letter);

            // Dektop points
            BingSearcher.GetPoints(false, letter);


        }
    }
}

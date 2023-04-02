namespace AutoRewards
{
    class Program
    {
        static void Main()
        {

            char letter;

            // Every day, one different letter (second char of day name).
            letter = DateTime.Today.DayOfWeek.ToString().ToUpper()[1];

            // Edge desktop points
            EdgeBingSearcher.GetPoints(letter);

            // Mobile points
            ChromeBingSearcher.GetPoints(true, letter);

            // Dektop points
            ChromeBingSearcher.GetPoints(false, letter);

        }
    }
}

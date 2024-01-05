namespace AutoRewards;

enum Browser
{
    Chrome,
    Edge
}

enum SearchType
{
    Mobile,
    Desktop
}

enum ExitCodes
{
    Correct = 0,
    NoBrowser = 1,
    NoSearchType = 2,
    NoCount = 3,
    InvalidCount = 4,
    InvalidArgument = 5,

}

class Program
{
    static void WriteAvailableArguments()
    {
        Console.WriteLine("");
        Console.WriteLine("Supported browsers:");
        Console.WriteLine("\t--chrome\tUse Chrome browser");
        Console.WriteLine("\t--edge\t\tUse Edge browser");
        Console.WriteLine("");
        Console.WriteLine("Supported search types:");
        Console.WriteLine("\t--mobile\tEmulate mobile mode");
        Console.WriteLine("\t--desktop\tNormal desktop mode");
        Console.WriteLine("");
        Console.WriteLine("Other arguments:");
        Console.WriteLine("\t--count N\tThe number of sarches to do (example: --count 5)");
        Console.WriteLine("\t--help\t\tShow this help");
        Console.WriteLine("");
    }


    static void Main(string[] args)
    {
        Browser? selectedBrowser = null;
        SearchType? selectedSearchType = null;
        int? count = null;

        Console.WriteLine("");


        // Analyze the arguments
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--help":
                    WriteAvailableArguments();
                    Environment.Exit((int)ExitCodes.Correct);
                    break;
                case "--chrome":
                    selectedBrowser = Browser.Chrome;
                    break;
                case "--edge":
                    selectedBrowser = Browser.Edge;
                    break;
                case "--desktop":
                    selectedSearchType = SearchType.Desktop;
                    break;
                case "--mobile":
                    selectedSearchType = SearchType.Mobile;
                    break;
                case "--count":
                    // Check for numeric value after argument
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int value))
                    {
                        count = value;
                        i++;    // Jump next argument
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Integer value not found after --count.\n");
                        Console.WriteLine("Use --help to show valid arguments.\n");
                        Environment.Exit((int)ExitCodes.InvalidCount);
                    }
                    break;
                default:
                    Console.WriteLine($"ERROR: Invalid argument: {args[i]}\n");
                    Console.WriteLine("Use --help to show valid arguments.\n");
                    Environment.Exit((int)ExitCodes.InvalidArgument);
                    break;
            }
        }


        // Check some errors
        if (selectedBrowser == null)
        {
            Console.WriteLine("ERROR: No browser specified.\n");
            Console.WriteLine("Use --help to show valid arguments.\n");
            Environment.Exit((int)ExitCodes.NoBrowser);
        }

        if (selectedSearchType == null)
        {
            Console.WriteLine("ERROR: No search type specified.\n");
            Console.WriteLine("Use --help to show valid arguments.\n");
            Environment.Exit((int)ExitCodes.NoSearchType);
        }

        if (count == null || count <= 0)
        {
            Console.WriteLine("\nERROR: No search count specified. Use --count argument.\n");
            Console.WriteLine("Use --help to show valid arguments.\n");
            Environment.Exit((int)ExitCodes.NoCount);
        }

        if (selectedBrowser == Browser.Chrome)
        {
            ChromeBingSearcher.GetPoints(selectedSearchType, count);
        }
        else if (selectedBrowser == Browser.Edge)
        {
            EdgeBingSearcher.GetPoints(selectedSearchType, count);
        }


        Console.WriteLine("End of task.\n");

        Environment.Exit(0);
    }
}


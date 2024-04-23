using System.Linq.Expressions;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using SharableSpreadSheet;
using DeadlockDetection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Drawing;

namespace SharableSpreadSheet;


internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]


    static void Main(string[] args)
    {

        int rows, cols, nThreads, nOperations, mssleep;


        try
        {
            rows = Int32.Parse(args[0]);
            cols = Int32.Parse(args[1]);
            nThreads = Int32.Parse(args[2]);
            nOperations = Int32.Parse(args[3]);
            mssleep = Int32.Parse(args[4]);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Invalid format type of parameters provided.");
            return;
        }

        catch (Exception ex)
        {
            Console.WriteLine("missing arugments");
            return;
        }


        /*
        int rows = 100;
        int cols = 1000;
        int nThreads = 30;
        int nOperations = 100;
        int mssleep = 50;
        */



        //using (Enable.DeadlockDetection(DeadlockDetectionMode.AlsoPotentialDeadlocks))
        //{
        SharableSpreadSheet s = new SharableSpreadSheet(rows, cols);
        Parallel.For(0, nThreads, i => { 
                randomizedFunc(s, mssleep, nOperations, i);
            });
        //}
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("The simulator completed");
        Console.ResetColor();

}

    public static void randomizedFunc(SharableSpreadSheet s1, int mssleep, int op, int user)
    {

        
        for (int i = 1; i < op+1; i++)
        {
            Random rnd = new Random();
            int randomFunction = rnd.Next(1, 14);
            switch (randomFunction)
            {

                case 1:
                    { // statement sequence
                        try
                        {
                            s1.addCol(1);
                            if (i == op)
                            {
                                WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                            }
                            Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  a new column added after column 1");
                            Thread.Sleep(mssleep);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                            break;
                        }
                    }

                case 2: // statement sequence
                    {
                        try
                        {
                            s1.addRow(4);
                            if (i == op)
                            {
                                WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                            }
                            Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  a new row added after row 3");
                            Thread.Sleep(mssleep);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred: " + ex.Message);
                            break;
                        }
                    }
                case 3: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        s1.setCell(1, 1, "barak");
            
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  string \"barak\" inserted to cell [1,1].");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 4: // statement sequence
                    {
                        try
                        {
                            if (i == op)
                            {
                                WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                            }
                            Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  string {s1.getCell(3, 4)} was getted from cell [3,4]");
                            Thread.Sleep(mssleep);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 5: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.exchangeCols(1, 3);
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  exchange between col 1 and col 3");
                        Thread.Sleep(mssleep);
                        break;
                    }
                        catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
            }
                case 6: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.exchangeRows(13,15);
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  exchange between row 13 and row 15");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                
                case 7: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}]  the size of the spreadsheet is {s1.getSize()}");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }

                case 8: // statement sequence
                    {
                        try{
                            if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        s1.setAll("Barak", "newBarak", false);
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] change all cells contains the string \"Barak\" with \"newBarak\" and ignore case sensative");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 9: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.searchInRange(1,3,2,4,"barak");
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] search the string \"barak\" in range of rows:1-3, cols:2-4");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 10: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.searchString("barak");
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] search the string \"barak\" in the whole spreadsheet");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 11: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] search the string \"barak\" in row 1");
                        s1.searchInRow(1,"barak");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                case 12: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.searchInCol(1, "barak");
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] search the string \"barak\" in col 1");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }

                case 13: // statement sequence
                    {
                        try { 
                        if (i == op)
                        {
                            WriteLineWithColor($"---------------{user} reach op {op} so thread {user} is done!!!!---------------", ConsoleColor.Red);
                        }
                        
                        s1.findAll("barak", true);
                        Console.WriteLine($"op number {i} of user[{user}]: [{DateTime.Now}] find all the location of the string \"barak\" within spreadsheet, no ignore for case sensative");
                        Thread.Sleep(mssleep);
                        break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The for loop completed");
        Console.ResetColor();
        static void WriteLineWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }

}

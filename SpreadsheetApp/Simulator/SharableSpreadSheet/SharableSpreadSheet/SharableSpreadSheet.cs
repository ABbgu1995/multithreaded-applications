using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using OfficeOpenXml;

namespace SharableSpreadSheet
{


    public class SharableSpreadSheet
    {
        private int nRows;
        private int nCols;
        private int nUsers;
        private string[,] spreadSheet;
        private ReaderWriterLockSlim[] rowLocks;
        private Mutex[] sizes;



        public SharableSpreadSheet(int nRows, int nCols, int nUsers = -1)
        {

            this.nRows = nRows;
            this.nCols = nCols;
            this.nUsers = nUsers;
            this.spreadSheet = new string[nRows, nCols];
            sizes = new Mutex[2];
            sizes[0] = new Mutex();
            sizes[1] = new Mutex();
            fillUp(this.spreadSheet);
            this.rowLocks = new ReaderWriterLockSlim[nRows];
            for (int i = 0; i < nRows; i++)
            {
                this.rowLocks[i] = new ReaderWriterLockSlim();
            }

            // nUsers used for setConcurrentSearchLimit, -1 mean no limit.
            // construct a nRows*nCols spreadsheet
        }
        public String getCell(int row, int col)
        {

            string cellContent = "";
            this.rowLocks[row].EnterReadLock();
            try
            {
                cellContent = this.spreadSheet[row, col];
            }
            finally
            {
                this.rowLocks[row].ExitReadLock();
            }
            return cellContent;
        }
        /**
        try
        {
            string cell_content;
            rows_re_Mutex[row].WaitOne();
            read_counts_per_row[row]++;
            if (read_counts_per_row[row] == 1)
                rows_rw_Mutex[row].WaitOne();
            rows_re_Mutex[row].ReleaseMutex();
            cell_content = this.spreadSheet[row, col];
            rows_re_Mutex[row].WaitOne();

            read_counts_per_row[row]--;
            if (read_counts_per_row[row] == 0)
                rows_rw_Mutex[row].ReleaseMutex();
            rows_re_Mutex[row].ReleaseMutex();
            return cell_content;
        }

        catch (ApplicationException)
        {
            rows_re_Mutex[row].ReleaseMutex();
            return "";
        }
        */


        public void setCell(int row, int col, String str)
        {
            this.rowLocks[row].EnterWriteLock();
            try
            {
                this.spreadSheet[row, col] = str;
            }
            finally
            {
                this.rowLocks[row].ExitWriteLock();
            }
            /*
            this.rows_rw_Mutex[row].WaitOne();
            this.spreadSheet[row, col] = str;
            this.rows_rw_Mutex[row].ReleaseMutex();
            */
        }

        public Tuple<int, int> searchString(String str)
        {
            // return first cell indexes that contains the string (search from first row to the last row)
            (int row, int col) result = (-1, -1);
            object lockObject = new object(); // used to lock the result variable
            Parallel.For(0, nRows, i => //The Parallel.For method in C# is used to execute a loop in parallel, where each iteration of the loop can be executed independently on a separate thread
            {
                rowLocks[i].EnterReadLock();
                try
                {
                    for (int j = 0; j < nCols; j++)
                    {
                        if (spreadSheet[i, j] == str)
                        {
                            lock (lockObject) // lock the result variable to ensure thread-safety
                            {
                                result = (i, j); // set the result variable to the matching row and column index
                                                 //The result variable stores the matching row and column index of the cell that matches the search string.
                                                 //Since multiple threads may access the result variable in parallel,
                                                 //we need to ensure that only one thread at a time can modify the value of result.
                                                 //By using lock (lockObject) around the code that sets the value of result,
                                                 //we ensure that only one thread at a time can modify the value of result. When a thread acquires a lock on lockObject
                                                 //, any other thread that tries to acquire a lock on the same object will be blocked until the first thread
                                                 //releases the lock. This ensures that the value of result is modified atomically, without any race conditions or
                                                 //inconsistent behavior due to multiple threads modifying the value of result simultaneously.
                                return; //he return statement after result inside the lock block is used to exit the loop and the parallel search
                                        //when a cell that matches the search string is found. The return statement terminates the current
                                        //thread's execution of the Parallel.For loop and returns control to the calling method,
                                        //which in this case is searchString.
                            }
                        }
                    }
                }
                finally
                {
                    rowLocks[i].ExitReadLock();
                }
            });
            return Tuple.Create(result.row, result.col); // return the result as a tuple

        }

        public void exchangeRows(int row1, int row2)
        {
            // exchange the content of row1 and row2
            rowLocks[row1].EnterWriteLock();
            rowLocks[row2].EnterWriteLock();
            try
            {
                for (int col = 0; col < nCols; col++)
                {
                    string temp = spreadSheet[row1, col];
                    spreadSheet[row1, col] = spreadSheet[row2, col];
                    spreadSheet[row2, col] = temp;
                }
            }
            finally
            {
                rowLocks[row1].ExitWriteLock();
                rowLocks[row2].ExitWriteLock();
            }
        }
        public void exchangeCols(int col1, int col2)
        {
            // exchange the content of col1 and col2 
            /*
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }
            */
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
                try
                {
                    string temp = spreadSheet[row, col1];
                    spreadSheet[row, col1] = spreadSheet[row, col2];
                    spreadSheet[row, col2] = temp;
                }
                finally
                //The finally block begins with the finally keyword and contains the code that must be executed when control leaves the try block
                {
                    rowLocks[row].ExitWriteLock();
                }
            }
            /*
            finally
            {
                for (int row = 0; row < nRows; row++)
                {
                    rowLocks[row].ExitWriteLock();
                }
            }
            */
        }

        public int searchInRow(int row, String str)
        {
            // also can split to mulitple searching threads
            int col = -1;
            rowLocks[row].EnterReadLock();
            try
            {
                for (int i = 0; i < nCols; i++)
                {
                    if (spreadSheet[row, i] == str)
                    {
                        col = i;
                        break;
                    }
                }
            }
            finally
            {
                rowLocks[row].ExitReadLock();
            }
            return col;
        }

        public int searchInCol(int col, String str)
        {
            int row = -1;
            object lockObject = new object();
            Parallel.For(0, nRows, check_row =>
            {
                rowLocks[check_row].EnterReadLock();
                try
                {
                    if (spreadSheet[check_row, col].Equals(str))
                    {
                        lock (lockObject)
                        {
                            row = check_row;
                            return;
                        }
                    }
                }

                finally
                //The finally block begins with the finally keyword and contains the code that must be executed when control leaves the try block
                {
                    rowLocks[check_row].ExitReadLock();
                }
            });
            return row;
        }
        public void addRow(int row1)
        {
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }

            try
            {
                // Create a new spreadsheet with an additional row
                string[,] newSpreadSheet = new string[nRows + 1, nCols];

                // Copy the existing spreadsheet up to row1
                for (int row = 0; row <= row1; row++)
                {
                    for (int col = 0; col < nCols; col++)
                    {
                        newSpreadSheet[row, col] = spreadSheet[row, col];
                    }
                }

                // Copy the existing spreadsheet after row1
                for (int row = row1 + 1; row < nRows; row++)
                {
                    for (int col = 0; col < nCols; col++)
                    {
                        newSpreadSheet[row + 1, col] = spreadSheet[row, col];
                    }
                }

                // Set the new row to empty strings
                for (int col = 0; col < nCols; col++)
                {
                    newSpreadSheet[row1 + 1, col] = "";
                }

                // Update the spreadsheet and row locks
                sizes[0].WaitOne();
                spreadSheet = newSpreadSheet;
                nRows++;
                sizes[0].ReleaseMutex();
                Array.Resize(ref rowLocks, nRows);
                rowLocks[nRows - 1] = new ReaderWriterLockSlim();
                rowLocks[nRows - 1].EnterWriteLock();
                /*
                rowLocks[row1 + 1] = new ReaderWriterLockSlim();
                for (int row = nRows - 1; row > row1 + 1; row--)
                {
                    rowLocks[row] = rowLocks[row - 1];
                }
                */
            }
            finally
            {
                for (int row = 0; row < nRows; row++)
                {
                    rowLocks[row].ExitWriteLock();
                }
                save("addrow");
            }
        }

        public void addCol(int col1)
        {
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }
            try
            {
                // Create a new spreadsheet with an additional row
                string[,] newSpreadSheet = new string[nRows, nCols+1];

                // Copy the existing spreadsheet up to row1
                for (int row = 0; row < nRows; row++)
                {
                    for (int col = 0; col <= col1; col++)
                    {
                        newSpreadSheet[row, col] = spreadSheet[row, col];
                    }

                    for (int col = col1 + 1; col < nCols; col++)
                    {
                        newSpreadSheet[row, col+1] = spreadSheet[row, col];
                    }
                   
                   newSpreadSheet[row, col1+1] = "";
                    
                }
                // Update the spreadsheet and row locks
                sizes[0].WaitOne();
                spreadSheet = newSpreadSheet;
                nCols++;
                sizes[0].ReleaseMutex();
                Array.Resize(ref rowLocks, nRows);
                rowLocks[nRows - 1] = new ReaderWriterLockSlim();
                rowLocks[nRows - 1].EnterWriteLock();
                /*
                rowLocks[row1 + 1] = new ReaderWriterLockSlim();
                for (int row = nRows - 1; row > row1 + 1; row--)
                {
                    rowLocks[row] = rowLocks[row - 1];
                }
                */
            }
            finally
            {
                for (int row = 0; row < nRows; row++)
                {
                    rowLocks[row].ExitWriteLock();
                }
                save("addcol");
            }
        }
    


        private void fillUp(string[,] spreedsheet)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";


            for (int row = 0; row < spreadSheet.GetLength(0); row++)
            {
                for (int col = 0; col < spreadSheet.GetLength(1); col++)
                {
                    string randomString = "";
                    for (int i = 0; i < 7; i++)
                    {
                        int index = new Random().Next(0, alphabet.Length);
                        char letter = alphabet[index];
                        randomString += letter;
                    }
                    spreadSheet[row, col] = randomString;
                }
            }
        }

        public Tuple<int, int> getSize()
        {
            // need to protect the values addRow and addCol function
            // return the size of the spreadsheet in nRows, nCols
            Tuple<int, int> rs;
            sizes[0].WaitOne();
            sizes[1].WaitOne();
            try
            {
                rs = Tuple.Create(this.nRows, this.nCols);
            }
            finally
            {
                sizes[0].ReleaseMutex();
                sizes[1].ReleaseMutex();
            }
            return rs;
        }
        public void save(String fileName)
        {
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }
            try
            {
                // Create a new Excel package
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Add a new worksheet to the package
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    // Populate the worksheet with data from the 2D array
                    for (int row = 0; row < this.spreadSheet.GetLength(0); row++)
                    {
                        for (int col = 0; col < this.spreadSheet.GetLength(1); col++)
                        {
                            worksheet.Cells[row + 1, col + 1].Value = this.spreadSheet[row, col];
                        }
                    }

                    // Save the package to a file
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    package.SaveAs(new FileInfo(String.Concat(fileName, ".xlsx")));
                }
            }
            finally
            {
                for (int row = 0; row < nRows; row++)
                {
                    rowLocks[row].ExitWriteLock();
                }
            }
        }

        public Tuple<int, int>[] findAll(String str, bool caseSensitive)
        {
            List<(int, int)> tupleList = new List<(int, int)>();
            object lockObject = new object(); // used to lock the result variable
            Parallel.For(0, nRows, i => //The Parallel.For method in C# is used to execute a loop in parallel, where each iteration of the loop can be executed independently on a separate thread
            {
                rowLocks[i].EnterReadLock();
                try
                {
                    for (int j = 0; j < nCols; j++)
                    {

                        if (caseSensitive == true)
                        {
                            if (spreadSheet[i, j] == str)
                            {
                                lock (lockObject) // lock the result variable to ensure thread-safety
                                {
                                    tupleList.Add((i, j));
                                }
                            }
                        }

                        else
                        {
                            if (string.Equals(spreadSheet[i, j], str, StringComparison.OrdinalIgnoreCase))
                            {
                                lock (lockObject) // lock the result variable to ensure thread-safety
                                {
                                    tupleList.Add((i, j));
                                }
                            }
                        
                        }
                    }
                }

                finally
                {
                    rowLocks[i].ExitReadLock();
                }
            });
            Tuple<int, int>[] tupleArray = tupleList.ConvertAll(t => Tuple.Create(t.Item1, t.Item2)).ToArray(); // return the result as a tuple
            return tupleArray; 
        }

        public void load(String fileName)
        {
            // load the spreadsheet from fileName
            // replace the data and size of the current spreadsheet with the loaded data


            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }
            try
            {
                using var package = new ExcelPackage(new FileInfo(String.Concat(Directory.GetCurrentDirectory(), '\\', fileName, ".xlsx")));
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;

                string[,] spreadsheet = new string[rowCount, colCount];

                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {

                        spreadsheet[row - 1, col - 1] = worksheet.Cells[row, col].Value.ToString();
                    }
                }

                this.spreadSheet = spreadsheet;
            }

            finally
            {
                for (int row = 0; row < nRows; row++)
                {
                    rowLocks[row].ExitWriteLock();
                }
            }
        }

            private void displayarry() {
            for (int i = 0; i < this.spreadSheet.GetLength(0); i++)
            {
                Console.Write("| ");
                for (int j = 1; j < this.spreadSheet.GetLength(1); j++) // Start at 1 to skip the first column
                {
                    Console.Write(this.spreadSheet[i, j] + " | ");
                }
                Console.WriteLine();
            }
        }


    }
}

/*
public Tuple<int, int> searchInRange(int col1, int col2, int row1, int row2, String str)
{
    int row, col
    // perform search within spesific range: [row1:row2,col1:col2] 
    //includes col1,col2,row1,row2
return < row,col >;
}

public void setAll(String oldStr, String newStr bool caseSensitive)
{
    // replace all oldStr cells with the newStr str according to caseSensitive param
}


*/


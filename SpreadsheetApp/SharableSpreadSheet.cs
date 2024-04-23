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
                this.rowLocks[i] = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            }

            // nUsers used for setConcurrentSearchLimit, -1 mean no limit.
            // construct a nRows*nCols spreadsheet
        }
        public String getCell(int row, int col)
        {
            
                if (row < 0 || row >= this.spreadSheet.GetLength(0))
                {
                    throw new ArgumentException("getCell error: Invalid row index", "row");
                }

                if (col < 0 || col >= this.spreadSheet.GetLength(1))
                {
                    throw new ArgumentException("getCell error: Invalid col index", "col");
                }
                try
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
                catch (Exception ex)
                {
                    throw new Exception("Error in getCell function", ex);
                }

        }
        
        public void setCell(int row, int col, String str)
        {
            if (row < 0 || row >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("setCell error: row", "Invalid row index");
            }

            if (col < 0 || col >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("setCell error: col", "Invalid column index");
            }
            try
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error in setCell function", ex);
            }
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

            if (row1 < 0 || row1 >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentException("exchangeRows error: Invalid row1 index", "row1");
            }

            if (row2 < 0 || row2 >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentException("exchangeRows error: Invalid row2 index", "row2");
            }
            try { 
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
            catch (Exception ex)
            {
                throw new Exception("Error in exchangeRows function", ex);
            }
        }
        public void exchangeCols(int col1, int col2)
        {
            // exchange the content of col1 and col2 
            if (col1 < 0 || col1 >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("exchangeCols error: col1", "Invalid column index");
            }

            if (col2 < 0 || col2 >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("exchangeCols error: col2", "Invalid column index");
            }

            try { 
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error in exchangeCols function", ex);
            }
        }

        public int searchInRow(int row, String str)
        {
            // also can split to mulitple searching threads

            // exchange the content of col1 and col2 
            int col = -1;
            if (row < 0 || row >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("searchInRow error: row", "Invalid column index");
            }
            try { 
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
            catch (Exception ex)
            {
                throw new Exception("Error in searchInRow function", ex);
            }
        }

        public int searchInCol(int col, String str)
        {
            if (col < 0 || col >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("searchInCol error: col", "Invalid column index");
            }
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error in searchInCol function", ex);
            }
            
        }
        public void addRow(int row1)
        {
            if (row1 < 0 || row1 >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("addRow error: row1", "Invalid column index");
            }
            try { 
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
                }
            }
                catch (Exception ex)
            {
                throw new Exception("Error in addRow function", ex);
            }
        }

        public void addCol(int col1)

        {
            if (col1 < 0 || col1 >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("addCol error: col1", "Invalid column index");
            }
            try { 
            for (int row = 0; row < nRows; row++)
            {
                rowLocks[row].EnterWriteLock();
            }
            try
            {
                // Create a new spreadsheet with an additional row
                string[,] newSpreadSheet = new string[nRows, nCols + 1];

                // Copy the existing spreadsheet up to row1
                for (int row = 0; row < nRows; row++)
                {
                    for (int col = 0; col <= col1; col++)
                    {
                        newSpreadSheet[row, col] = spreadSheet[row, col];
                    }

                    for (int col = col1 + 1; col < nCols; col++)
                    {
                        newSpreadSheet[row, col + 1] = spreadSheet[row, col];
                    }

                    newSpreadSheet[row, col1 + 1] = "";

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
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in addCol function", ex);
            }
        }

        public Tuple<int, int> getSize()
        {
            // need to protect the values addRow and addCol function
            // return the size of the spreadsheet in nRows, nCols
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception("Error in getSize function", ex);
            }
        }
        public void save(String fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("load error: File does not exist");
            }
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error in save function", ex);
            }
        }

        public void setAll(String oldStr, String newStr, bool caseSensitive)
        {
            try { 
            Tuple<int, int>[] arrayTuple = findAll(oldStr, caseSensitive);
            for (int i = 0; i < arrayTuple.Length; i++) {
                setCell(arrayTuple[i].Item1, arrayTuple[i].Item2, newStr);
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in setAll function", ex);
            }

        }

        public Tuple<int, int>[] findAll(String str, bool caseSensitive)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error in findAll function", ex);
            }
        }

        public void load(String fileName)
        {
            // load the spreadsheet from fileName
            // replace the data and size of the current spreadsheet with the loaded data

            if (!File.Exists(fileName))
            {
                throw new Exception("load error: File does not exist");
            }
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception("Error in load function", ex);
            }
        }
        public Tuple<int, int> searchInRange(int col1, int col2, int row1, int row2, String str)
        {

            if (row1 < 0 || row1 >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("searchInRange error: row1", "Invalid row index");
            }

            if (col1 < 0 || col1 >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("searchInRange error: col1", "Invalid column index");
            }

            if (row2 < 0 || row2 >= this.spreadSheet.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("searchInRange error: row2", "Invalid row index");
            }

            if (col2 < 0 || col2 >= this.spreadSheet.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("searchInRange error: col2", "Invalid column index");
            }
            try
            {
                int row, col;
                // perform search within spesific range: [row1:row2,col1:col2] 
                //includes col1,col2,row1,row2

                (int row, int col) result = (-1, -1);
                object lockObject = new object(); // used to lock the result variable
                Parallel.For(row1, row2, i => //The Parallel.For method in C# is used to execute a loop in parallel, where each iteration of the loop can be executed independently on a separate thread
                {
                    rowLocks[i].EnterReadLock();
                    try
                    {
                        for (int j = col1; j < col2; j++)
                        {
                            if (this.spreadSheet[i, j].Equals(str))
                            {
                                lock (lockObject) // lock the result variable to ensure thread-safety
                                {
                                    result = (i, col1);
                                    return;
                                }
                            }
                        }
                    }
                    finally
                    {
                        rowLocks[i].ExitReadLock();
                    }
                });
                return Tuple.Create(result.row, result.col);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in searchInRange function", ex);
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


    }
}




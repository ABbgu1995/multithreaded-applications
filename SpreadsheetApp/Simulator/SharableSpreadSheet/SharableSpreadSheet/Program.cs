using System.Security.Cryptography;

namespace SharableSpreadSheet
{
    internal static class Program {
        static void Main()
        {

            
                SharableSpreadSheet s1 = new SharableSpreadSheet(4, 5);

                Thread t1 = new Thread(() =>
                {
                    s1.getCell(0, 0);
                    Console.Write("t1");
                });
                t1.Start();
                Thread t2 = new Thread(() =>
                {
                    s1.getCell(0, 0);
                    Console.Write("t2");
                });
                t2.Start();

            Thread t7 = new Thread(() =>
            {
                s1.addCol(2);
                Console.Write("t7");

            });
            t7.Start();

            Thread t200 = new Thread(() =>
            {
                s1.addRow(2);
                Console.Write("t7");

            });
            t200.Start();


            Thread t300 = new Thread(() =>
            {
                s1.load("first");
                s1.save("new");
                Console.Write("t7");

            });
            t300.Start();

            Thread t3 = new Thread(() =>
                {
                    s1.setCell(1, 1, "Hi");
                    Console.Write("t3");
                });
                t3.Start();

                Thread t4 = new Thread(() =>
                {
                    s1.setCell(3, 0, "hi");
                    Console.Write("t4");
                });
                t4.Start();

              
            /*            
            Thread t10 = new Thread(() =>
            {
                s1.exchangeCols(1, 3);
                Console.Write("t10");
            });
            t10.Start();
            */
            
            
            
            Thread t11 = new Thread(() =>
            {
                Console.Write(s1.searchInRow(1, "hi"));
                Console.Write("t11");
            });
            t11.Start();

            Thread t201 = new Thread(() =>
            {
                s1.findAll("hi", true);
                Console.Write("t11");
            });
            t201.Start();






        }
    }
}

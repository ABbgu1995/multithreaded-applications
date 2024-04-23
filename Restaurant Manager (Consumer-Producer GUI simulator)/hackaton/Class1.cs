using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hackaton
{
    public class Resrestaurant
    {
        private Semaphore empty;
        private Semaphore full;
        private Mutex mutex;

        private int count; //count how much catsumers is in the restaurant
        private int inIndex; //The captured chair
        private int outIndex; //The vacant chair
        private int freeChairs;

        private int[] resrestaurantBuffer;
        private int bufferSize;

        private int totalWaitingTime;

        public Resrestaurant (int capacity)
        {
            this.bufferSize = capacity;
            this.resrestaurantBuffer = new int[capacity];

            this.count = 0;
            this.inIndex = 0;
            this.outIndex = 0;
            this.freeChairs = capacity;

            this.empty=new Semaphore(capacity, capacity);
   
            this.full=new Semaphore(0,capacity);
            this.mutex=new Mutex();

        }

        public static void func1() { }
        public void Produce()
        {
            empty.WaitOne(); // Wait for an empty chair

            mutex.WaitOne(); //start critical section
            //resrestaurantBuffer[inIndex] = 1;
            //inIndex = (inIndex + 1) % bufferSize;
            //count++;
            //if (count>=bufferSize) { return; }
            int customerNumber = Interlocked.Increment(ref count);
            int index = Array.FindIndex(resrestaurantBuffer, seat => seat == 0);
            if (index >= 0)
            {
                resrestaurantBuffer[index] = customerNumber;
            }

            freeChairs = freeChairs - 1; ;

            mutex.ReleaseMutex();// end critical section
           
            full.Release(); // Signal that a slot is filled with a value
            

        }
        public void AddGroup(int whiting_time,int numOfPepole)
        {
            while (true)
            {
                for (int i = 0; i < numOfPepole; i++)
                {
                    Thread t1 = new Thread(() => Produce());
                    t1.Start();
                }
                Thread.Sleep(whiting_time);
            }
        }

        public void Consume()
        {
            full.WaitOne(); // Wait for a custumer that want to get out from the resturant

            mutex.WaitOne(); // Acquire mutex to modify buffer
                             //int value = resrestaurantBuffer[outIndex];
                             //outIndex = (outIndex + 1) % bufferSize;
                             //count--;
            //if (count == 0) { return; }
            int index = Array.FindIndex(resrestaurantBuffer, seat => seat != 0);
            if (index >= 0)
            {
                int customerNumber = resrestaurantBuffer[index];
                resrestaurantBuffer[index] = 0;
                int waitingTime = count - customerNumber;
                totalWaitingTime += waitingTime;

            }
            freeChairs = freeChairs + 1; ;


            mutex.ReleaseMutex();

            empty.Release(); // Signal that a slot is emptied
           
           
        }

        public void RemoveGroup(int whiting_time, int numOfPepole)
        {
            while (true)
            {
                for (int i = 0; i < numOfPepole; i++)
                {
                    Thread t1 = new Thread(() => Consume());
                    t1.Start();
                }

                Thread.Sleep(whiting_time);
            }
        }


        public int getCount() { return this.count; }
        public int getSize() { return this.bufferSize; }
        public int getEmptyPlaces() { return bufferSize - count; }
        public int getToatalWaitingTime() { return totalWaitingTime; }
        public int getFreeChairs() { return this.freeChairs; }

    }


}

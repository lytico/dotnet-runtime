// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// static field

using System;
using System.Threading;

namespace Precise
{
    internal class Driver
    {
        public static void f()
        {
            test.b = 0xF;
        }
        public static int Main()
        {
            try
            {
                Console.WriteLine("Testing .cctor() invocation by accessing static field across assembly");
                Console.WriteLine();
                Console.WriteLine("Before calling static field");
                // .cctor should not run yet
                if (measure.a != 0xCC)
                {
                    Console.WriteLine("in Main(), measure.a is {0}", measure.a);
                    Console.WriteLine("FAILED");
                    return 1;
                }
                // spin up 5 threads
                Thread[] tasks = new Thread[5];
                for (int i = 0; i < 5; i++)
                {
                    ThreadStart threadStart = new ThreadStart(f);
                    tasks[i] = new Thread(threadStart);
                    tasks[i].Name = "Thread #" + i;
                }

                // Start tasks
                foreach (Thread _thread in tasks)
                    _thread.Start();

                // Wait for tasks to finish	
                foreach (Thread _thread in tasks)
                    _thread.Join();

                // Should only have one thread accessing .cctor
                Console.WriteLine("After calling static field");
                if (measure.a != 212)
                {
                    Console.WriteLine("in Main(), measure.a is {0}", measure.a);
                    Console.WriteLine("FAILED");
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                return -1;
            }
            Console.WriteLine();
            Console.WriteLine("PASSED");
            return 100;
        }
    }
}
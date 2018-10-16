using System;

namespace LeonardosPrimeFactors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Solution
    {

        /*
         * Complete the primeCount function below.
         */
        static int primeCount(long n)
        {
            /*
             * Write your code here.
             */

            PrimeFinder primeFinder = new PrimeFinder();

            int primesMultiplied = 0;

            ulong uniquePrimeProduct = 1;

            while(uniquePrimeProduct <= (ulong)n)
            {
                // Find next prime and multiply
                long nextPrime = primeFinder.GetNextPrime();

                uniquePrimeProduct = uniquePrimeProduct * (ulong)nextPrime;

                primesMultiplied++;

                //Console.WriteLine("prod: " + uniquePrimeProduct + " num: " + primesMultiplied);
            }

            return primesMultiplied - 1;
        }

        static void Main(string[] args)
        {
            TextReader resultTextReader = new StreamReader(@"d:\Privat\HackerRank\LeonardosPrimeFactors\TestCase11\output11.txt");

            long input;
            long expectedResult;
            foreach (var line in File.ReadLines(@"d:\Privat\HackerRank\LeonardosPrimeFactors\TestCase11\input11.txt"))
            {
                input = long.Parse(line);
                expectedResult = long.Parse(resultTextReader.ReadLine());

                if(expectedResult != primeCount(input))
                {
                    Console.WriteLine($"Input: {input}, result: {primeCount(input)} , expected result: {expectedResult} ");
                    break;
                }

            }

            //List<long> inputs = new List<long> { 1, 2, 3, 500, 5000, 10000000000, 200560490129, 200560490130, 200560490131 };
            //    Console.WriteLine(input + " gives:");
            //    Console.WriteLine(primeCount(input));
 
        }
    }

    class PrimeFinder
    {
        List<long> primesFound = new List<long>();

        public long GetNextPrime()
        {
            if(primesFound.Count > 1)
            {
                long lastPrime = primesFound[primesFound.Count - 1];

                long nextPrimeCandidate = lastPrime + 2;
                while (!CheckPrime(nextPrimeCandidate)){

                    nextPrimeCandidate = nextPrimeCandidate + 2;
                }
                primesFound.Add(nextPrimeCandidate);
            }
            else if (primesFound.Count == 1)
            {
                primesFound.Add(3);
            }
            else
            {
                primesFound.Add(2);
            }

            return primesFound[primesFound.Count - 1];
        }

        private bool CheckPrime(long nextPrimeCandidate)
        {
            long sqrtOfCandidate = (long)Math.Sqrt(nextPrimeCandidate);

            foreach(long prime in primesFound)
            {
                if(prime > sqrtOfCandidate)
                {
                    return true;
                }

                if(nextPrimeCandidate % prime == 0)
                {
                    return false;
                }
            }

            Console.WriteLine("This should not happen 01");
            Console.ReadLine();
            return true;
        }
    }
}

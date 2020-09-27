using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            Dictionary<ulong, ulong> dict = new Dictionary<ulong, ulong>();
            
            Func<ulong, Dictionary<ulong, ulong>, ulong> fibCached = null;
            fibCached = (number, d) =>
            { 
                if (number == 1 || number == 2) return 1;
                
                if (d.ContainsKey(number))
                {
                    return d[number];
                }

                var result = fibCached(number - 2, d) + fibCached(number - 1, d);
                d.Add(number, result);

                return result;
            };
            
            return fibCached(n, dict);
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if(n <= 1) {
                return n;
            }
            ulong a = 1;
            ulong b = 1;
            for (ulong i = 2; i <= n; i ++)
            {
                var c = b;
                b += a;
                a = c;
            }
            return a;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}

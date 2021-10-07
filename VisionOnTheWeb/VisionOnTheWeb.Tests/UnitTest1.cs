using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace VisionOnTheWeb.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var web = new VisionWeb();
            await web.Login();
            await web.GetCustomer();

            var timeBag = new ConcurrentBag<long>();
            var time = Stopwatch.StartNew();
            await Parallel.ForEachAsync(Enumerable.Range(0, 500), async (i, c) =>
            {
                var custTime = Stopwatch.StartNew();
                await web.GetCustomer();
                custTime.Stop();
                timeBag.Add(custTime.ElapsedMilliseconds);
            });
            time.Stop();

            var average = timeBag.Average();
            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Average: {time.Elapsed}");
        }
    }
}
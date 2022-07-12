using RideScheduler.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideScheduler.Tests
{
    public class TestBase
    {
        public DataProvider DataProvider { get; private set; } = new TestDataProvider();
    }
}

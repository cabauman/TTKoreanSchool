using System;
using System.Threading.Tasks;
using Splat;
using TTKoreanSchool.Services;
using Xunit;

namespace TTKoreanSchool.Tests
{
    public class UnitTestSample
    {
        [Fact]
        public void ThisShouldPass()
        {
            Assert.True(true);
        }

        [Fact]
        public async Task ThisShouldFail()
        {
            await Task.Run(() => { throw new Exception("boom"); });
        }

        [Fact]
        public void LoggerRegistration()
        {
            var logger = new LoggingService();
            Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));
            Assert.Equal(logger, Locator.Current.GetService<ILogger>());
        }
    }
}
using ChatRoom.ChatBot.Domain.Bots;
using System;
using Xunit;

namespace ChatRoom.XUnitTest
{
    public class StockBot_Test
    {

        private readonly IBotBase _stockBot;

        public StockBot_Test()
        {
            _stockBot = new StockBot(
                "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv",
                "{0} quote is ${1} per share",
                "Stock Symbol: {0} not found");
        }

        [Fact]
        public void StockBotNameShouldBe()
        {
            var result = _stockBot.BotCommandName;

            Assert.Matches("^stock", result);
        }

        [Theory]
        [InlineData("/stock=AAAA.AA")]
        [InlineData("/stock=aaaa1")]
        public void StockBotVerifyCommandName(string value)
        {
            var result = _stockBot.VerifyCommandName(value);

            Assert.True(result, $"{value} should be a correct CommandName");
        }

        [Theory]
        [InlineData("/stuck=CCCC.DD")]
        [InlineData("\\stock=CCCC.DD")]
        [InlineData("//stock=CCCC.DD")]
        public void StockBotVerifyCommandNameShouldBeFalse(string value)
        {
            var result = _stockBot.VerifyCommandName(value);

            Assert.False(result, $"{value} should be an incorrect CommandName");
        }

        [Theory]
        [InlineData("/stock=AAPL.US")]
        [InlineData("/stock=AFGH.US")]
        [InlineData("/stock=AGZ.US")]
        [InlineData("/stock=144.HK")]
        [InlineData("/stock=2768.JP")]
        public void StockBotExecuteCommandResultShouldBe(string value)
        {
            var result = _stockBot.ExecuteActions(value);
            Assert.Contains("quote is", result.Message);
        }

        [Theory]
        [InlineData("/stock=APPPL")]
        [InlineData("/stock=XXXXX.XX")]
        [InlineData("/stock=YYYZZZ")]
        [InlineData("/stock=NON.EX1STANT")]
        [InlineData("/stock=FICT.NON")]
        public void StockBotExecuteCommandResultShouldBeUnavailable(string value)
        {
            var result = _stockBot.ExecuteActions(value);
            Assert.Contains("not found", result.Message);
        }
    }
}

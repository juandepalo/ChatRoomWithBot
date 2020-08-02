using ChatRoom.ChatBot.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatRoom.ChatBot.Bots
{
    public class StockBot : IBotBase
    {

        private readonly string _stockUrl;
        private readonly string _stockMsg;
        private readonly string _notFoundMsg;

        public StockBot(string stockUrl, string stockMsg, string notFoundMsg)
        {
            _stockUrl = stockUrl;
            _stockMsg = stockMsg;
            _notFoundMsg = notFoundMsg;
        }
        public string BotName => "StockBot";
        public string BotCommandName => "stock";

        public BotResponse ExecuteActions(string command)
        {
            var argumentsMatch = obtainArgs(command);
            var stockSymbol = argumentsMatch.Groups[1].Value.ToUpper();

            var botResult = runBotActions(stockSymbol);
            if (botResult.CompareTo("N/D") == 0)
            {
                return new BotResponse() { BotName = BotName, Message = string.Format(_notFoundMsg,stockSymbol) };
            }

            return new BotResponse() { BotName = BotName, Message = string.Format(_notFoundMsg, stockSymbol, botResult) };
        }

        private string runBotActions(string stock_code)
        {
            var stockUrl = string.Format(_stockUrl, stock_code);

            string csvTextFile;
            using (HttpClient httpClient = new HttpClient())
            using (var response = httpClient.GetStringAsync(stockUrl))
            {
                csvTextFile = response.Result;
            }

            var lines = csvTextFile.Split('\n');
            var stockData = lines[1].Split(',');
            return stockData[6];
        }

        public bool VerifyCommandName(string command)
        {
            return obtainArgs(command).Success;
        }

        private Match obtainArgs(string command)
        {
            return new Regex(@"^/" + BotCommandName + @"=([\w\.]+)").Match(command);
        }
    }
}

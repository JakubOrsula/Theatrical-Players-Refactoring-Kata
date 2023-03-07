﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private int CalcCost(string playType, Performance perf )
        {
            int result;
            switch (playType) 
            {
                case "tragedy":
                    result = 40000;
                    if (perf.Audience > 30) {
                        result += 1000 * (perf.Audience - 30);
                    }
                    break;
                case "comedy":
                    result = 30000;
                    if (perf.Audience > 20) {
                        result += 10000 + 500 * (perf.Audience - 20);
                    }
                    result += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + playType);
            }

            return result;
        }
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayId];
                var thisAmount = CalcCost(play.Type, perf);
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }
    }
}

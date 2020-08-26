using System;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Common
{
    public static class Extensions
    {
        public static string ToLocalDisplayText(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToLocalTime().ToString(DisplayFormat.TransactionDateTime);
        }

        public static string ToString2Decimals(this decimal number)
        {
            return number.ToString("N2").Replace(".00", "");
        }

        public static string ToString2Decimals(this float number)
        {
            return number.ToString("N2").Replace(".0000", "");
        }

        public static string ToPercentage2Decimals(this float number)
        {
            return (number * 100).ToString("N2").Replace(".0000", "");
        }

        public static string ToString4Decimals(this decimal number)
        {
            return number.ToString("N4").Replace(".0000", "");
        }

        public static string ToString4Decimals(this float number)
        {
            return number.ToString("N4").Replace(".0000", "");
        }

        public static MarkupString ToRawHtml(this string html)
        {
            return (MarkupString)html;
        }

        public static string ToStringFriendly(this TimeSpan duration)
        {
            int years = duration.Days / 365; //no leap year accounting
            int months = (duration.Days % 365) / 30; //naive guess at month size
            int weeks = ((duration.Days % 365) % 30) / 7;
            int days = (((duration.Days % 365) % 30) % 7);

            StringBuilder sb = new StringBuilder();

            if (years > 0)
                sb.Append($"{years}y, ");

            if (months > 0)
                sb.Append($"{months}m, ");

            if (weeks > 0)
                sb.Append($"{weeks}w, ");

            if (days > 0)
                sb.Append($"{days}d");
            else
                sb.Append("< 1d");

            return sb.ToString();
        }
    }
}
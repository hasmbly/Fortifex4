﻿namespace Fortifex4.Application.Common.Interfaces.Dogecoin
{
    public class DogecoinTransaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }
        public string Hash { get; set; }
        public long TimeStamp { get; set; }
    }
}
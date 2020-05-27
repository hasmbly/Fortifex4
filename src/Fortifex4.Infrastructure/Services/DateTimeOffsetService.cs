using Fortifex4.Application.Common.Interfaces;
using System;

namespace Fortifex4.Infrastructure.Services
{
    public class DateTimeOffsetService : IDateTimeOffsetService
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
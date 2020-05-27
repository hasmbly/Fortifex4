using System;

namespace Fortifex4.Application.Common.Interfaces
{
    public interface IDateTimeOffsetService
    {
        DateTimeOffset Now { get; }
    }
}
using System;

namespace Fortifex4.Domain.Common
{
    public class AuditableEntity
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }
}
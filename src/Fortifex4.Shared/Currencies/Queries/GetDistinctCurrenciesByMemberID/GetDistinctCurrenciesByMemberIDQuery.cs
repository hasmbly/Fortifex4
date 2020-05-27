using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDQuery : IRequest<GetDistinctCurrenciesByMemberIDResult>
    {
        public string MemberUsername { get; set; }
    }
}
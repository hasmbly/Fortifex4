using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Email;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Contributors.Commands.CreateContributors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Contributors.Commands.CreateContributors
{
    public class CreateContributorsCommandHandler : IRequestHandler<CreateContributorsRequest, CreateContributorsResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IEmailService _emailService;
        private readonly ICurrentWeb _currentWeb;

        public CreateContributorsCommandHandler(IFortifex4DBContext context, IEmailService emailService, ICurrentWeb currentWeb)
        {
            _context = context;
            _emailService = emailService;
            _currentWeb = currentWeb;
        }

        public async Task<CreateContributorsResponse> Handle(CreateContributorsRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateContributorsResponse();

            foreach (var candidate in request.MemberUsername)
            {
                var invitedContributor = await _context.Contributors
                    .Where(x => x.MemberUsername == candidate && x.ProjectID == request.ProjectID)
                    .FirstOrDefaultAsync(cancellationToken);

                if (invitedContributor == null)
                {
                    var contributor = new Contributor
                    {
                        MemberUsername = candidate,
                        ProjectID = request.ProjectID,
                        InvitationCode = Guid.NewGuid(),
                        InvitationStatus = InvitationStatus.Invited
                    };

                    await _context.Contributors.AddAsync(contributor);

                    ContributorDTO contributorDTO = new ContributorDTO
                    {
                        MemberUsername = contributor.MemberUsername,
                        ProjectID = contributor.ProjectID,
                        InvitationCode = contributor.InvitationCode
                    };

                    result.Contributors.Add(contributorDTO);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var contributorDTO in result.Contributors)
            {
                StringBuilder emailBodyStringBuilder = new StringBuilder();
                emailBodyStringBuilder.AppendLine("Silahkan klik link di bawah ini untuk Accept atau Reject");
                emailBodyStringBuilder.AppendLine("<br />");
                emailBodyStringBuilder.AppendLine($"Username Anda: {contributorDTO.MemberUsername} : {contributorDTO.InvitationCode}");
                emailBodyStringBuilder.AppendLine("<br />");
                emailBodyStringBuilder.AppendLine("<div>");
                emailBodyStringBuilder.AppendLine($"<a href=\"{_currentWeb.BaseURL}/projects/accept?invitationCode={contributorDTO.InvitationCode}\">Accept</a>");
                emailBodyStringBuilder.AppendLine($"<a href=\"{_currentWeb.BaseURL}/projects/reject?invitationCode={contributorDTO.InvitationCode}\">Reject</a>");
                emailBodyStringBuilder.AppendLine("</div>");

                EmailItem mailItem = new EmailItem
                {
                    ToAdress = "fuady@fortifex.com",
                    Subject = "Welcome to Fortifex",
                    Body = emailBodyStringBuilder.ToString()
                };

                await _emailService.SendEmailAsync(mailItem);
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
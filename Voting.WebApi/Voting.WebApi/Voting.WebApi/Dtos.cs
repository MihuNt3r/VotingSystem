using System.ComponentModel.DataAnnotations;
using VotingWebApi.Enums;

namespace VotingWebApi.Dtos
{
    public record VotingDto(int Id, string Name, string Description, VotingState state, IEnumerable<ProposalDto> Proposals);

    public record ProposalDto(string Id, string Proposal, int VotesCount);

    public record CreateVotingDto([Required] string Name, string Description, [Required] IEnumerable<CreateProposalDto> Proposals);

    public record CreateProposalDto(string Id, string Proposal);


}

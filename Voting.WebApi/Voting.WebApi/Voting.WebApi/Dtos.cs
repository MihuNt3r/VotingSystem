using System.ComponentModel.DataAnnotations;
using VotingWebApi.Enums;

namespace VotingWebApi.Dtos
{
    public record VotingDto(int Id, string Name, string Description, VotingState state, IEnumerable<ProposalDto> Proposals);

    public record CreateVotingDto([Required] int Id, [Required] string Name, string Description, [Required] IEnumerable<ProposalDto> Proposals);

    public record ProposalDto(int Id, string Proposal, int VoteCounts);

}

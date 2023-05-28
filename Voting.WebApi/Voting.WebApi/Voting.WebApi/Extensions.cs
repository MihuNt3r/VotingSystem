using VotingWebApi.Dtos;
using VotingWebApi.Entities;

namespace VotingWebApi
{
    public static class Extensions
    {
        public static VotingDto AsDto(this Voting voting)
        {
            var votingProposalsDtos = voting.Proposals.Select(proposal => new ProposalDto(proposal.Id, proposal.Proposal, proposal.VoteCount));

            return new VotingDto(
                voting.Id,
                voting.Name,
                voting.Description,
                voting.State,
                votingProposalsDtos
                );
        }
    }
}

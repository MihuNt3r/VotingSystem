using VotingWebApi.Enums;

namespace VotingWebApi.Entities
{
    public class Voting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public VotingState State { get; set; }

        public IEnumerable<VotingProposal> Proposals { get; set; }
    }
}

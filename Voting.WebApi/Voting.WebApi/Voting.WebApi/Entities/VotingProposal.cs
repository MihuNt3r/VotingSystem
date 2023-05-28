namespace VotingWebApi.Entities
{
    public class VotingProposal
    {
        public string Id { get; set; }

        public string Proposal { get; set; }

        public int VoteCount { get; set; }
    }
}

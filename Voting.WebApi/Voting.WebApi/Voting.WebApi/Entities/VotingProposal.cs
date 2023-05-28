namespace VotingWebApi.Entities
{
    public class VotingProposal
    {
        public int Id { get; set; }

        public string Proposal { get; set; }

        public int VoteCount { get; set; }
    }
}

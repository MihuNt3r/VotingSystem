using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


namespace VotingWebApi.SmartContractQueries
{
    [Function("voteCountByProposal", "uint256")]
    public class VoteCountByProposalFunction : FunctionMessage
    {
        [Parameter("string", "", 1)] public string IdProposal { get; set; }
    }
}

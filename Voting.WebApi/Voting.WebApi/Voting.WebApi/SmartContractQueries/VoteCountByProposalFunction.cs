using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


namespace VotingWebApi.SmartContractQueries
{
    [Function("voteCountByProposal", "uint256")]
    public class VoteCountByProposalFunction : FunctionMessage
    {
        [Parameter("uint256", "", 1)] public int IdProposal { get; set; }
    }
}

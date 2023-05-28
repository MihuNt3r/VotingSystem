using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using VotingWebApi.SmartContractDtos;
using System.Numerics;

namespace VotingWebApi.SmartContractQueries
{
    [Function("getVotingInfo", typeof(VotingStatusDto))]
    public class VotingStatusFunction : FunctionMessage
    {
        [Parameter("uint256", "id", 1)] public int IdVoting { get; set; }
    }
}

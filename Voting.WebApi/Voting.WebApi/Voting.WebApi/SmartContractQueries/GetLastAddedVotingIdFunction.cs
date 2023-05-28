using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace VotingWebApi.SmartContractQueries
{
    [Function("getLastAddedVotingId", "uint256")]
    public class GetLastAddedVotingIdFunction : FunctionMessage
    {
    }
}

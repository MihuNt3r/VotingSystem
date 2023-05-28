using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace VotingWebApi.SmartContractDtos
{
    [FunctionOutput]
    public class VotingStatusDto : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public BigInteger VotingId { get; set; }
        [Parameter("uint8", "", 2)]
        public byte State { get; set; }
        [Parameter("int256", "", 3)]
        public BigInteger WinningProposal { get; set; }
    }
}

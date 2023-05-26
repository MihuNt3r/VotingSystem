using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using System.Numerics;

namespace Voting.WebApi.Controllers
{
    [ApiController]
    [Route("api/votings")]
    public class VotingsController : ControllerBase
    {
        [HttpGet("{ethAddress}/balance")]
        public async Task<ActionResult<BigInteger>> GetWalletBalance([FromRoute] string ethAddress)
        {
            var web3 = new Web3("https://mainnet.infura.io/v3/ccd658afb4ff4d04b1891723bae0fb49");

            var balance = await web3.Eth.GetBalance.SendRequestAsync(ethAddress);

            return Ok(balance.Value);
        }

        [HttpGet("")]
        public async Task<ActionResult> GetVotings()
        {
            return Ok();
        }

        [HttpGet("{idVoting}")]
        public async Task<ActionResult> GetVoting([FromRoute] int idVoting)
        {
            return Ok();
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateVoting()
        {
            return Ok();
        }

        [HttpPost("{idVoting}/vote")]
        public async Task<ActionResult> Vote([FromRoute] int idVoting)
        {
            return Ok();
        }

        [HttpDelete("[idVoting]")]
        public async Task<ActionResult> Delete([FromRoute] int idVoting)
        {
            return Ok();
        }
    }
}

using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using VotingWebApi.Dtos;
using VotingWebApi.Enums;
using VotingWebApi.Entities;
using VotingWebApi.Repositories;
using VotingWebApi.SmartContractDtos;
using VotingWebApi.SmartContractQueries;

namespace VotingWebApi.Controllers
{
    [ApiController]
    [Route("api/votings")]
    public class VotingsController : ControllerBase
    {
        private readonly VotingsRepository votingsRepository;
        private readonly Web3 web3;
        private const string CONTRACT_ADDRESS = "0xC541a6816B3cFb3a2bD9EF99Bff629328dA8b16d";

        public VotingsController()
        {
            votingsRepository= new VotingsRepository();
            web3 = new Web3("https://sepolia.infura.io/v3/ccd658afb4ff4d04b1891723bae0fb49");
        }

        [HttpGet("{ethAddress}/balance")]
        public async Task<ActionResult<BigInteger>> GetWalletBalance([FromRoute] string ethAddress)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(ethAddress);

            return Ok(balance.Value);
        }

        [HttpGet("{idProposal}/votes")]
        public async Task<ActionResult<int>> GetVotesByProposal([FromRoute] int idProposal)
        {
            var web3 = new Web3("https://sepolia.infura.io/v3/ccd658afb4ff4d04b1891723bae0fb49");

            var voteCountMessage = new VoteCountByProposalFunction
            {
                IdProposal = idProposal
            };

            var queryHandler = web3.Eth.GetContractQueryHandler<VoteCountByProposalFunction>();

            var voteCount = await queryHandler
                .QueryAsync<int>(CONTRACT_ADDRESS, voteCountMessage)
                .ConfigureAwait(false);

            return Ok(voteCount);
        }

        [HttpGet("{idVoting}/state")]
        public async Task<ActionResult<int>> GetVoting([FromRoute] int idVoting)
        {
            var web3 = new Web3("https://sepolia.infura.io/v3/ccd658afb4ff4d04b1891723bae0fb49");

            var votingStatusMessage = new VotingStatusFunction
            {
                IdVoting = idVoting
            };

            var queryHandler = web3.Eth.GetContractQueryHandler<VotingStatusFunction>();

            var votingInfo = await queryHandler
                .QueryDeserializingToObjectAsync<VotingStatusDto>(votingStatusMessage, CONTRACT_ADDRESS)
                .ConfigureAwait(false);

            var votingId = (int)votingInfo.VotingId;
            var winningProposal = (int)votingInfo.WinningProposal;
            var state = (VotingState)votingInfo.State;

            return Ok(2);
            //return Ok(voteCount);
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<VotingDto>>> GetVotings()
        {
            var votings = (await votingsRepository.GetAllEnabledVotings())
                          .Select(voting => voting.AsDto());

            return Ok(votings);
        }

        [HttpGet("{idVoting}")]
        public async Task<ActionResult<VotingDto>> GetVotingById([FromRoute] int idVoting)
        {
            var voting = await votingsRepository.GetById(idVoting);

            if (voting == null)
            {
                return NotFound();
            }

            return Ok(voting.AsDto());
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateVoting([FromBody] CreateVotingDto createVotingDto)
        {
            var voting = new Voting
            {
                Id = createVotingDto.Id,
                Name = createVotingDto.Name,
                Description = createVotingDto.Description,
                IsEnabled = true,
                State = VotingState.Started,
                Proposals = createVotingDto.Proposals
                                           .Select(proposal => new VotingProposal
                                           {
                                               Id = proposal.Id,
                                               Proposal = proposal.Proposal,
                                               VoteCount = 0
                                           })
            };

            await votingsRepository.Create(voting);

            return CreatedAtAction(nameof(GetVotingById), new { idVoting = voting.Id }, voting);
        }

        [HttpPost("{idVoting}/vote/{idProposal}")]
        public async Task<ActionResult> Vote([FromRoute] int idVoting, [FromRoute] int idProposal)
        {
            return Ok();
        }

        [HttpPost("{idVoting}/end")]
        public async Task<ActionResult> EndVoting([FromRoute] int idVoting)
        {
            return Ok();
        }

        [HttpPost("{idVoting}/disable")]
        public async Task<ActionResult> DisableVoting([FromRoute] int idVoting)
        {
            var votingToDisable = await votingsRepository.GetById(idVoting);

            if (votingToDisable == null)
            {
                return NotFound();
            }

            votingToDisable.IsEnabled = false;

            await votingsRepository.Update(votingToDisable);

            return NoContent();
        }
    }
}

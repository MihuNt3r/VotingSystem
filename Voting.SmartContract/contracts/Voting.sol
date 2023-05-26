// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.9;

contract Voting {
    address private owner;
    VotingState private votingState;
    VotingProposal[] private votingProposals;
    uint256[] public winners;
    uint private duration;
    uint private deadline;
    uint constant DEFAULT_DURATION = 15 minutes;

    enum VotingState {
        Started,
        InProgress,
        FinishedWithWinner,
        Draw,
        NotEnoughVotes
    }
    struct VotingProposal {
        uint proposalId;
        string name;
        uint voteCount;
    }

    constructor(uint _duration, string[] memory proposalNames) {
        owner = msg.sender;
        duration = _duration == 0 ? DEFAULT_DURATION : _duration;
        deadline = block.timestamp + duration * 60;
        votingState = VotingState.Started;

        for (uint i = 0; i < proposalNames.length; i++) {
            votingProposals.push(
                VotingProposal({
                    proposalId: i + 1,
                    name: proposalNames[i],
                    voteCount: 0
                })
            );
        }
    }

    function getWinnerId() public view returns (uint256) {
        require(
            votingState == VotingState.FinishedWithWinner,
            "Voting isn't finished or there is more than one winner"
        );

        return winners[0];
    }

    function getWinnersIds() public view returns (uint256[] memory) {
        require(
            votingState == VotingState.FinishedWithWinner,
            "Voting isn't finished or there is more than one winner"
        );
        return winners;
    }
}

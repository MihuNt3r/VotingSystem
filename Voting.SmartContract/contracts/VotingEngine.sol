// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.9;

contract VotingEngine {
    enum VotingState {
        Started,
        HasLeader,
        Draw,
        FinishedWithWinner,
        FinishedWithDraw
    }

    struct Voting {
        uint votingId;
        uint[] proposalIds;
        VotingState state;
        int winningProposal;
        uint winningVoteCount;
    }

    uint public votingsNumber;
    Voting[] public votings;
    mapping(uint => uint) public voteCountByProposal;

    function createVoting(
        uint[] memory proposalIds
    ) public payable returns (uint _id) {
        require(proposalIds.length > 0, "Proposals array can't be empty");

        uint votingId = votingsNumber++;

        votings.push(
            Voting({
                votingId: votingId,
                proposalIds: proposalIds,
                state: VotingState.Started,
                winningProposal: -1,
                winningVoteCount: 0
            })
        );

        for (uint i = 0; i < proposalIds.length; i++) {
            uint proposalId = proposalIds[i];
            voteCountByProposal[proposalId] = 0;
        }

        return votingId;
    }

    function vote(uint votingId, uint proposalId) public payable {
        Voting storage voting = votings[votingId];
        require(
            voting.state != VotingState.FinishedWithWinner ||
                voting.state != VotingState.FinishedWithDraw,
            "Voting is already finished"
        );

        uint currentVotesCount = voteCountByProposal[proposalId];

        uint newVotesCount = currentVotesCount + 1;
        voteCountByProposal[proposalId] = newVotesCount;

        if (newVotesCount > voting.winningVoteCount) {
            voting.winningProposal = int(proposalId);
            voting.winningVoteCount = newVotesCount;
            voting.state = VotingState.HasLeader;
        } else if (newVotesCount == voting.winningVoteCount) {
            voting.winningProposal = -1; // Multiple winners
            voting.state = VotingState.Draw;
        }
    }

    // Maybe it's better to have list of ids in MongoDb
    function getVotingsList() public view returns (uint[] memory) {
        uint[] memory votingsIds = new uint[](votings.length);

        for (uint i = 0; i < votings.length; i++) {
            Voting memory v = votings[i];

            votingsIds[i] = v.votingId;
        }

        return votingsIds;
    }

    function getVotingInfo(
        uint id
    ) public view returns (uint, uint[] memory, VotingState, int) {
        Voting memory voting = votings[id];

        uint votingId = voting.votingId;
        uint[] memory proposalIds = voting.proposalIds;
        VotingState state = voting.state;
        int winningProposal = voting.winningProposal;

        return (votingId, proposalIds, state, winningProposal);
    }

    // Maybe we can return winning proposal here
    function endVoting(uint idVoting) public payable {
        Voting storage voting = votings[idVoting];

        require(
            voting.state != VotingState.Started,
            "Can't finish voting because no one voted yet"
        );

        if (voting.winningProposal != -1) {
            voting.state = VotingState.FinishedWithWinner;
        } else {
            voting.state = VotingState.FinishedWithDraw;
        }
    }

    function getWinningProposal(uint idVoting) public view returns (int) {
        Voting memory voting = votings[idVoting];

        // Can bring to method voting.isFinished
        require(
            voting.state == VotingState.FinishedWithWinner ||
                voting.state == VotingState.FinishedWithDraw,
            "Voting is not finished yet"
        );

        return voting.winningProposal;
    }
}

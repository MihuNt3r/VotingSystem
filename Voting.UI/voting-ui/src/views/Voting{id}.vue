<template>
  <v-app>
    <v-main>
      <v-app-bar rounded color="purple">
        <template v-slot:prepend>
          <v-icon icon="fas fa-shield" size="42" />
        </template>

        <v-app-bar-title class="text-h5">
          Secure Votings Dapp - {{ voting.name }}
        </v-app-bar-title>

        <template v-slot:append>
          <span v-if="$store.getters.account"
            >Account: {{ $store.getters.account }}</span
          >
        </template>
      </v-app-bar>
      <div v-if="isLoading" style="margin-top: 30vh">
        <div class="text-center">
          <v-progress-circular
            color="purple"
            indeterminate
            :size="70"
            :width="5"
          />
        </div>
      </div>
      <v-row v-else justify="center">
        <v-col cols="12" class="text-center mt-5 text-h5">
          Voting Description: {{ voting.description }}
        </v-col>
        <v-col cols="3" v-for="proposal in voting.proposals" :key="proposal.id">
          <v-card class="mx-auto" max-width="344" variant="outlined">
            <v-card-item>
              <div>
                <div class="text-h6 mb-1 text-center">
                  {{ proposal.proposal }}
                </div>
              </div>
            </v-card-item>

            <v-card-actions>
              <v-row justify="space-between">
                <v-spacer />
                <v-btn
                  variant="outlined"
                  color="purple"
                  @click="vote(proposal.id)"
                  :loading="idProposalBeingVoted === proposal.id"
                >
                  Vote
                </v-btn>
                <v-spacer />
                <div class="mt-2">Vote Count: {{ proposal.votesCount }}</div>
                <v-spacer />
              </v-row>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-main>
  </v-app>
</template>

<script>
import { ethers } from "ethers";
import abi from "../../utils/VotingEngine.json";

const axios = require("axios");

export default {
  name: "VotingsTable",
  data: () => ({
    voting: {},
    contractAddress: "0x74482b8FF7893dEC5Bd370637F656bE0A5025749",
    contractAbi: abi.abi,
    isLoading: true,
    idProposalBeingVoted: null,
  }),
  async created() {
    await this.loadData();
  },
  methods: {
    async vote(idProposal) {
      this.idProposalBeingVoted = idProposal;

      try {
        const { ethereum } = window;

        if (ethereum) {
          const provider = new ethers.providers.Web3Provider(ethereum, "any");
          const signer = provider.getSigner();
          const votingContract = new ethers.Contract(
            this.contractAddress,
            this.contractAbi,
            signer
          );

          const idVoting = Number(this.idVoting);

          const voteTxn = await votingContract.vote(idVoting, idProposal);

          console.log({ voteTxn });

          await voteTxn.wait();

          console.log("success");

          await axios.post(
            `http://localhost:21682/api/votings/${this.idVoting}/vote/${idProposal}`
          );

          await this.loadData();
        }
      } finally {
        this.idProposalBeingVoted = null;
      }
    },
    async loadData() {
      this.isLoading = true;
      try {
        const response = await axios.get(
          `http://localhost:21682/api/votings/${this.idVoting}`
        );

        this.voting = response.data;
        console.log(this.voting);
      } finally {
        this.isLoading = false;
      }
    },
  },
  computed: {
    idVoting() {
      return this.$route.params.idVoting;
    },
  },
};
</script>

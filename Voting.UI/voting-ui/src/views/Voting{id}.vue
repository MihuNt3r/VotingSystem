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
              <v-row>
                <v-col>
                  <v-spacer />
                </v-col>
                <v-col>
                  <v-btn
                    variant="filled"
                    color="purple"
                    size="small"
                    @click="proposal.isBeingChecked = true"
                  >
                    Check fairness

                    <v-dialog v-model="proposal.isBeingChecked" width="auto">
                      <v-card>
                        <v-card-text>
                          To check fairness:
                          <br />
                          1.Click this button:
                          <v-btn
                            variant="filled"
                            href="https://sepolia.etherscan.io/address/0xc2227eddb18421241bc2eaff9c7273830b6b614f#readContract"
                            target="_blank"
                            size="small"
                          >
                            Smart contract on Etherscan
                          </v-btn>
                          <br />
                          2.Select section Contract, then select section Read
                          Contract
                          <br />
                          3.Find function named voteCountByIdProposal and call
                          it passing this unique Id:
                          {{ proposal.id }}
                          <br />
                          4. You can ensure that vote count in blockchain
                          matches current vote count
                        </v-card-text>
                        <v-card-actions>
                          <v-btn
                            color="primary"
                            block
                            @click="proposal.isBeingChecked = false"
                            >Close</v-btn
                          >
                        </v-card-actions>
                      </v-card>
                    </v-dialog>
                  </v-btn>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <div class="text-h6 mb-1 text-center">
                    {{ proposal.proposal }}
                  </div>
                </v-col>
              </v-row>
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
    contractAddress: "0xc2227eddb18421241bc2eaff9c7273830b6b614f",
    contractAbi: abi.abi,
    isLoading: true,
    idProposalBeingVoted: null,
    idProposalBeingChecked: null,
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

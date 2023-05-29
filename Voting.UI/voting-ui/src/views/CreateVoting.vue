<template>
  <v-app>
    <v-main>
      <v-app-bar rounded color="purple">
        <template v-slot:prepend>
          <v-icon icon="fas fa-shield" size="42" />
        </template>

        <v-app-bar-title class="text-h5">
          Secure Votings Dapp - New Voting
        </v-app-bar-title>

        <template v-slot:append>
          <span v-if="$store.getters.account"
            >Account: {{ $store.getters.account }}</span
          >
        </template>
      </v-app-bar>
      <v-row justify="center" class="mt-3">
        <v-col cols="5">
          <v-text-field
            v-model="votingName"
            label="Voting Name"
            variant="outlined"
            color="purple"
            hide-details
            dense
          ></v-text-field>
        </v-col>
        <v-col cols="5">
          <v-text-field
            v-model="votingDescription"
            label="Voting Description"
            variant="outlined"
            color="purple"
            hide-details
            dense
          ></v-text-field>
        </v-col>
        <v-col cols="11">
          <v-card elevation="8">
            <v-row justify="center" class="mb-3 mt-1 text-center">
              <v-col cols="12">
                <v-card-title> Proposals</v-card-title>
              </v-col>
              <v-col
                v-for="(proposal, index) in proposals"
                :key="index"
                cols="9"
              >
                <v-text-field
                  v-model="proposal.name"
                  label="Proposal"
                  variant="outlined"
                  color="purple"
                  hide-details
                  dense
                  append-icon="fas fa-trash"
                  @click:append="deleteProposal(proposal.id)"
                ></v-text-field>
              </v-col>
              <v-col cols="9">
                <v-btn
                  icon="fas fa-plus"
                  color="purple"
                  @click="addProposal"
                ></v-btn>
              </v-col>
            </v-row>
          </v-card>
        </v-col>
        <v-col cols="12" class="text-center">
          <v-btn
            color="purple"
            rounded="xl"
            size="large"
            @click="createVoting"
            :loading="createButtonLoading"
          >
            Create Voting
          </v-btn>
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
  name: "CreateVoting",
  components: {
    // HelloWorld,
  },
  data: () => ({
    votingName: "",
    votingDescription: "",
    proposals: [],
    contractAddress: "0x74482b8FF7893dEC5Bd370637F656bE0A5025749",
    contractAbi: abi.abi,
    createButtonLoading: false,
  }),
  methods: {
    addProposal() {
      this.proposals.push({ id: this.uuidv4(), name: "" });
    },
    deleteProposal(proposalId) {
      this.proposals = this.proposals.filter(
        (proposal) => proposal.id !== proposalId
      );
    },
    async createVoting() {
      try {
        this.createButtonLoading = true;
        const { ethereum } = window;

        if (ethereum) {
          const provider = new ethers.providers.Web3Provider(ethereum, "any");
          const signer = provider.getSigner();
          const votingContract = new ethers.Contract(
            this.contractAddress,
            this.contractAbi,
            signer
          );

          console.log(votingContract);

          const proposalIds = this.proposals.map((p) => p.id);

          console.log(proposalIds);

          const createVotingTxn = await votingContract.createVoting(
            proposalIds
          );

          console.log({ createVotingTxn });

          const idVoting = await createVotingTxn.wait();

          console.log("success", idVoting);

          const model = {
            name: this.votingName,
            description: this.votingDescription,
            proposals: this.proposals.map((proposal) => ({
              id: proposal.id,
              proposal: proposal.name,
            })),
          };

          console.log(model);

          await axios.post("http://localhost:21682/api/votings", model);

          this.$router.back();
        }
      } catch (error) {
        console.log(error);
      } finally {
        this.createButtonLoading = false;
      }
    },
    uuidv4() {
      return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
        (
          c ^
          (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
        ).toString(16)
      );
    },
  },
};
</script>

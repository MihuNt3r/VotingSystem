<template>
  <v-app>
    <v-main>
      <v-app-bar rounded color="purple">
        <template v-slot:prepend>
          <v-icon icon="fas fa-shield" size="42" />
        </template>

        <v-app-bar-title class="text-h5">Secure Votings Dapp</v-app-bar-title>

        <template v-slot:append>
          <span v-if="$store.getters.account"
            >Account: {{ $store.getters.account }}</span
          >
        </template>
      </v-app-bar>
      <v-row justify="center" class="mt-3">
        <v-btn
          v-if="!$store.getters.account"
          @click="connectWallet"
          color="purple"
          rounded="xl"
          size="x-large"
          style="margin-top: 30vh"
        >
          Connect Wallet
        </v-btn>
        <v-col v-else cols="12">
          <VotingsTable />
        </v-col>
      </v-row>
    </v-main>
  </v-app>
</template>
<script>
import { ethers } from "ethers";
import abi from "../../utils/VotingEngine.json";

import VotingsTable from "@/components/votings-table.vue";

export default {
  name: "VotingsList",

  components: {
    VotingsTable,
  },

  data: () => ({
    currentAccount: null,
    contractAddress: "0x74482b8FF7893dEC5Bd370637F656bE0A5025749",
    contractAbi: abi.abi,
  }),
  methods: {
    async connectWallet() {
      console.log("started connecting wallet");

      try {
        const { ethereum } = window;

        if (!ethereum) {
          console.log("please install MetaMask");
        }

        const accounts = await ethereum.request({
          method: "eth_requestAccounts",
        });

        const [account] = accounts;

        this.currentAccount = account;

        this.$store.dispatch("setAccount", account);
      } catch (error) {
        console.log(error);
      }
    },

    async createVoting() {
      try {
        const { ethereum } = window;

        if (ethereum) {
          // put this in created lifecycle hook
          const provider = new ethers.providers.Web3Provider(ethereum, "any");
          const signer = provider.getSigner();
          const votingContract = new ethers.Contract(
            this.contractAddress,
            this.contractAbi,
            signer
          );

          console.log(votingContract);

          const createVotingTxn = await votingContract.createVoting([1, 2, 3]);

          console.log({ createVotingTxn });

          await createVotingTxn.wait();

          console.log("success");
        }
      } catch (error) {
        console.log(error);
      }
    },

    async logFirstVotingInfo() {
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

          console.log("fetching first voting from the blockchain..");

          const voting = await votingContract.getVotingInfo(0);

          console.log({ voting });
          console.log("fetched!");
        }
      } catch (error) {
        console.log(error);
      }
    },

    async voteFor3Proposal() {
      const { ethereum } = window;

      if (ethereum) {
        const provider = new ethers.providers.Web3Provider(ethereum, "any");
        const signer = provider.getSigner();
        const votingContract = new ethers.Contract(
          this.contractAddress,
          this.contractAbi,
          signer
        );

        const voteTxn = await votingContract.vote(0, 3);

        console.log({ voteTxn });

        await voteTxn.wait();

        console.log("success");
      }
    },
  },
};
</script>

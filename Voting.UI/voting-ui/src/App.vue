<template>
  <v-app>
    <v-main>
      <v-app-bar rounded color="purple">
        <template v-slot:prepend>
          <v-icon icon="fas fa-shield" size="42" />
        </template>

        <v-app-bar-title class="text-h5">Secure Votings Dapp</v-app-bar-title>

        <template v-slot:append>
          <span v-if="currentAccount">Account: {{ currentAccount }}</span>
        </template>
      </v-app-bar>
      <v-row justify="center" class="mt-3">
        <v-col cols="12">
          <v-btn
            v-if="!currentAccount"
            @click="connectWallet"
            color="purple"
            rounded="xl"
            size="x-large"
          >
            Connect Wallet
          </v-btn>
          <v-data-table
            v-else
            v-model:items-per-page="itemsPerPage"
            :headers="headers"
            :items="desserts"
            item-value="name"
            class="elevation-1"
          ></v-data-table>
        </v-col>
        <v-col cols="2">
          <v-btn @click="voteFor3Proposal" color="purple" rounded>
            Vote for 3 Proposal
          </v-btn>
        </v-col>
        <v-col cols="2">
          <v-btn @click="logFirstVotingInfo" color="purple" rounded>
            Log first voting info</v-btn
          >
        </v-col>
      </v-row>
    </v-main>
  </v-app>
</template>

<script>
import { ethers } from "ethers";
import abi from "../utils/VotingEngine.json";

export default {
  name: "App",

  //   components: {
  //     VDataTable,
  //   },

  data: () => ({
    currentAccount: null,
    contractAddress: "0x84d26fFaB939b0AF567E27e55fA3B3d2FAC9dB8b",
    contractAbi: abi.abi,
    itemsPerPage: 5,
    headers: [
      {
        title: "Dessert (100g serving)",
        align: "start",
        sortable: false,
        key: "name",
      },
      { title: "Calories", align: "end", key: "calories" },
      { title: "Fat (g)", align: "end", key: "fat" },
      { title: "Carbs (g)", align: "end", key: "carbs" },
      { title: "Protein (g)", align: "end", key: "protein" },
      { title: "Iron (%)", align: "end", key: "iron" },
    ],
    desserts: [
      {
        name: "Frozen Yogurt",
        calories: 159,
        fat: 6.0,
        carbs: 24,
        protein: 4.0,
        iron: "1",
      },
      {
        name: "Jelly bean",
        calories: 375,
        fat: 0.0,
        carbs: 94,
        protein: 0.0,
        iron: "0",
      },
      {
        name: "KitKat",
        calories: 518,
        fat: 26.0,
        carbs: 65,
        protein: 7,
        iron: "6",
      },
      {
        name: "Eclair",
        calories: 262,
        fat: 16.0,
        carbs: 23,
        protein: 6.0,
        iron: "7",
      },
      {
        name: "Gingerbread",
        calories: 356,
        fat: 16.0,
        carbs: 49,
        protein: 3.9,
        iron: "16",
      },
      {
        name: "Ice cream sandwich",
        calories: 237,
        fat: 9.0,
        carbs: 37,
        protein: 4.3,
        iron: "1",
      },
      {
        name: "Lollipop",
        calories: 392,
        fat: 0.2,
        carbs: 98,
        protein: 0,
        iron: "2",
      },
      {
        name: "Cupcake",
        calories: 305,
        fat: 3.7,
        carbs: 67,
        protein: 4.3,
        iron: "8",
      },
      {
        name: "Honeycomb",
        calories: 408,
        fat: 3.2,
        carbs: 87,
        protein: 6.5,
        iron: "45",
      },
      {
        name: "Donut",
        calories: 452,
        fat: 25.0,
        carbs: 51,
        protein: 4.9,
        iron: "22",
      },
    ],
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

          const createVotingTxn = await votingContract.createVoting([
            1, 2, 3, 4, 5,
          ]);

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

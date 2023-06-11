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
import abi from "../../utils/VotingEngine.json";

import VotingsTable from "@/components/votings-table.vue";

export default {
  name: "VotingsList",

  components: {
    VotingsTable,
  },

  data: () => ({
    contractAddress: "0xc2227eddb18421241bc2eaff9c7273830b6b614f",
    contractAbi: abi.abi,
  }),
  methods: {
    async connectWallet() {
      try {
        const { ethereum } = window;

        if (!ethereum) {
          console.log("please install MetaMask");
        }

        const accounts = await ethereum.request({
          method: "eth_requestAccounts",
        });

        const [account] = accounts;

        this.$store.dispatch("setAccount", account);
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>

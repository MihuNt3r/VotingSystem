<template>
  <v-row justify="center" class="mt-3">
    <v-col cols="11"> {{ idVoting }} </v-col>
  </v-row>
</template>

<script>
const axios = require("axios");

export default {
  name: "VotingsTable",
  components: {
    // HelloWorld,
  },
  data: () => ({
    votings: [],
  }),
  async created() {
    const response = await axios.get("http://localhost:21682/api/votings");

    this.votings = response.data;
    console.log(this.votings);
  },
  methods: {
    logItem(item) {
      console.log(item);
    },
    createVoting() {
      this.$router.push({ path: "/newVoting" });
    },
  },
  computed: {
    idVoting() {
      return this.$route.params.idVoting;
    },
  },
};
</script>

<template>
  <v-row justify="center" class="mt-3">
    <v-col cols="11">
      <v-toolbar color="white">
        <v-spacer />
        <v-btn
          size="x-large"
          icon="fas fa-plus"
          color="purple"
          @click="createVoting"
        ></v-btn>
      </v-toolbar>
      <v-table>
        <thead>
          <tr>
            <th class="text-left">Name</th>
            <th class="text-left">Description</th>
            <th class="text-left">Status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="voting in votings"
            :key="voting.name"
            @click="logItem(voting)"
          >
            <td style="height: 100px">{{ voting.name }}</td>
            <td>{{ voting.description }}</td>
            <td>{{ voting.state }}</td>
            <td><v-btn icon="fas fa-arrow-right" size="large"></v-btn></td>
          </tr>
        </tbody>
      </v-table>
    </v-col>
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
};
</script>

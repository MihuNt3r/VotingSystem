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
            <td></td>
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

<style>
.greenBar {
  position: relative;
  height: 100%;
  width: 15px;
  border-radius: 10px 0 0 10px;
}

.rounded-card {
  border-radius: 20px;
}

.v-data-table__wrapper .table {
  border-collapse: separate;
  border-spacing: 0 15px;
  background: none;
  padding: 0 5px;
}

.v-data-table__wrapper .table .tr {
  box-shadow: 0px 0px 12px #00000033;
  border-radius: 10px 10px 10px 10px !important;
}

.v-data-table__wrapper .table .td {
  border-bottom: 0 none !important;
}
</style>

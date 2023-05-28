<template>
  <router-view />
</template>

<script>
export default {
  name: "HomeView",
  created() {
    document.title = "Secure Votings Dapp";
    this.$router.push({ path: "/votings" });
  },
};
</script>

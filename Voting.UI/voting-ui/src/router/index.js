import { createRouter, createWebHashHistory } from "vue-router";
import Votings from "../views/Votings.vue";
import CreateVoting from "../views/CreateVoting.vue";

const routes = [
  {
    path: "/votings",
    name: "votings",
    component: Votings,
  },
  {
    path: "/newVoting",
    name: "newVoting",
    component: CreateVoting,
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;

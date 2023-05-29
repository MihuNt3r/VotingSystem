import { createRouter, createWebHashHistory } from "vue-router";
import Votings from "../views/Votings.vue";
import CreateVoting from "../views/CreateVoting.vue";
import Voting from "../views/Voting{id}.vue";

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
  {
    path: "/votings/:idVoting",
    name: "voting",
    component: Voting,
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;

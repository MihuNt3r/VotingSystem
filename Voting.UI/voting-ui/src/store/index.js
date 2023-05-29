import { createStore } from "vuex";

export default createStore({
  state: {
    account: null,
  },
  mutations: {
    updateAccount(state, payload) {
      state.account = payload;
    },
  },
  actions: {
    setAccount(context, payload) {
      context.commit("updateAccount", payload);
    },
  },
  getters: {
    account: function (state) {
      return state.account;
    },
  },
});

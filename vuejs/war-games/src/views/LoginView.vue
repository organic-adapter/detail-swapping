<template>
  <div class="login">
    <div v-if="account">
      <div class="level">
        <div class="level-item title">
          You are logged in as {{ account.name }}
        </div>
      </div>
    </div>
    <div v-else>
      You need to authenticate to access
      <button @click="SignIn()">Sign In</button>
    </div>
  </div>
</template>
<script lang="ts">
//derived from https://devblogs.microsoft.com/azure-sdk/vue-js-user-authentication/
import { defineComponent } from "vue";
import { mapActions, mapGetters } from "vuex";
import { PublicClientApplication } from "@azure/msal-browser";
export default defineComponent({
  name: "HeaderBar",
  data() {
    return {
      account: undefined,
      signin: "https://microsoft.com",
    };
  },
  computed: {
    ...mapGetters({
      msalConfig: "msal/msalConfig",
    }),
  },
  async created() {
    await this.getProviderDetails();
  },
  mounted() {
    this.$msalInstance = new PublicClientApplication(this.msalConfig);
    const accounts = this.$msalInstance.getAllAccounts();
    if (accounts.length == 0) {
      return;
    }
    this.account = accounts[0];
    this.$emitter.emit("login", this.account);
  },
  methods: {
    ...mapActions({
      getProviderDetails: "msal/getProviderDetails",
    }),
    async SignIn() {
      await this.$msalInstance.loginPopup({}).then(() => {
        const myAccounts = this.$msalInstance.getAllAccounts();
        this.account = myAccounts[0];
        this.$emitter.emit("login", this.account);
      });
      //.catch(error => {
      //  console.error(`error during authentication: ${error}`);
      //});
    },
    async SignOut() {
      await this.$msalInstance.logout({}).then(() => {
        this.$emitter.emit("logout", "logging out");
      });
      //.catch(error => {
      //  console.error(error);
      //});
    },
  },
});
</script>

import state from "./state";
import getters from "./getters";
import actions from "./actions";
import mutations from "./mutations";

/**
 * Derived from https://devblogs.microsoft.com/azure-sdk/vue-js-user-authentication/
 */
export const msal =
{
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
}
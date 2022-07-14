import Services from '@/assets/services';
export default {
    setAccessToken(state: any, token: string) {
        state.accessToken = token;
    },
    setMsalConfigAuth(state: any, authDetails: any) {
        state.msalConfig.auth = Object.assign(state.msalConfig.auth, authDetails);
    },
    setServices(state: any, services: Services) {
        state.services = services;
    }
}
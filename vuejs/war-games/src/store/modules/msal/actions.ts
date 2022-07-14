export default {
    async getProviderDetails(context: any) {
        const services = context.state.services;
        const details = await services.getProviderDetails();
        context.commit("setMsalConfigAuth", details);
    }
}
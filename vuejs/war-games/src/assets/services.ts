import axios from "axios";

export default class Service {
    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }
    baseUrl:string;

    async getProviderDetails() {
        const response = await axios.get(`${this.baseUrl}/logon/providerDetails`);
        return response.data;
    }
}
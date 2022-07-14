import * as TinyEmitter from 'tiny-emitter';
import { apiConfiguration } from '@/assets/configuration';
import Services from '@/assets/services';
import store from '@/store';

/**
 * Derived from https://devblogs.microsoft.com/azure-sdk/vue-js-user-authentication/
 */
export const defaultGlobals = {
    install(app: any, options: any) {
        const services = new Services(apiConfiguration.baseUrl);
        options.store.commit('msal/setServices', services);

        app.config.globalProperties.$emitter = new TinyEmitter.TinyEmitter();
        app.config.globalProperties.$msalInstance = {};
        app.config.globalProperties.$services = services;
        return app;
    }
}

declare module 'vue' {
    interface ComponentCustomProperties {
        $emitter: TinyEmitter.TinyEmitter
        $msalInstance: any,
        $services: any,
        $store: any
    }
}
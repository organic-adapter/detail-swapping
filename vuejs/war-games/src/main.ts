import { createApp } from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import { defaultGlobals } from './startups/globals'

createApp(App)
    .use(store)
    .use(router)
    .use(defaultGlobals, { store })
    .mount('#app');
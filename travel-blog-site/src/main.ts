import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from './router';
import { faHome } from '@fortawesome/free-solid-svg-icons/faHome';
import { faImage } from '@fortawesome/free-solid-svg-icons/faImage';
import { faList } from '@fortawesome/free-solid-svg-icons/faList';
import { faSort } from '@fortawesome/free-solid-svg-icons/faSort';
import { faSyncAlt } from '@fortawesome/free-solid-svg-icons/faSyncAlt';
import { library} from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import App from './App.vue';

async function main() {
    const bootstrapPromise = import('bootstrap/dist/css/bootstrap.min.css');

    library.add(faHome);
    library.add(faSort);
    library.add(faList);
    library.add(faImage);
    library.add(faSyncAlt);

    const app = createApp(App);
    
    app.component('font-awesome-icon', FontAwesomeIcon);
    app.use(createPinia());
    app.use(router);

    await bootstrapPromise;

    app.mount('#app');
}

main();
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from './router';

import 'bootstrap/dist/css/bootstrap.min.css';

import { faHome } from '@fortawesome/free-solid-svg-icons/faHome';
import { faImage } from '@fortawesome/free-solid-svg-icons/faImage';
import { faList } from '@fortawesome/free-solid-svg-icons/faList';
import { faSort } from '@fortawesome/free-solid-svg-icons/faSort';
import { faMoon } from '@fortawesome/free-solid-svg-icons/faMoon';
import { faSun } from '@fortawesome/free-solid-svg-icons/faSun';
import { faSyncAlt } from '@fortawesome/free-solid-svg-icons/faSyncAlt';

import { library} from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

import ToastPlugin from 'vue-toast-notification';
import 'vue-toast-notification/dist/theme-sugar.css';

import '@/css/night-mode.css';

import App from './App.vue';

function main() {
    library.add(faHome);
    library.add(faSort);
    library.add(faList);
    library.add(faImage);
    library.add(faSyncAlt);
    library.add(faMoon);
    library.add(faSun);

    const app = createApp(App);
    
    app.component('font-awesome-icon', FontAwesomeIcon);
    app.use(createPinia());
    app.use(router);
    app.use(ToastPlugin);

    app.mount('#app');
}

main();
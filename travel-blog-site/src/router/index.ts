import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import TravelAlbum from '@/components/TravelAlbum.vue'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: '/',
      name: 'Home',
      component: HomeView
    },
    {
      path: '/trip/:album',
      component: TravelAlbum,
      props: true,
    }
  ]
})

export default router

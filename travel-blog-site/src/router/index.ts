import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import TravelAlbum from '@/components/TravelAlbum.vue'
import Admin from '@/components/Admin.vue'

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
    },
    {
      path: '/admin',
      name: 'Admin',
      component: Admin,
    }
  ]
})

export default router

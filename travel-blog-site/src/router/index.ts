import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import TravelAlbum from '@/components/TravelAlbum.vue'
import Admin from '@/components/Admin.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/trip/:album',
      component: TravelAlbum,
      props: true,
    },
    {
      path: '/admin',
      component: Admin,
    }
  ]
})

export default router

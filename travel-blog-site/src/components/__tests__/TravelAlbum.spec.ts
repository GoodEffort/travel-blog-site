import { describe, it, expect } from 'vitest'

import { mount } from '@vue/test-utils'
import TravelAlbum from '../TravelAlbum.vue'

describe('HelloWorld', () => {
  it('renders properly', () => {
    const wrapper = mount(TravelAlbum, { props: { album: 'Iceland' } })
    expect(wrapper.text()).toContain('Iceland')
  })
})

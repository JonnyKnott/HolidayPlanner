import { render } from '@testing-library/react'
import App from '../App'

describe('App', () => {
  it('mounts without throwing', () => {
    render(<App />)
    expect(document.body).toBeDefined()
  })
})

const axios = require('axios')

export default class AxiosService {
  post(url, data, IsRequired, Header) {
    return axios.post(url, data, IsRequired && Header)
  }
}

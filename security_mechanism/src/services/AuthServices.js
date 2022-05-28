import AxiosService from "./AxiosService";
import Configuration from "../configurations/Configuration";

const axiosService = new AxiosService();

export default class AuthServices {
    SignUp(data) {
        return axiosService.post(Configuration.SignUp, data, false)
    }
    Login(data) {
        return axiosService.post(Configuration.Login, data, true, FormData)
    }
    Register(data) {
        return axiosService.post(Configuration.Register, data, true, FormData)
    }
}
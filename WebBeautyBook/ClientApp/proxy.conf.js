const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:39774';

const PROXY_CONFIG = [
  {
    context: [
      //User data
      "/api/User",

      //Category
      "/api/Category",
      "/api/Category/GetMainCategories",

      //Auth
      "/api/Auth",
      "/api/Auth/register",
      "/api/Auth/login",
      "/api/Auth/confirmEmail",
      "/api/Auth/forgotPassword",
      "/api/Auth/resetPassword",
      "/api/Auth/registrationViaAdmin",
      "/api/Auth/registrationViaCompany",

      //Location
      "/api/Location/getAll",
      "/api/Location/getAllCountry",
      "/api/Location/getAllCity",

      //Company
      "/api/Company",
      "/api/Company/getAll",
      "/api/Company/getMyCompany",
      "/api/Company/getWorkers",

      //Service
      "/api/Service",

      //WorkerService
      "/api/WorkerService/getWorker",
      "/api/WorkerService/insertWorkerToService",
      "/api/WorkerService/removeWorkerFromService",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;

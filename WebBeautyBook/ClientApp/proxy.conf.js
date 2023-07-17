const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:39774';

const PROXY_CONFIG = [
  {
    context: [
      //User data
      "/api/User",

      //Auth
      "/api/Auth",
      "/api/Auth/register",
      "/api/Auth/login",
      "/api/Auth/confirmEmail",
      "/api/Auth/forgotPassword",
      "/api/Auth/resetPassword",
      "/api/Auth/registrationViaAdmin",
      "/api/Auth/registrationViaCompany",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;

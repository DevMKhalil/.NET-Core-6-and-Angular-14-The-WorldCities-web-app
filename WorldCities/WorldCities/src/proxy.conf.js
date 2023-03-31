const PROXY_CONFIG = [
  {
    context: [
      //"/weatherforecast",
      "/api",
    ],
    target: "https://localhost:40443",
    secure: false
  }
]

module.exports = PROXY_CONFIG;

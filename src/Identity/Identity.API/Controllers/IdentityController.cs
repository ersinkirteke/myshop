﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IConfiguration configuration, ILogger<IdentityController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Login with user credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("login")]
        public IActionResult Login(string userName, string password)
        {
            //TODO:implement identity service and handle login with identity
            TokenHandler._configuration = _configuration;
            return Ok(userName == "ersinkirteke" && password == "12345" ? TokenHandler.CreateAccessToken() : new UnauthorizedResult());
        }
    }
}

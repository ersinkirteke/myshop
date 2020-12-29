using Basket.API.Models;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    //TODO: add global exception middleware to handle exception globally
    //TODO: add logging mechanism with ELK stack or Graylog
    //TODO: think about audit log mechanism and add correlationid to the request header
    //TODO: use external audit log with rabbitmq in order to getting best performance
    //TODO: use automapper for mapping mechanism
    //TODO: if it is need, try to add cqrs with mediatr and refit.
    //TODO: later add retry and circuit breaker pattern
    //TODO: use masstransit as a service bus
    //TODO: use external configuration store pattern
    //TODO: use saga pattern 
    //TODO: add all the container to the kubernetes cluster
    //TODO: write azure pipeline yml for each api
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        #region PROPERTIES
        private readonly IHttpClientFactory _clientFactory;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientFactory"></param>
        public BasketController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        #endregion

        #region POST
        /// <summary>
        /// Add basket item to shopping cart
        /// </summary>
        /// <param name="basketItemRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("items")]
        public async Task<ActionResult> AddToCart(AddBasketItemRequest basketItemRequest)
        {
            //TODO:create web bff on the apigateway,configure ocelot json to go for created web bff and gather all the service calls on the BFF side later
            //TODO: use service layer in BFF side
            //TODO: add ef core,dbcontext and ms-sql data store as a docker container
            //TODO: write urls to the constants
            //TODO:Try with grpc
            //TODO:add healtchecks to apigateway and add service discovery
            //get cart item catalog info from the catalog service

            HttpClient httpClient = _clientFactory.CreateClient();

            string accessToken = await HttpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            Uri url = new Uri("http://localhost:8004/api/v1/catalog/item?catalogItemId=" + basketItemRequest.CatalogItemId);

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Authorization header has been set, but the server reports that it is missing.
                // It was probably stripped out due to a redirect.

                var finalRequestUri = response.RequestMessage.RequestUri; // contains the final location after following the redirect.

                if (finalRequestUri != url) // detect that a redirect actually did occur.
                {
                    // If this is public facing, add tests here to determine if Url should be trusted
                    response = await httpClient.GetAsync(finalRequestUri);
                }
            }

            var content = await response.Content.ReadAsStringAsync();
            var catalogItem = JsonConvert.DeserializeObject<CatalogItem>(content);

            //control stock informations
            if (catalogItem != null && catalogItem.AvailableStock < basketItemRequest.Quantity)
            {
                return BadRequest($"The item you try to buy,we have only {catalogItem.AvailableStock} items in our stock.");
            }

            if(catalogItem != null && catalogItem.MaxStockThreshold < basketItemRequest.Quantity)
            {
                return BadRequest($"You can add only {catalogItem.MaxStockThreshold} items from this product.");
            }

            //TODO:add jwt to azure durable function later
            //call addtocart azure durable function with using a actor model for handling shopping cart
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"http://localhost:7071/api/cart/{basketItemRequest.BasketId}/add");
            req.Method = "POST";
            req.ContentType = "application/json";
            Stream stream = req.GetRequestStream();
            var request= new { ProductId=basketItemRequest.CatalogItemId, Quantity=20 };
            string json = JsonConvert.SerializeObject(request);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            if(res.StatusCode!=HttpStatusCode.Accepted)
            {
                return BadRequest("Item not adding to basket succesfully");
            }

            //TODO: try shopping cart side with event sourcing 
            return Ok();
        }
        #endregion
    }
}

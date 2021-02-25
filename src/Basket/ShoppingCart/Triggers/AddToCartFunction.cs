using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace MyShop.AddToCart
{
    public static class AddToCartFunction
    {
        /// <summary>
        /// Azure durable function for adding cart item to the shopiing cart, it use retry mechanism for failures. 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="id"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [FunctionName("AddToCart")]
        [FixedDelayRetry(5, "00:00:02")]
        public static async Task<ActionResult> RunAsync(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cart/{id}/add")] HttpRequestMessage req,
          Guid id,
          [DurableClient] IDurableEntityClient client)
        {
            if (id == Guid.Empty)
            {
                return (ActionResult)new BadRequestObjectResult("Id is required");
            }
            var entityId = new EntityId(nameof(ShoppingCartEntity), id.ToString());

            var data = await req.Content.ReadAsAsync<CartItem>();

            await client.SignalEntityAsync<IShoppingCart>(entityId, proxy => proxy.Add(data));

            return (ActionResult)new AcceptedResult(); 
        }
    }
}
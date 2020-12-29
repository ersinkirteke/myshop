using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace MyShop.AddToCart
{
    /// <summary>
    /// Durable entity for shopping cart, it works like as an actor which holds own state in memory
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCartEntity : IShoppingCart
    {

        [JsonProperty("list")]
        private List<CartItem> list { get; set; } = new List<CartItem>();

        /// <summary>
        /// Add cart item
        /// </summary>
        /// <param name="item"></param>
        public void Add(CartItem item)
        {
            // Get existing
            var existingItem = this.list.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem == null)
            {
                this.list.Add(item);
            }
            else
            {
                existingItem.Count += item.Count;
            }
        }

        /// <summary>
        /// Remove cart item
        /// </summary>
        /// <param name="item"></param>
        public void Remove(CartItem item)
        {
            var existingItem = this.list.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem == null)
            {
                return;
            }

            if (existingItem.Count > item.Count)
            {
                existingItem.Count -= item.Count;
            }
            else
            {
                this.list.Remove(existingItem);
            }
        }

        /// <summary>
        /// Get Cart items
        /// </summary>
        /// <returns></returns>
        public Task<ReadOnlyCollection<CartItem>> GetCartItems()
        {
            return Task.FromResult(this.list.AsReadOnly());
        }

        [FunctionName(nameof(ShoppingCartEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
               => ctx.DispatchAsync<ShoppingCartEntity>();
    }
}
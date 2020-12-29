using Catalog.Domain.ValueObjects;
using Catalog.Infrastructure.Exceptions;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Catalog.Domain.Entities
{
    public class CatalogItem:Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        [JsonIgnore]
        public Price Price { get; set; }

        public int AvailableStock { get; set; }

        public int MaxStockThreshold { get; set; }

        public CatalogItem() { }

        public int RemoveStock(int quantityDesired)
        {
            if (AvailableStock == 0)
            {
                throw new CatalogException($"Empty stock, product item {Name} is sold out");
            }

            if (quantityDesired <= 0)
            {
                throw new CatalogException($"Item units desired should be greater than zero");
            }

            int removed = Math.Min(quantityDesired, this.AvailableStock);

            this.AvailableStock -= removed;

            return removed;
        }

        public int AddStock(int quantity)
        {
            int original = this.AvailableStock;

            if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
            {
                this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
            }
            else
            {
                this.AvailableStock += quantity;
            }

            return this.AvailableStock - original;
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}

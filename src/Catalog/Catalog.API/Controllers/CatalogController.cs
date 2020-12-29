using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using Common.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        #region PROPERTIES
        private readonly ILogger<CatalogController> _logger;
        private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger = logger;
        }
        #endregion

        #region GET
        /// <summary>
        /// Getting Catalog Item By Catalog Item ID
        /// </summary>
        /// <param name="catalogItemId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("item")]
        public async Task<ActionResult<CatalogItem>> GetShoppingCartItem([FromQuery] int catalogItemId)
        {
            //TODO: add generic repository and unit of work later
            //TODO: add ef core ,dbcontext and add ms-sql data store as a docker container and retrieve data from database with ids
            IEnumerable<CatalogItem> items = new[] {
                new CatalogItem
                {
                    Id = 1,
                    Name = "Dut Kurusu",
                    Description = "Bahçemden Kayısı Dut Kurusu 400 gr",
                    AvailableStock = 50,
                    MaxStockThreshold = 10,
                    Price = Price.FromDecimal(35, "TL", CurrencyLookup)
                },
                new CatalogItem
                {
                    Id = 2,
                    Name = "Gün Kurusu",
                    Description = "Bahçemden Kayısı Gün Kurusu 1 kg",
                    AvailableStock = 50,
                    MaxStockThreshold = 10,
                    Price = Price.FromDecimal(80, "TL", CurrencyLookup)
                }
            };

            var item = items.FirstOrDefault(x => x.Id == catalogItemId);
            if (item == null) return NotFound();

            return item;
        }
        #endregion
    }
}

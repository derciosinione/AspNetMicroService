using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Models;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger _logger;

        public CatalogController(IProductRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduts()
        {
            var result = await _repository.GetProducts();
            return Ok(result);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var result = await _repository.GetProduct(id);
            if(result is null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{category}", Name = nameof(GetProductByCategory))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var result = await _repository.GetProductByCategory(categoryName);
            if(result is null)
            {
                _logger.LogError($"Product with category: {categoryName}, not found.");
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{name}", Name = nameof(GetProductByName))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var result = await _repository.GetProductByName(name);
            if(result is null)
            {
                _logger.LogError($"Product with name: {name}, not found.");
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);

            return CreatedAtRoute(nameof(Created), new { id = product.Id}, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);

            return CreatedAtRoute(nameof(Created), new { id = product.Id}, product);
        }

        [HttpGet("{id:length(24)}", Name = nameof(DeleteProduct))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var result = await _repository.DeleteProduct(id);
            return Ok(result);
        }

    }
}
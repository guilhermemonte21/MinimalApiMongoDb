using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIMongo.Domains;
using MinimalAPIMongo.Services;
using MongoDB.Driver;

namespace MinimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPost]

        public async Task<ActionResult> Post(Product newProduct)
        {
            try
            {
                await _product.InsertOneAsync(newProduct);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetById")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                return Ok(await _product.Find(FindById(id)).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put(Product newProduct, string id)
        {
            try
            {
                newProduct.Id = id;
                await _product.FindOneAndReplaceAsync(x => x.Id == id, newProduct);
                return Ok(newProduct);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }


       [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _product.DeleteOneAsync(FindById(id));
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }

        
        public FilterDefinition<Product> FindById(string id)
        {

            return Builders<Product>.Filter.Eq(m => m.Id, id);
        }

    }
}

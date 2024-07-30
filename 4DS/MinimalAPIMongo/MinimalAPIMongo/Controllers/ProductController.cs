﻿using Microsoft.AspNetCore.Http;
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
                return Ok(newProduct);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();
                return product is not null ? Ok(product) : NotFound();
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
                //buscar por id (filtro)
                var filter = Builders<Product>.Filter.Eq(x => x.Id, newProduct.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _product.ReplaceOneAsync(filter, newProduct);
                    return Ok();
                }
                return NotFound();
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

        [HttpGet("FindById")]
        public FilterDefinition<Product> FindById(string id)
        {

            return Builders<Product>.Filter.Eq(m => m.Id, id);
        }

    }
}

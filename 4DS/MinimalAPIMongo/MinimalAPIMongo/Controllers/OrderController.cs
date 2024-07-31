using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIMongo.Domains;
using MinimalAPIMongo.Services;
using MinimalAPIMongo.ViewModel;
using MongoDB.Driver;

namespace MinimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;


        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("Order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("Client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("Product");

        }
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                foreach (var order in orders)
                {
                    if (order.ProductIds != null)
                    {
                        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductIds);

                        order.Products = await _product.Find(filter).ToListAsync();
                    }

                    if (order.ClientId != null)
                    {
                        order.Client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                    }
                }
                0+

                return Ok(orders);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPost]

        public async Task<ActionResult> Create(OrderViewModel orderViewModel)
        {
            try
            {
                Order order = new Order();

                order.Id = orderViewModel.Id;
                order.Date = orderViewModel.Date;
                order.Status = orderViewModel.Status;
                order.ProductIds = orderViewModel.ProductIds;
                order.ClientId = orderViewModel.ClientId;

                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null) 
                {
                    return NotFound("Cliente não Encontrado");
                }

                //Tambem esta correto, porem optei por fazer no metodo GET
                //List<Product> products = new List<Product>();
                //var produtosBuscados = order.ProductIds;
                //var productList = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                //foreach (string Id in produtosBuscados) 
                //{
                //    foreach(Product p in productList)
                //    {
                //        if(Id == p.Id)
                //        {
                //            products.Add(p);
                //        }
                //    }
                //}
                //order.Products = products;
                //order.Client = client;

                await _order!.InsertOneAsync(order);
                return StatusCode(201, order);
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
                await _order.DeleteOneAsync(FindById(id));
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpGet("FindById")]
        public FilterDefinition<Order> FindById(string id)
        {

            return Builders<Order>.Filter.Eq(m => m.Id, id);
        }
        [HttpPut]
        public async Task<ActionResult> Put(Order order, string id)
        {
            try
            {
                //buscar por id (filtro)
                var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _order.ReplaceOneAsync(filter, order);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}

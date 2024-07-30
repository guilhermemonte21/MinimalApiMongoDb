using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIMongo.Domains;
using MinimalAPIMongo.Services;
using MongoDB.Driver;

namespace MinimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("Client");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPost]

        public async Task<ActionResult> Post(Client client)
        {
            try
            {
                await _client.InsertOneAsync(client);
                return Ok(client);
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
                var cliente = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
                return cliente is not null ? Ok(cliente) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put(Client client, string id)
        {
            try
            {
                //buscar por id (filtro)
                var filter = Builders<Client>.Filter.Eq(x => x.Id, client.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _client.ReplaceOneAsync(filter, client);
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
                await _client.DeleteOneAsync(FindById(id));
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpGet("FindById")]
        public FilterDefinition<Client> FindById(string id)
        {

            return Builders<Client>.Filter.Eq(m => m.Id, id);
        }

    }
}

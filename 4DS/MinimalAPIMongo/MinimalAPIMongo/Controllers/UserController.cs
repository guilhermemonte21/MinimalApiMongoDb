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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<Users> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<Users>("Users");
        }

        [HttpPost]
        public async Task<ActionResult> Post(Users user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return Ok();
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
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                return user is not null ? Ok(user) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put(Users user, string id)
        {
            try
            {
                //buscar por id (filtro)
                var filter = Builders<Users>.Filter.Eq(x => x.Id, user.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _user.ReplaceOneAsync(filter, user);
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
                await _user.DeleteOneAsync(FindById(id));
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        [HttpGet("FindById")]
        public FilterDefinition<Users> FindById(string id)
        {

            return Builders<Users>.Filter.Eq(m => m.Id, id);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemon.Cache;

namespace RedisWebSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IDistributedCache cache;
        public ValuesController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        // GET api/values
        [HttpGet]
        public async Task<List<TestModel>> GetAsync()
        {
            List<TestModel> models = new List<TestModel>();
            models.Add(new TestModel() { Key = "keys", Value = "99988998" });
            await cache.SetValueAsync<List<TestModel>>("keys", models, TimeSpan.FromMinutes(5));
            return await cache.GetValueAsync<List<TestModel>>("keys");
        }

        public class TestModel
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetAsync(int id)
        {
            await cache.SetValueAsync("key", id.ToString(), TimeSpan.FromMinutes(5));
            return await cache.GetValueAsync("key");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

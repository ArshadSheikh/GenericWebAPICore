using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicAndGenericControllersSample.DB;
using Microsoft.AspNetCore.Mvc;

namespace DynamicAndGenericControllersSample.Controllers
{
    [Route("api/[controller]")]
    public class BaseController<T> : Controller where T : class
    {
        private GenericRepository<T> _storage;

        public BaseController(GenericRepository<T> storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public IEnumerable<T> GetAll()
        {
            return _storage.GetAll();
        }

        [HttpGet("{id}")]
        public T Get(string Id)
        {
            return _storage.GetByID(Id);
        }

        [HttpPost("{id}")]
        public void Post([FromBody]T value)
        {
            _storage.Insert(value);
        }

        [HttpPut]
        public void Put([FromBody]T value)
        {
            _storage.Update(value);
        }

        [HttpDelete]
        public void Delete(string id)
        {
            _storage.Delete(id);
        }

    }
}

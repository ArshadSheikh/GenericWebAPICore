using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicAndGenericControllersSample.DB;
using GenericWebAPICore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DynamicAndGenericControllersSample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController<T> : Controller where T : class
    {
        private GenericRepository<T> _storage;

        public BaseController(GenericRepository<T> storage)
        {
            _storage = storage;
        }

        [HttpGet]
        [EnableQuery]
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

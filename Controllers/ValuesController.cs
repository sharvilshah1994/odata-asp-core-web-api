using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using TestApi.DbContexts;
using TestApi.Models;

namespace TestApi.Controllers
{
    public class ValuesController : ODataController
    {

        private readonly ValueContext _valueContext;

        public ValuesController(ValueContext valueContext)
        {
            _valueContext = valueContext;
        }

        // GET /values
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        [ODataRoute("/values")]
        public PageResult<Value> Get()
        {
            IQueryable<Value> results = _valueContext.Values;
            return new PageResult<Value>(results as IEnumerable<Value>, Request.GetNextPageLink(1), results.Count());
        }

        // GET /values/5
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        [ODataRoute("/values(id={id})")]
        public SingleResult<Value> Get([FromODataUri] int id)
        {
            IQueryable<Value> result = _valueContext.Values.Where(p => p.Id == id);
            return SingleResult.Create(result);
        }
    }
}

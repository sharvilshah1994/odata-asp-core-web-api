using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using TestApi.DbContexts;
using TestApi.Models;

namespace TestApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOData();
            services.AddDbContext<ValueContext>(
                options => options.UseSqlServer(
                            Configuration.GetConnectionString("TestApiDatabase"),
                            providerOptions => providerOptions.EnableRetryOnFailure()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(mvc =>
                {
                    mvc.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                    mvc.MapODataServiceRoute("odata", "odata", GetImplicitModel());
                });

        }

        private IEdmModel GetImplicitModel()
        {
            ODataModelBuilder model = new ODataConventionModelBuilder();
            model.EntitySet<Value>("Values");
            return model.GetEdmModel();
        }

        private IEdmModel GetExplicitModel()
        {
            EdmModel edmModel = new EdmModel();

            EdmEntityType valuesEntityType = new EdmEntityType("TestApi.Models", "Values");
            valuesEntityType.AddKeys(valuesEntityType.AddStructuralProperty("Id", new EdmPrimitiveTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int32), false)));
            valuesEntityType.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);

            EdmEntityContainer defaultEntityContainer = new EdmEntityContainer("Default", "Container");
            defaultEntityContainer.AddEntitySet("Values", valuesEntityType);

            edmModel.AddElement(valuesEntityType);
            edmModel.AddElement(defaultEntityContainer);
            return edmModel;
        }
        
    }
}

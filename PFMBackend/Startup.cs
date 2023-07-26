using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PFMBackend.Database.Repositories;
using PFMBackend.Database;
using PFMBackend.Services;
using System.Reflection;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace PFMBackend
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions", Version = "v1" });
            });

            services.AddDbContext<TransactionsDbContext>(opts => {
                opts.UseNpgsql(CreateConnectionString());
            });

            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<ITransactionsService, TransactionsService>();

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //cors
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions v1"));
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                scope.ServiceProvider.GetRequiredService<TransactionsDbContext>().Database.Migrate();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private string CreateConnectionString()
        {
            var username = Environment.GetEnvironmentVariable("Database_Username");
            var pass = Environment.GetEnvironmentVariable("Database_Password");
            var host = Environment.GetEnvironmentVariable("Database_Host") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("Database_Port") ?? "5432";
            var dbName = Environment.GetEnvironmentVariable("Database_Name") ?? "transactions";

            var builder = new NpgsqlConnectionStringBuilder()
            {
                Username = username,
                Password = pass,
                Host = host,
                Port = int.Parse(port),
                Database = dbName,
                Pooling = true
            };

            return builder.ConnectionString;
        }
    }
}

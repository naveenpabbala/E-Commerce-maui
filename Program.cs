using E_Commercenaveen.Api.Constants;
using E_Commercenaveen.Api.Data;
using Microsoft.EntityFrameworkCore;
using E_Commercenaveen.Api.Data.Entities;
using Npgsql; 

namespace E_Commercenaveen.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options =>
              options.UseNpgsql(builder.Configuration.GetConnectionString(DataBaseConstants.GroceryConnectionStringKey)));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            var mastersGroup = app.MapGroup("/masters")
                                  .AllowAnonymous();


            mastersGroup.MapGet("/categories", async (DataContext context) =>
               TypedResults.Ok(await context.Categories
                   .AsNoTracking()
                   .ToArrayAsync()
                    )
            );
            mastersGroup.MapGet("/offers", async (DataContext context) =>
               TypedResults.Ok(await context.Offers
                   .AsNoTracking()
                   .ToArrayAsync()
                    )
            );

            app.MapGet("/popular-products", async (DataContext context, int? count) =>
            {
                if (!count.HasValue || count <= 0)
                    count = 6;

                var randomProducts = await context.Products
                                        .AsNoTracking()
                                        .OrderBy(p => Guid.NewGuid())
                                        .Take(count.Value)
                                        .Select(Product.DtoSelector)
                                        .ToArrayAsync();
                return TypedResults.Ok(randomProducts);
            });


            //app.UseAuthorization();


            //app.MapControllers();

            app.Run("https://localhost:12345");
        }
    }
}

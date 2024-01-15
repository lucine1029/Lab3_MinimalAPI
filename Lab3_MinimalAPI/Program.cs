using Lab3_MinimalAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab3_MinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");

            builder.Services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(connectionString));

            var app = builder.Build();

            app.MapGet("/", () => "Hej You, gott nytt år ^__^");

            //--------- Person
            app.MapPost("/person", ApiHandler.AddNewPerson);
            app.MapGet("/person", ApiHandler.ListAllPeople);

            //--------- Interest
            app.MapPost("/person/{personId}", ApiHandler.AddNewInterest);
            app.MapGet("/person/{personId}/interests", ApiHandler.ListPersonsInterests);
            app.MapPost("/person/{personId}/interests/{interestId}/connectInterest", ApiHandler.ConnectInterestToPerson);

            //--------- InterestLink
            app.MapGet("/person/{personId}/interestLinks", ApiHandler.ListPersonsLinks);
            app.MapPost("/person/{personId}/interests/{interestId}/insertLink", ApiHandler.InsertLinkToPersonToInterest);

            app.Run();
        }
    }
}
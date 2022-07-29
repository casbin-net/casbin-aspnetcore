using Casbin.AspNetCore.Authorization;
using System.Security.Claims;
using Casbin.AspNetCore.Authorization.Transformers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add Casbin Authorization
builder.Services.AddCasbinAuthorization(options =>
{
    options.PreferSubClaimType = ClaimTypes.Name;
    options.DefaultModelPath = Path.Combine("CasbinConfigs", "basic_model.conf");
    options.DefaultPolicyPath = Path.Combine("CasbinConfigs", "basic_policy.csv");

    // Comment line below to use the default BasicRequestTransformer
    // Note: Commenting the line means that the action methods MUST have [CasbinAuthorize()] attribute which explicitly specifies obj and policy. Otherwise authorization will be denied
    options.DefaultRequestTransformer = new KeyMatchRequestTransformer();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCasbinAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Text;
using adoptera_hund.api.Data;
using adoptera_hund.api.Models;
using adoptera_hund.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/////// Adding swagger authentication, configures security definitions and requirements for Swagger UI
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

// set upp DB connection
    builder.Services.AddDbContext<AdopteraHundContext>(options => {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDevelopment"));
    });

// User Identity config
builder.Services.AddIdentityCore<UserModel>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AdopteraHundContext>();
// JWT config
builder.Services.AddScoped<TokenService>();

/////   JWT Authentication, default authentication scheme for the application
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["tokenSettings:tokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
/////   JWT Authentication, default authentication scheme for the application
builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
    options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  seedDataToDB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AdopteraHundContext>();
    var userMng = services.GetRequiredService<UserManager<UserModel>>();
    var roleMng = services.GetRequiredService<RoleManager<IdentityRole>>();
    await context.Database.MigrateAsync();

    await SeedDataToDB.LoadRolesAndUsers(userMng, roleMng);
    await SeedDataToDB.LoadDogsToDB(context);
}
catch(Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

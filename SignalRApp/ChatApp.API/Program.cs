using ChatApp.Business.DependencyResolvers.Microsoft;
using ChatApp.Business.Hub;
using ChatApp.Core.Utilities.Middleware;
using ChatApp.DataAccess.Concrete.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný ve Temel Servisler
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // Genelde builder.Services.AddControllers(); civarýna eklenir.
builder.Services.AddRegisterServices();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// 2. CORS Yapýlandýrmasý (DÜZELTÝLDÝ)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Angular portunuz genellikle 4200'dür. 
        // SignalR ve Auth iþlemleri için spesifik origin belirtmek daha saðlýklýdýr.
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // SignalR baðlantýlarý için bu kritiktir
    });
});

// 3. Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp API", Version = "v1" });
	c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

	// --- JWT TOKEN SWAGGER YAPILANDIRMASI ---

	// 1. Swagger'a JWT Bearer güvenlik þemasýný tanýmlýyoruz
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
	});

	// 2. Bu güvenlik þemasýný tüm API isteklerine global olarak uyguluyoruz
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});


var app = builder.Build();

// 4. Middleware Sýralamasý (KRÝTÝK SIRALAMA)

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp v1");
	});
}

// Exception middleware en üstte kalmalý
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Rotalar burada belirlenir

// CORS, UseRouting'den hemen sonra, UseAuthentication'dan önce gelmelidir!
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// 5. Endpoint Eþleþmeleri
app.MapControllers();
app.MapHub<ChatHub>("/chat-hub");

app.Run();
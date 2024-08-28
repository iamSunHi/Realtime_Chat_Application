
using ChatApp_API.Data;
using ChatApp_API.Repositories;
using ChatApp_API.Repositories.IRepositories;
using ChatApp_API.Services;
using ChatApp_API.Services.IServices;
using ChatApp_API.Services.WebSocketServices;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			#region Repositories

			builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

			#endregion

			#region Services

			builder.Services.AddScoped<IAuthService, AuthService>();

			#endregion

			// Add services for websockets
			builder.Services.AddSingleton<WebSocketConnectionManager>();
			builder.Services.AddSingleton<ChatWebSocketHandler>();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Auto update database
			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				dbContext.Database.Migrate();
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseWebSockets();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}

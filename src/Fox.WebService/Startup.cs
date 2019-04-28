using System.Text;

using Fox.DataAccess;
using Fox.DataAccess.Models;
using Fox.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Serialization;

namespace Fox
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
			services.AddMvc().AddJsonOptions(j => j.SerializerSettings.ContractResolver = new DefaultContractResolver()
			{
				NamingStrategy = new SnakeCaseNamingStrategy()
			});

			IConfigurationSection appSettings = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettings);

			AppSettings settings = appSettings.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(settings.Secret);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			services.AddDbContext<DeadlockContext>();
			services.AddScoped<IUsuarioRepository, UsuarioRepository>();
			services.AddScoped<IProductoRepository, ProductoRepository>();
			services.AddScoped<IOrdenCompraRepository, OrdenCompraRepository>();
			services.AddScoped<IEstadoCompraRepository, EstadoCompraRepository>();
			services.AddScoped<ILaboratorioRepository, LaboratorioRepository>();
			services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(c => c.AllowAnyOrigin());
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}

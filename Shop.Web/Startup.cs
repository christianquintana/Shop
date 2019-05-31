namespace Shop.Web
{
    using System.Text;
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    // Clase de inicio
    public class Startup
    {
        // El método ConfigureServices configura/registra los servicios de la aplicación y se consumen en toda la aplicación a través de la inyección de dependencia (DI) o ApplicationServices.
        // ConfigureServices y Configure son llamados en tiempo de ejecución cuando se inicia la aplicación

        // Constructor que toma un parametro IConfiguration que representa un conjunto de clave/valor de propiedades de configuración de la aplicación 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Aqui se carga el archivo appsettings.json en una instancia de IConfiguration dentro de la aplicación 
            //string userName = Configuration.GetSection("AppConfiguration")["UserName"];
            //string password = Configuration.GetSection("AppConfiguration")["Password"];
        }

        public IConfiguration Configuration { get; }

        // Método para agregar servicios al contenedor. Este método es llamado en tiempo de ejecución. 
        public void ConfigureServices(IServiceCollection services)
        {
            // Agrega y configura el sistema de identidad para los tipos de usuarios y roles especificados
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<DataContext>();

            // Le decimos a la aplicacion que se van a generar Tokens (autenticación basada en Token (JWT Json Web Token) para proteger nuestras Apis de usuarios no autorizados)
            // y se va a utilizar autenticacion, cookies, JwtBearer
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        // SymmetricSecurityKey no se puede decompilar | AsymmetricSecurityKey se puede compilar y decompilar
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });

            // Registra el contexto dado como un servicio en IServiceCollection (Añade inyección de dependencia de base de datos en la aplicación)            
            services.AddDbContext<DataContext>(cfg =>
            {
                // Configura el contexto para conectarse a un servidor de base de datos
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Agregue un servicio transitorio del tipo especificado en TService a la IServiceCollection especificada (se crea cada vez que se solicita)
            // Transitorio: En las instancias de objetos nuevos transitorios, se inyectarán en una sola solicitud y respuesta.
            services.AddTransient<SeedDb>();

            //services.AddScoped<IRepository, Repository>();

            // Agrega un servicio de ámbito del tipo especificado en TService con un tipo de implementación especificado en TImplementation a la IServiceCollection especificada
            // Con ámbito: En el mismo ámbito, la instancia de objeto se inyectará en una sola solicitud y respuesta.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IUserHelper, UserHelper>();

            // Singleton: En Singleton, el mismo objeto se inyectará en todas las solicitudes y respuestas. En este caso se creará una instancia global del objeto.


            // Esta lambda determina si se necesita el consentimiento del usuario para cookies no esenciales para una solicitud determinada
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // El método SetCompatibilityVersion permite que una aplicación se inscriba o rechace los posibles cambios de comportamiento introducidos en ASP.NET Core MVC 2.1 o posterior. 
            // Estos cambios de comportamiento potencialmente de ruptura están generalmente en cómo se comporta el subsistema MVC y cómo en tiempo de ejecución llama a su código. 
            // De optar, obtiene el comportamiento más reciente y el comportamiento a largo plazo de ASP.NET Core.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // Este método es llamado en tiempo de ejecución para configurar la canalización de solicitud HTTP.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Si el entorno es de desarrollo
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // El valor HSTS predeterminado es de 30 días. Es posible que desee cambiar esto para los escenarios de producción, consulte https://aka.ms/aspnetcore-hsts.
                // Agrega middleware para usar HSTS, agrega el encabezado Estricto-Transporte-Seguridad. middleware: software que proporciona servicios a las aplicaciones
                app.UseHsts();
            }

            // Agrega middleware para redireccionar los requisitos HTTP a HTTPS. middleware: software que proporciona servicios a las aplicaciones
            app.UseHttpsRedirection();
            // Habilita el servicio de archivos estáticos para la ruta de solicitud actual
            app.UseStaticFiles();
            // Permite capacidades de autenticación
            app.UseAuthentication();
            // Permite capacidades de política de cookies
            app.UseCookiePolicy();

            // Configuracion/registro de enrutamiento en MVC, la ruta se configura mediante el método de extensión MapRoute con el nombre y la plantilla del patrón URL de la ruta
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

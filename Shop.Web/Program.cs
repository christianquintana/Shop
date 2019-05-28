namespace Shop.Web
{
    using Data;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        // Para arrancar la aplicación usa el método Main (similar a las aplicaciones de consola) y Startup se carga a través de ahí.
        public static void Main(string[] args)
        {
            // Llama al método que crea y configura el WebHost
            var host = CreateWebHostBuilder(args).Build();
            // Llama al método alimentador (semilla) de la base de datos (clase SeedDb)
            RunSeeding(host);
            // Ejecuta la aplicación web y bloquea el hilo de llamada hasta que se apague (shutdown) el host
            host.Run();
        }

        // Método alimentador (semilla) de la base de datos (clase SeedDb)
        private static void RunSeeding(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                // Invoca al método SeedAsync de la clase SeedDb y espera el termino de la operacion asincrona (async Task) para completar la ejecución
                seeder.SeedAsync().Wait();
            }
        }

        // Método que crea y configura el WebHost, especifica el tipo de inicio que utilizará el servidor web (Startup)
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

}

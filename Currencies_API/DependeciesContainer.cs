using ApiCaller;
using Currencies_API.DataSource;
using Currencies_API.Domain;
using DataSource;

namespace Currencies_API
{
    public static class DependeciesContainer
    {
        public static void DeclareDependencies(IServiceCollection serviceDescriptors)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            serviceDescriptors.AddSwaggerGen();
            serviceDescriptors.AddControllers();
            serviceDescriptors.AddEndpointsApiExplorer();


            serviceDescriptors.AddScoped<IApiClient, ApiXmlClient>();
            serviceDescriptors.AddScoped<IParser, XmlParser>();
            serviceDescriptors.AddScoped<IDataManager,DataManagerXml>();
            serviceDescriptors.AddScoped<IDataManagerBL, DataManagerBL>();
            serviceDescriptors.AddScoped<ICurrencyExchanger, CurrencyExchanger>();
            serviceDescriptors.AddScoped<ICurrenciesManager, CurrenciesManager>();
        }
    }
}

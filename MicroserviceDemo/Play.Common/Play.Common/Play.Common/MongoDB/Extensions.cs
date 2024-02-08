using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                // Get Configuration Service
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                // ServiceSettings from appsettings.json
                var serviceSettings = configuration.GetRequiredSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                // Get MongoDbSettings from appsettings.json
                var mongoDbSettings = configuration.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                // Construct MongoClient using MongoDbSettings
                var mongoClient = new MongoClient(mongoDbSettings?.ConnectionString);
                // Creating IMongo datbase
                return mongoClient.GetDatabase(serviceSettings?.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                // Retrieve IMongo Database
                var database = serviceProvider.GetService<IMongoDatabase>();
                // Constructing MongoRepository.
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}

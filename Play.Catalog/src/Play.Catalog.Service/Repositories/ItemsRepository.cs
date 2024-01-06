using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items"; //Represents the name of collection in the database 

        private readonly IMongoCollection<Item> dbCollection; //Represents the collection in this case (items) who we are going to work.

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;  //Declaring a filter for the queries.

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); //Creating an Instance of MongoDB 
            var database = mongoClient.GetDatabase("Catalog"); //References the database "Catalog"
            dbCollection = database.GetCollection<Item>(collectionName); //Obtains a collection of Items

        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync(); //Used To obtains all the elements of the collection asynchronous.
        }


        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);

            return await dbCollection.Find(filter).FirstOrDefaultAsync(); //Used to obtains an especific element of the Collection.
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);

        }

        public async Task RemoveAsync (Guid id)
        {
            FilterDefinition<Item> Filter = filterBuilder.Eq(entity => entity.Id, id); 
            await dbCollection.DeleteOneAsync(Filter);
        }

    }
}
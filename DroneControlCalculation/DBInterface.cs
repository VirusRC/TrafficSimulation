using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace DroneControlCalculation
{
	public class DBInterface
	{
		public IMongoClient Client = null;
		public IMongoDatabase Database = null;
		public List<QueryData> queryData = null;

		/// <summary>
		/// Constructor incl. init
		/// </summary>
		public DBInterface()
		{
			Client = new MongoClient();
			Database = Client.GetDatabase(Statics.DRONEDBNAME);
			queryData = new List<DroneControlCalculation.QueryData>();
		}

		/// <summary>
		/// Manual init of DB connection
		/// </summary>
		public void InitDatabase()
		{
			Client = new MongoClient(/*"193.171.53.68"*/);

			Database = Client.GetDatabase(Statics.DRONEDBNAME);
		}

		/// <summar>
		/// Drops all tables from DroneDB
		/// </summary>
		public void ResetDatabase()
		{
			if(Database != null)
			{
				Database.DropCollection(Statics.ISCOORDINATES);
				Database.DropCollection(Statics.SHOULDCOORDINATES);
				Database.DropCollection(Statics.DRONECOMMANDS);
			}
			Client = null;
			Database = null;
		}

		public int QueryData()
		{
			if(Database == null)
			{
				return -1;
			}

			IMongoCollection<BsonDocument> shouldCollection = null;
			IMongoCollection<BsonDocument> isCollection = null;
			List<BsonDocument> shouldTuples = null;
			List<BsonDocument> isTuples = null;

			shouldCollection = Database.GetCollection<BsonDocument>(Statics.SHOULDCOORDINATES);
			isCollection = Database.GetCollection<BsonDocument>(Statics.ISCOORDINATES);

			if(shouldCollection == null || isCollection == null)
			{
				return -1;
			}

			shouldTuples = shouldCollection.Find(FilterDefinition<BsonDocument>.Empty)?.ToList();
			shouldCollection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

			isTuples = isCollection.Find(FilterDefinition<BsonDocument>.Empty)?.ToList();
			isCollection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

			queryData.Add(new QueryData(shouldTuples, isTuples));

			return 0;
		}

		/// <summary>
		/// Sends given command to drone; 
		/// (Stores it into DB)
		/// </summary>
		/// <param name="_document"></param>
		public async void sendCommand(BsonDocument _document)
		{
			var collection = Database.GetCollection<BsonDocument>(Statics.DRONECOMMANDS);
			await collection.InsertOneAsync(_document);
		}

	}
}

using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneControlCalculation
{
	public class QueryData
	{
		public double queryTimeStamp = double.MinValue;
		public List<BsonDocument> shouldData = null;
		public List<BsonDocument> isData = null;

		public QueryData(List<BsonDocument> _should, List<BsonDocument> _is)
		{
			shouldData = _should;
			isData = _is;

			queryTimeStamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public QueryData(QueryData data)
		{
			if(data == null)
			{
				return;
			}

			queryTimeStamp = data.queryTimeStamp;
			shouldData = data.shouldData;
			isData = data.isData;
		}

	}
}

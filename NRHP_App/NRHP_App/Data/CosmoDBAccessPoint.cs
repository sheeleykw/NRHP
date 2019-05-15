using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using NRHP_App.Models;
using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using System.Diagnostics;
using System.Linq;

namespace NRHP_App.Data
{
    public class CosmoDBAccessPoint
    {
        static DocumentClient docClient = null;

        static readonly string databaseName = "Map Items";
        static readonly string collectionName = "Points";

        static async Task<bool> ConnectDatabase()
        {
            if (docClient != null)
                return true;
            try
            {
                docClient = new DocumentClient(new Uri(DBKeys.CosmosUrl), DBKeys.CosmosKey);

                await docClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
                await docClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName }, new RequestOptions { OfferThroughput = 400 });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);

                docClient = null;
                return false;
            }
            return true;
        }

        public async static Task<List<DataPoint>> GetPoints(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            var points = new List<DataPoint>();

            if (!await ConnectDatabase())
                return points;

            var pointsQuery = docClient.CreateDocumentQuery<DataPoint>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), new FeedOptions { MaxItemCount = -1 }).Where(point => point.Latitude <= TopLatitude && point.Latitude >= BottomLatitude && point.Longitude <= RightLongitude && point.Longitude >= LeftLongitude).AsDocumentQuery();

            while (pointsQuery.HasMoreResults)
            {
                var queryResults = await pointsQuery.ExecuteNextAsync<DataPoint>();
                points.AddRange(queryResults);
            }

            return points;
        }
    }
}
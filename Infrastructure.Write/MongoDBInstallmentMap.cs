using Infrastructure.Core;
using Infrastructure.Write.Post;
using Infrastructure.Write.User;
using MongoDB.Bson.Serialization;
using System;

namespace Infrastructure.Write
{
    public static class MongoDBInstallmentMap
    {
        public static void Map()
        {
            BsonClassMap.RegisterClassMap<Dto>(map => {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);

                map.MapIdMember(x => x.Id);
            });

            var postBsonMap = new PostBsonMapper();
            postBsonMap.Map();

            var userBsonMap = new UserBsonMapper();
            userBsonMap.Map();
        }
    }
}

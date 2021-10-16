using Infrastructure.Core;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace Infrastructure.Write.Post
{
    public class PostBsonMapper : IMapper
    {
        public void Map()
        {
            BsonClassMap.RegisterClassMap<PostWriteDto>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);

                map.MapMember(x => x.Title).SetIsRequired(true);
                map.MapMember(x => x.Text).SetIsRequired(true);
                map.MapMember(x => x.Category).SetIsRequired(true);
                map.MapMember(x => x.CreateBy).SetIsRequired(true);
                map.MapMember(x => x.ImageUrl).SetIsRequired(true);

                map.MapMember(x => x.CreateDate).SetIsRequired(true).SetSerializer(new DateTimeSerializer(DateTimeKind.Local)); ;
            });
        }
    }
}

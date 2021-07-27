using Infrastructure.Core;
using MongoDB.Bson.Serialization;

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
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Title).SetIsRequired(true);
            });
        }
    }
}

﻿using Infrastructure.Core;
using MongoDB.Bson.Serialization;
using System;

namespace Infrastructure.Write.User
{
    public class UserBsonMapper : IMapper
    {
        public void Map()
        {
            BsonClassMap.RegisterClassMap<UserWriteDto>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);

                map.MapMember(x => x.Name).SetIsRequired(true);
                map.MapMember(x => x.Surname).SetIsRequired(true);
                map.MapMember(x => x.Email).SetIsRequired(true);
                map.MapMember(x => x.LoginWith).SetIsRequired(true);
            });
        }
    }
}

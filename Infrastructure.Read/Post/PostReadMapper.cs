﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Infrastructure.Read.Post
{
    public class PostReadMapper
    {
        [BsonId]
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string ImageThumb { get; set; }

        [DataMember]
        public string ImageMain { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int[] Tags { get; set; }

        [DataMember]
        public string CreateBy { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public DateTime UpdateDate { get; set; }

        [DataMember]
        public DateTime? PublishDate { get; set; } 
        
        [DataMember]
        public List<PostCommentReadDto> Comments { get; set; }   

        [DataMember]
        public List<Guid> PostsRelated { get; set; }
    }
}

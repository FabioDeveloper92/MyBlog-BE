using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace Web.Api.Models.Auth
{
    [CollectionName("AspNet_Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
    }
}

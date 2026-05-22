using MongoDbGenericRepository.Attributes;
using AspNetCore.Identity.MongoDbCore.Models;

namespace IdentityMongodb.Models
{
    [CollectionName("Roles")]
    public class ApplicationRoles : MongoIdentityRole<Guid>
    {

    }

}

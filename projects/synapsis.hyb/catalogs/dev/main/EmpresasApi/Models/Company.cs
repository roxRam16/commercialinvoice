using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CompaniesApi.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        //Agregamos validaciones
        //[Required]
        [Required(ErrorMessage = "{0} is required")]
        public required string CompanyName { get; set; }

        public int Status { get; set; }

        public int State { get; set; }
    }
}

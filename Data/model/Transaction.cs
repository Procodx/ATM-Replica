using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ATM_Replica.Data_folder.model_folder
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string userId { get; set; }
        public string? receiverEmail { get; set; }
        public string? TransactionType { get; set; }
        public string? description { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal amount { get; set; }
    }
}

using GameCtor.Repository;
using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class BaseEntity : IModel
    {
        [JsonIgnore]
        public string Id { get; set; }
    }
}
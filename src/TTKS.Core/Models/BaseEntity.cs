using GameCtor.Repository;
using Newtonsoft.Json;

namespace TTKS.Core.Models
{
    public class BaseEntity : IModel
    {
        [JsonIgnore]
        public string Id { get; set; }
    }
}
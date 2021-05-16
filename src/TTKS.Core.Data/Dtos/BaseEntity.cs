using GameCtor.Repository;
using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class BaseEntity : IModel
    {
        [JsonIgnore]
        public string Id { get; set; }
    }
}
using GameCtor.Repository;

namespace TTKS.Core.Models
{
    public class BaseEntity : IModel
    {
        public string Id { get; set; }
    }
}
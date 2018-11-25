using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IHomonymItemViewModel
    {
        StringEntity Model { get; }

        string Value { get; set; }
    }
}

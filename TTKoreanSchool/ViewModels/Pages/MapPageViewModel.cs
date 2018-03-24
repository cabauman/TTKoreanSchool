using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTKoreanSchool.ViewModels
{
    public interface IMapPageViewModel : IPageViewModel
    {
    }

    public class MapPageViewModel : BasePageViewModel, IMapPageViewModel
    {
    }
}
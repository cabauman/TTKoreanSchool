using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTKoreanSchool.ViewModels
{
    public interface IHangulZoneViewModel : IScreenViewModel
    {
    }

    public class HangulZoneViewModel : BaseScreenViewModel, IHangulZoneViewModel
    {
    }
}
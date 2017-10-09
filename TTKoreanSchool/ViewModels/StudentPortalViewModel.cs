using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTKoreanSchool.ViewModels
{
    public interface IStudentPortalViewModel : IScreenViewModel
    {
    }

    public class StudentPortalViewModel : BaseViewModel, IStudentPortalViewModel
    {
    }
}

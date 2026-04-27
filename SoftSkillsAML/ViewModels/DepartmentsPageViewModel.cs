using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftSkillsAML.ViewModels
{
    internal class DepartmentsPageViewModel: ViewModelBase
    {
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }
    }
}

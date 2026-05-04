using ReactiveUI;
using SoftSkillsAML.Models;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected static int _id;

        public static int CurrentUserId
        {
            get => _id;
            set => _id = value;
        }

        public static User? CurrentUser => MainWindowViewModel.db.Users.FirstOrDefault(x => x.Id == _id);
    }
}

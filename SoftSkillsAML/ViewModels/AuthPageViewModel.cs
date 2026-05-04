using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoftSkillsAML.ViewModels
{
    internal class AuthPageViewModel: ViewModelBase
    {
        string _login = string.Empty;
        string _password = string.Empty;

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        bool _isPasswordVisible = false;

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => this.RaiseAndSetIfChanged(ref _isPasswordVisible, value);
        }

        string _eye = "closed_eye.png";

        public string Eye
        {
            get => _eye;
            set => this.RaiseAndSetIfChanged(ref _eye, value);
        }

        public void ChangePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            Eye = Eye == "closed_eye.png" ? "opened_eye.png" : "closed_eye.png";
        }

        public async void Auth()
        {
            User? user = MainWindowViewModel.db.Users.FirstOrDefault(x => x.Login == Login && x.Password == MD5.HashData(Encoding.ASCII.GetBytes(Password)));
            if (user == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пользователь не найден", ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
                return;
            }

            if (user.IsBlocked)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ваш аккаунт заблокирован администратором", ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
                return;
            }

            CurrentUserId = user.Id;
            EnsureUserLinks(user);
            MainWindowViewModel.db.SaveChanges();

            MainWindowViewModel.Instance.Page = user.IsAdmin ? new StatisticPageView() : new DepartmentsPageView();
        }

        private static void EnsureUserLinks(User user)
        {
            List<Achievement> achievements = MainWindowViewModel.db.Achievements.ToList();
            List<Question> questions = MainWindowViewModel.db.Questions.ToList();
            List<SoftSkill> softSkills = MainWindowViewModel.db.SoftSkills.ToList();
            foreach (var achievement in achievements)
            {
                if (!MainWindowViewModel.db.UserAchievements.Any(x => x.User == user.Id && x.Achievement == achievement.Id))
                {
                    MainWindowViewModel.db.UserAchievements.Add(new UserAchievement() { UserNavigation = user, AchievementNavigation = achievement });
                }
            }
            foreach (var question in questions)
            {
                if (!MainWindowViewModel.db.UserQuestions.Any(x => x.User == user.Id && x.Question == question.Id))
                {
                    MainWindowViewModel.db.UserQuestions.Add(new UserQuestion() { UserNavigation = user, QuestionNavigation = question });
                }
            }
            foreach (var softSkill in softSkills)
            {
                if (!MainWindowViewModel.db.UserSoftSkills.Any(x => x.User == user.Id && x.SoftSkill == softSkill.Id))
                {
                    MainWindowViewModel.db.UserSoftSkills.Add(new UserSoftSkill() { UserNavigation = user, SoftSkillNavigation = softSkill });
                }
            }
        }

        public void ToReg()
        {
            MainWindowViewModel.Instance.Page = new RegPageView();
        }
    }
}

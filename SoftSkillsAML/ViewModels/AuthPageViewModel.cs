using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            switch (Eye)
            {
                case "closed_eye.png":
                    Eye = "opened_eye.png";
                    break;
                case "opened_eye.png":
                    Eye = "closed_eye.png";
                    break;
            }
        }

        public async void Auth()
        {
            ViewModelBase._id = 7;
            User? user = MainWindowViewModel.db.Users.FirstOrDefault(x => x.Login == Login && x.Password == MD5.HashData(Encoding.ASCII.GetBytes(Password)));
            if (user == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пользователь не найден", MsBox.Avalonia.Enums.ButtonEnum.Ok,Icon.Info);
                await message.ShowAsync();
                return;
            }
            if (user.IsAdmin == true)
            {
                MainWindowViewModel.Instance.Page = new StatisticPageView();
                return;
            }
            List<Achievement> achievements = MainWindowViewModel.db.Achievements.ToList();
            List<Question> questions = MainWindowViewModel.db.Questions.ToList();
            List<SoftSkill> softSkills = MainWindowViewModel.db.SoftSkills.ToList();
            foreach (var achievement in achievements)
            {
                try
                {
                    MainWindowViewModel.db.UserAchievements.Add(new UserAchievement() { UserNavigation = user, AchievementNavigation = achievement });
                }
                catch
                {

                }
            }
            foreach (var question in questions)
            {
                try
                {
                    MainWindowViewModel.db.UserQuestions.Add(new UserQuestion() { UserNavigation = user, QuestionNavigation = question });
                }
                catch
                {

                }
            }
            foreach (var softSkill in softSkills)
            {
                try
                {
                    MainWindowViewModel.db.UserSoftSkills.Add(new UserSoftSkill() { UserNavigation = user, SoftSkillNavigation = softSkill });
                }
                catch
                {

                }
            }
            MainWindowViewModel.Instance.Page = new DepartmentsPageView();

        }

        public void ToReg()
        {
            MainWindowViewModel.Instance.Page = new RegPageView();
        }
    }
}

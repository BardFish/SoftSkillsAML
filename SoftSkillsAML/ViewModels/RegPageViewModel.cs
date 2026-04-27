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
    internal class RegPageViewModel: ViewModelBase
    {
        User _newUser = new User() { Birthday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) };

        public User NewUser
        {
            get => _newUser;
            set => this.RaiseAndSetIfChanged(ref _newUser, value);
        }

        string _password = string.Empty;

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

        public List<Gender> Genders => MainWindowViewModel.db.Genders.ToList();

        public void ChangePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            switch(Eye)
            {
                case "closed_eye.png":
                    Eye = "opened_eye.png";
                    break;
                case "opened_eye.png":
                    Eye = "closed_eye.png";
                    break;
            }
        }

        public async void Reg()
        {            
            if (DateTime.Today.CompareTo(NewUser.Birthday.AddYears(18)) > 0)
            {
                NewUser.Password = MD5.HashData(Encoding.ASCII.GetBytes(Password));
                MainWindowViewModel.db.Users.Add(NewUser);
                List<Achievement> achievements = MainWindowViewModel.db.Achievements.ToList();
                List<Question> questions = MainWindowViewModel.db.Questions.ToList();
                List<SoftSkill> softSkills = MainWindowViewModel.db.SoftSkills.ToList();
                foreach (var achievement in achievements)
                {
                    try
                    {
                        MainWindowViewModel.db.UserAchievements.Add(new UserAchievement() { UserNavigation = NewUser, AchievementNavigation = achievement });
                    }
                    catch
                    {

                    }
                }
                foreach (var question in questions)
                {
                    try
                    {
                        MainWindowViewModel.db.UserQuestions.Add(new UserQuestion() { UserNavigation = NewUser, QuestionNavigation = question });
                    }
                    catch
                    {

                    }
                }
                foreach (var softSkill in softSkills)
                {
                    try
                    {
                        MainWindowViewModel.db.UserSoftSkills.Add(new UserSoftSkill() { UserNavigation = NewUser, SoftSkillNavigation = softSkill });
                    }
                    catch
                    {

                    }
                }
                try
                {
                    MainWindowViewModel.db.SaveChanges();
                    MainWindowViewModel.Instance.Page = new DepartmentsPageView();
                }
                catch
                {

                }
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Возраст меньше 18", MsBox.Avalonia.Enums.ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
            }
        }

        public void ToAuth()
        {
            MainWindowViewModel.Instance.Page = new AuthPageView();
        }
    }
}

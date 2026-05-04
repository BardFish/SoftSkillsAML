using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoftSkillsAML.ViewModels
{
    internal class RegPageViewModel: ViewModelBase
    {
        User _newUser = new User() { Birthday = DateTime.Today };

        public User NewUser
        {
            get => _newUser;
            set => this.RaiseAndSetIfChanged(ref _newUser, value);
        }

        Gender? _selectedGender;
        public Gender? SelectedGender
        {
            get => _selectedGender;
            set => this.RaiseAndSetIfChanged(ref _selectedGender, value);
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
            Eye = Eye == "closed_eye.png" ? "opened_eye.png" : "closed_eye.png";
        }

        public async void Reg()
        {
            if (SelectedGender == null || string.IsNullOrWhiteSpace(NewUser.Login) || string.IsNullOrWhiteSpace(Password))
            {
                var message1 = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните все поля", ButtonEnum.Ok, Icon.Info);
                await message1.ShowAsync();
                return;
            }

            if (DateTime.Today.CompareTo(NewUser.Birthday.AddYears(18)) <= 0)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Возраст меньше 18", ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
                return;
            }

            if (MainWindowViewModel.db.Users.Any(x => x.Login == NewUser.Login))
            {
                var message2 = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Логин уже занят", ButtonEnum.Ok, Icon.Info);
                await message2.ShowAsync();
                return;
            }

            NewUser.Gender = SelectedGender.Id;
            NewUser.Password = MD5.HashData(Encoding.ASCII.GetBytes(Password));
            MainWindowViewModel.db.Users.Add(NewUser);
            MainWindowViewModel.db.SaveChanges();

            CurrentUserId = NewUser.Id;
            List<Achievement> achievements = MainWindowViewModel.db.Achievements.ToList();
            List<Question> questions = MainWindowViewModel.db.Questions.ToList();
            List<SoftSkill> softSkills = MainWindowViewModel.db.SoftSkills.ToList();
            foreach (var achievement in achievements)
            {
                MainWindowViewModel.db.UserAchievements.Add(new UserAchievement() { User = NewUser.Id, Achievement = achievement.Id });
            }
            foreach (var question in questions)
            {
                MainWindowViewModel.db.UserQuestions.Add(new UserQuestion() { User = NewUser.Id, Question = question.Id });
            }
            foreach (var softSkill in softSkills)
            {
                MainWindowViewModel.db.UserSoftSkills.Add(new UserSoftSkill() { User = NewUser.Id, SoftSkill = softSkill.Id });
            }

            MainWindowViewModel.db.SaveChanges();
            MainWindowViewModel.Instance.Page = new DepartmentsPageView();
        }

        public void ToAuth()
        {
            MainWindowViewModel.Instance.Page = new AuthPageView();
        }
    }
}

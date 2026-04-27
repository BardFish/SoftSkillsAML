using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class AddQuestionPageViewModel : ViewModelBase
    {
        public ObservableCollection<Department> Departments { get; } = new(MainWindowViewModel.db.Departments.OrderBy(x => x.Name).ToList());
        public ObservableCollection<SoftSkill> SoftSkills { get; } = new(MainWindowViewModel.db.SoftSkills.OrderBy(x => x.Name).ToList());
        public ObservableCollection<AnswerDraft> Answers { get; } = new();

        Department? _selectedDepartment;
        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set => this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
        }

        AnswerDraft? _selectedAnswer;
        public AnswerDraft? SelectedAnswer
        {
            get => _selectedAnswer;
            set => this.RaiseAndSetIfChanged(ref _selectedAnswer, value);
        }

        string _questionText = string.Empty;
        public string QuestionText
        {
            get => _questionText;
            set => this.RaiseAndSetIfChanged(ref _questionText, value);
        }

        public AddQuestionPageViewModel()
        {
            AddAnswer();
            AddAnswer();
        }

        public void AddAnswer()
        {
            var draft = new AnswerDraft();
            foreach (var skill in SoftSkills)
            {
                draft.SkillPoints.Add(new SkillPointsDraft
                {
                    SoftSkillId = skill.Id,
                    SoftSkillName = skill.Name,
                    PointsInput = "0"
                });
            }

            Answers.Add(draft);
            SelectedAnswer = draft;
        }

        public void RemoveSelectedAnswer()
        {
            if (SelectedAnswer == null) return;
            Answers.Remove(SelectedAnswer);
            SelectedAnswer = Answers.FirstOrDefault();
        }

        public async void AddQuestion()
        {
            if (SelectedDepartment == null || string.IsNullOrWhiteSpace(QuestionText))
            {
                var err = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните текст и направление", ButtonEnum.Ok, Icon.Info);
                await err.ShowAsync();
                return;
            }

            var filledAnswers = Answers.Where(a => !string.IsNullOrWhiteSpace(a.Text)).ToList();
            if (filledAnswers.Count < 2)
            {
                var err = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Добавьте минимум два варианта ответа", ButtonEnum.Ok, Icon.Info);
                await err.ShowAsync();
                return;
            }

            var question = new Question
            {
                Text = QuestionText,
                Department = SelectedDepartment.Id,
                HasImage = false
            };
            MainWindowViewModel.db.Questions.Add(question);
            MainWindowViewModel.db.SaveChanges();

            var questionId = question.Id;

            foreach (var answerDraft in filledAnswers)
            {
                var answer = new Answer
                {
                    Question = questionId,
                    Text = answerDraft.Text,
                    IsImage = false,
                    IsCorrect = answerDraft.IsCorrect
                };
                MainWindowViewModel.db.Answers.Add(answer);
                MainWindowViewModel.db.SaveChanges();

                foreach (var skill in answerDraft.SkillPoints)
                {
                    var points = int.TryParse(skill.PointsInput, out var parsed) ? parsed : 0;
                    MainWindowViewModel.db.AnswerSoftSkills.Add(new AnswerSoftSkill
                    {
                        Answer = answer.Id,
                        SoftSkill = skill.SoftSkillId,
                        Points = points
                    });
                }
                MainWindowViewModel.db.SaveChanges();
            }

            var ok = MessageBoxManager.GetMessageBoxStandard("Успех", "Задание с вариантами ответов добавлено", ButtonEnum.Ok, Icon.Success);
            await ok.ShowAsync();

            QuestionText = string.Empty;
            Answers.Clear();
            AddAnswer();
            AddAnswer();
        }

        public void BackToStat() => MainWindowViewModel.Instance.Page = new StatisticPageView();
    }

    internal class AnswerDraft : ReactiveObject
    {
        string _text = string.Empty;
        bool _isCorrect;

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public bool IsCorrect
        {
            get => _isCorrect;
            set => this.RaiseAndSetIfChanged(ref _isCorrect, value);
        }

        public ObservableCollection<SkillPointsDraft> SkillPoints { get; } = new();
    }

    internal class SkillPointsDraft : ReactiveObject
    {
        string _pointsInput = "0";

        public int SoftSkillId { get; set; }
        public string SoftSkillName { get; set; } = string.Empty;

        public string PointsInput
        {
            get => _pointsInput;
            set => this.RaiseAndSetIfChanged(ref _pointsInput, value);
        }
    }
}

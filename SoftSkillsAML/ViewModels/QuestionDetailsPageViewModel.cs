using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class QuestionDetailsPageViewModel : ViewModelBase
    {
        public Question Question { get; }
        public ObservableCollection<Answer> Answers { get; }

        Answer? _selectedAnswer;
        public Answer? SelectedAnswer
        {
            get => _selectedAnswer;
            set => this.RaiseAndSetIfChanged(ref _selectedAnswer, value);
        }

        public QuestionDetailsPageViewModel(int questionId)
        {
            Question = MainWindowViewModel.db.Questions.First(x => x.Id == questionId);
            Answers = new ObservableCollection<Answer>(MainWindowViewModel.db.Answers.Where(x => x.Question == questionId).ToList());
        }

        public async void SubmitAnswer()
        {
            if (SelectedAnswer == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите вариант ответа", ButtonEnum.Ok, Icon.Info);
                await message.ShowAsync();
                return;
            }

            var userQuestion = MainWindowViewModel.db.UserQuestions.First(x => x.User == CurrentUserId && x.Question == Question.Id);
            userQuestion.IsAnswered = true;
            userQuestion.Answer = SelectedAnswer.Id;

            var skillBonuses = MainWindowViewModel.db.AnswerSoftSkills.Where(x => x.Answer == SelectedAnswer.Id).ToList();
            foreach (var bonus in skillBonuses)
            {
                var userSkill = MainWindowViewModel.db.UserSoftSkills.FirstOrDefault(x => x.User == CurrentUserId && x.SoftSkill == bonus.SoftSkill);
                if (userSkill != null)
                {
                    userSkill.Points += bonus.Points;
                }
            }

            MainWindowViewModel.db.SaveChanges();
            MainWindowViewModel.Instance.Page = new QuestionsPageView(Question.Department);
        }

        public void BackToQuestions()
        {
            MainWindowViewModel.Instance.Page = new QuestionsPageView(Question.Department);
        }
    }
}

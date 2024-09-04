using LOI_Job_Generator.Core;
using LOI_Job_Generator.Events;
using LOI_Job_Generator.Events.EventArg;
using LOI_Job_Generator.Model;
using LOI_Job_Generator.View;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LOI_Job_Generator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ItemView> ItemModels { get; set; } = new ObservableCollection<ItemView>();
        public ObservableCollection<ItemViewModel> ItemViewModels { get; set; } = new ObservableCollection<ItemViewModel>();

        private List<PersoonModel> persoonModels = new List<PersoonModel>();

        public ICommand AddCommand { get; }
        public ICommand InitialisatieCommand { get; }

        private string buttonVisibility;
        public string ButtonVisibility
        {
            get { return buttonVisibility; }
            set
            {
                buttonVisibility = value;
                OnPropertyChanged(nameof(ButtonVisibility));
            }
        }

        private string progressBarVisibility;
        public string ProgressBarVisibility
        {
            get { return progressBarVisibility; }
            set
            {
                progressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }

        private bool editMode;
        public bool EditMode
        {
            get { return editMode; }
            set
            {
                editMode = value;
                OnPropertyChanged(nameof(EditMode));

                switch (editMode)
                {
                    case true:
                        ButtonVisibility = VisibilityEnum.Visible.ToString();
                        break;
                    case false:
                        ButtonVisibility = VisibilityEnum.Hidden.ToString();
                        break;
                }

                EventService.NotifyEditModeChanged(editMode);
            }
        }

        private bool expertMode;
        public bool ExpertMode
        {
            get { return expertMode; }
            set
            {
                expertMode = value;
                OnPropertyChanged(nameof(ExpertMode));

                EventService.NotifyExpertModeChanged(expertMode);
            }
        }

        private string appStatus;
        public string AppStatus
        {
            get { return appStatus; }
            set
            {
                appStatus = value;
                OnPropertyChanged(nameof(AppStatus));
            }
        }

        private int appProgressValue;
        public int AppProgressValue
        {
            get { return appProgressValue; }
            set
            {
                appProgressValue = value;
                OnPropertyChanged(nameof(AppProgressValue));
            }
        }

        private int appProgressMax;
        public int AppProgressMax
        {
            get { return appProgressMax; }
            set
            {
                appProgressMax = value;
                OnPropertyChanged(nameof(AppProgressMax));
            }
        }

        public MainViewModel()
        {
            AppStatus = StatusEnum.Inactief.ToString();

            EventService.ErrorCatch += OnErrorCatch;
            EventService.GenerateStarted += OnGenerateStarted;
            EventService.GenerateFinished += OnGenerateFinished;
            EventService.RemoveItem += OnRemoveItem;
            EventService.UpdateItemCollection += OnUpdateItemCollection;
            EventService.UpdateProgressValue += OnUpdateProgressValue;
            EventService.CloseAddWindow += OnCloseAddWindow;

            GetPersoonModels();

            ButtonVisibility = VisibilityEnum.Hidden.ToString();
            ProgressBarVisibility = VisibilityEnum.Hidden.ToString();

            AddCommand = new RelayCommand(Add, CanAdd);
            InitialisatieCommand = new RelayCommand(Initialisatie, CanInitialisatie);
        }

        private bool CanAdd(object obj)
        {
            return true;
        }

        private bool CanInitialisatie(object obj)
        {
            if (ItemViewModels.Count != 0)
                return false;

            return true;
        }

        private void Add(object obj)
        {
            AddPersoonView addPersoonView = new AddPersoonView(ExpertMode);
            addPersoonView.Show();
        }

        private void Initialisatie(object obj)
        {
            var messageBox = MessageBox.Show("Wil je de database opnieuw initialiseren?",
                "Database initialisatie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            AppStatus = StatusEnum.Initialisatie.ToString();

            if (messageBox == MessageBoxResult.Yes)
            {
                Thread thread = new Thread(JobGeneratorManager.InitialisatieDB);
                thread.Start();
            }
        }

        private void GetPersoonModels()
        {
            try
            {
                persoonModels = PersoonModel.Read();
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (ExpertMode != true)
                    message = "Er is een fout opgetreden, neem contact op met de ontwikkelaar.";

                EventService.NotifyErrorCatch(message);
            }

            if (ItemModels.Count != 0)
                ItemModels.Clear();

            foreach (PersoonModel persoonModel in persoonModels)
            {
                ItemModels.Add(new ItemView(persoonModel));
            }
        }

        private void OnErrorCatch(object? sender, EventMessage e)
        {
            MessageBox.Show(e.eventMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnGenerateStarted(object? sender, EventArgs e)
        {
            ItemViewModels.Add((ItemViewModel)sender);
            ((RelayCommand)InitialisatieCommand).RaiseCanExecuteChanged();

            EventService.NotifyItemCollectionChanged(this);

            if (ItemViewModels.Count == 1)
            {
                AppStatus = StatusEnum.Berekenen.ToString();
                ProgressBarVisibility = VisibilityEnum.Visible.ToString();
                AppProgressValue = 0;
                AppProgressMax = 100;
            }
            else
                AppProgressMax = AppProgressMax + 100;
        }

        private void OnGenerateFinished(object? sender, EventArgs e)
        {
            ItemViewModels.Remove((ItemViewModel)sender);
            ((RelayCommand)InitialisatieCommand).RaiseCanExecuteChanged();

            EventService.NotifyItemCollectionChanged(this);

            if (ItemViewModels.Count == 0)
            {
                AppStatus = StatusEnum.Inactief.ToString();
                ProgressBarVisibility = VisibilityEnum.Hidden.ToString();
            }
        }
        
        private void OnRemoveItem(object? sender, EventArgs e)
        {
            try
            {
                ItemViewModel bufferItem = (ItemViewModel)sender;

                PersoonModel.Delete((int)bufferItem.bufferPersoonModel.iId);

                ItemModels.Remove(ItemModels.FirstOrDefault(item => item.DataContext == (ItemViewModel)sender));
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (ExpertMode != true)
                    message = "Er is een fout opgetreden, neem contact op met de ontwikkelaar.";

                EventService.NotifyErrorCatch(message);
            }
        }

        private void OnUpdateItemCollection(object? sender, EventArgs e)
        {
            if (App.Current.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                GetPersoonModels();
                AppStatus = StatusEnum.Inactief.ToString();
                EventService.NotifyEditModeChanged(EditMode);
            }
            else
            {
                App.Current.Dispatcher.Invoke(new Action(() => {
                    GetPersoonModels();
                    AppStatus = StatusEnum.Inactief.ToString();
                    EventService.NotifyEditModeChanged(EditMode);
                }));
            }
        }

        private void OnUpdateProgressValue(object? sender, EventArgs e)
        {
            AppProgressValue = AppProgressValue + 1;
        }

        private void OnCloseAddWindow(object? sender, EventArgs e)
        {
            if (((AddPersoonViewModel)sender).bufferPersoonModel != null)
            {
                try
                {
                    List<PersoonModel> bufferPersoonModels = PersoonModel.Read();

                    ItemModels.Add(new ItemView(bufferPersoonModels.Last()));

                    ((ItemViewModel)ItemModels.Last().DataContext).ButtonVisibility = VisibilityEnum.Visible.ToString();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;

                    if (ExpertMode != true)
                        message = "Er is een fout opgetreden, neem contact op met de ontwikkelaar.";

                    EventService.NotifyErrorCatch(message);
                }
            }
        }
    }
}

using LOI_Job_Generator.Core;
using LOI_Job_Generator.Events;
using LOI_Job_Generator.Events.EventArg;
using LOI_Job_Generator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LOI_Job_Generator.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        public PersoonModel bufferPersoonModel;

        private Thread generateThread;

        public ICommand GenerateCommand { get; }
        public ICommand RemoveCommand {  get; }

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

        public string PersoonNaam { get; set; }

        private string persoonBaan;
        public string PersoonBaan 
        {  
            get { return persoonBaan; }
            set
            {
                persoonBaan = value;
                OnPropertyChanged(nameof(PersoonBaan));

                if (generateThread != null)
                {
                    bufferPersoonModel.tBaan = persoonBaan;
                    PersoonModel.Update(bufferPersoonModel, (int)bufferPersoonModel.iId);

                    EventService.NotifyGenerateFinished(this);

                    while (generateThread.ThreadState == System.Threading.ThreadState.Running) { }

                    ((RelayCommand)GenerateCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private int persoonPrograss;
        public int PersoonProgress 
        {
            get { return persoonPrograss; }
            set
            {
                persoonPrograss = value;
                OnPropertyChanged(nameof(PersoonProgress));
            }
        }

        public ItemViewModel(PersoonModel persoonModel)
        {
            bufferPersoonModel = persoonModel;
            PersoonNaam = bufferPersoonModel.tNaam + ", " + bufferPersoonModel.iLeeftijd;
            PersoonBaan = bufferPersoonModel.tBaan;

            ButtonVisibility = VisibilityEnum.Hidden.ToString();

            EventService.EditModeChanged += OnEditModeChanged;

            GenerateCommand = new RelayCommand(Generate, CanGenerate);
            RemoveCommand = new RelayCommand(Remove, CanRemove);
        }

        private bool CanGenerate(object obj)
        {
            if (generateThread != null && generateThread.ThreadState == System.Threading.ThreadState.Running)
                return false;

            return true;
        }

        private bool CanRemove(object obj)
        {
            if (generateThread != null && generateThread.ThreadState == System.Threading.ThreadState.Running)
                return false;

            return true;
        }

        private void Generate(object obj)
        {
            PersoonProgress = 0;

            EventService.NotifyGenerateStarted(this);

            generateThread = new Thread(GenerateBestJob);

            generateThread.Start();

            ((RelayCommand)GenerateCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
        }

        private void Remove(object obj)
        {
            var messageBox = MessageBox.Show("Wil je deze persoon verwijderen?",
                "Persoon verwijderen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (messageBox == MessageBoxResult.Yes)
                EventService.NotifyRemoveItem(this);
        }

        private void GenerateBestJob()
        {
            List<OpleidingModel> opleidingModels = OpleidingModel.Read();
            List<BeroepModel> beroepModels = BeroepModel.Read();

            foreach (BeroepModel beroepModel in beroepModels)
            {
                foreach (OpleidingModel opleidingModel in opleidingModels)
                {
                    Debug.WriteLine($"{opleidingModel.tOpleiding} en {beroepModel.tBeroep} berekenen.");

                    var pauze = JobGeneratorManager.RandomGenerator(100, 1000);
                    Thread.Sleep(pauze);

                    var valueBerekend = opleidingModel.ValueRandom * beroepModel.ValueRandom;
                    Debug.WriteLine($"De kans van slagen met deze opleiding en baan is: {valueBerekend}");

                    if (valueBerekend > beroepModel.ValueBerekend)
                        beroepModel.ValueBerekend = valueBerekend;

                    PersoonProgress = PersoonProgress + 1;
                    EventService.NotifyUpdateProgressValue();
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                PersoonBaan = beroepModels.MaxBy(item => item.ValueBerekend).tBeroep;
            });
        }

        private void OnEditModeChanged(object? sender, EventBool e)
        {
            switch (e.eventBool)
            {
                case true:
                    ButtonVisibility = VisibilityEnum.Visible.ToString();
                    break;
                case false:
                    ButtonVisibility = VisibilityEnum.Hidden.ToString();
                    break;
            }
        }
    }
}

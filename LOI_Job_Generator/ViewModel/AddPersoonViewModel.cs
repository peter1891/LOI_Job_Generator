using LOI_Job_Generator.Core;
using LOI_Job_Generator.Events;
using LOI_Job_Generator.Events.EventArg;
using LOI_Job_Generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LOI_Job_Generator.ViewModel
{
    public class AddPersoonViewModel : ViewModelBase
    {
        public PersoonModel bufferPersoonModel;

        public ICommand ToevoegenCommand { get; set; }
        public ICommand AnnulerenCommand {  get; }

        private bool expertMode;

        private string persoonNaam;
        public string PersoonNaam
        {
            get { return persoonNaam; } 
            set
            {
                persoonNaam = value;
                OnPropertyChanged(nameof(PersoonNaam));
                ((RelayCommand)ToevoegenCommand).RaiseCanExecuteChanged();
            }
        }

        private string persoonLeeftijd;
        public string PersoonLeeftijd
        {
            get { return persoonLeeftijd; }
            set
            {
                persoonLeeftijd = value;
                OnPropertyChanged(nameof(PersoonLeeftijd));
                ((RelayCommand)ToevoegenCommand).RaiseCanExecuteChanged();
            }
        }

        public AddPersoonViewModel(bool exportMode)
        {
            this.expertMode = exportMode;
            EventService.ExpertModeChanged += OnExpertModeChanged;

            ToevoegenCommand = new RelayCommand(Toevoegen, CanToevoegen);
            AnnulerenCommand = new RelayCommand(Annuleren, CanAnnuleren);
        }

        private bool CanToevoegen(object obj)
        {
            if (!string.IsNullOrEmpty(PersoonNaam) && !string.IsNullOrEmpty(PersoonLeeftijd) && int.TryParse(PersoonLeeftijd, out int temp))
                return true;

            return false;
        }

        private bool CanAnnuleren(object obj)
        {
            return true;
        }

        private void Toevoegen(object obj)
        {
            long bufferPersoonLeeftijd = int.Parse(PersoonLeeftijd);

            bufferPersoonModel = new PersoonModel()
            {
                tNaam = PersoonNaam,
                iLeeftijd = bufferPersoonLeeftijd
            };

            try
            {
                PersoonModel.Create(bufferPersoonModel);
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (expertMode != true)
                    message = "Er is een fout opgetreden, neem contact op met de ontwikkelaar.";

                EventService.NotifyErrorCatch(message);
            }

            EventService.NotifyCloseAddWindow(this);
        }

        private void Annuleren(object obj)
        {
            EventService.NotifyCloseAddWindow(this);
        }

        private void OnExpertModeChanged(object? sender, EventBool e)
        {
            expertMode = e.eventBool;
        }
    }
}

using System;
using LOI_Job_Generator.Events.EventArg;

namespace LOI_Job_Generator.Events
{
    public static class EventService
    {
        public static event EventHandler<EventMessage> ErrorCatch;
        public static event EventHandler<EventBool> EditModeChanged;
        public static event EventHandler<EventBool> ExpertModeChanged;

        public static event EventHandler GenerateStarted;
        public static event EventHandler GenerateFinished;
        public static event EventHandler RemoveItem;
        public static event EventHandler UpdateItemCollection;
        public static event EventHandler UpdateProgressValue;
        public static event EventHandler ItemCollectionChanged;
        public static event EventHandler CloseAddWindow;

        public static void NotifyErrorCatch(string message)
        {
            if (ErrorCatch != null)
                ErrorCatch(null, new EventMessage(message));
        }

        public static void NotifyEditModeChanged(bool value)
        {
            if (EditModeChanged != null)
                EditModeChanged(null, new EventBool(value));
        }

        public static void NotifyExpertModeChanged(bool value)
        {
            if (ExpertModeChanged != null)
                ExpertModeChanged(null, new EventBool(value));
        }

        public static void NotifyGenerateStarted(object obj)
        {
            if (GenerateStarted != null)
                GenerateStarted(obj, EventArgs.Empty);
        }

        public static void NotifyGenerateFinished(object obj)
        {
            if (GenerateFinished != null)
                GenerateFinished(obj, EventArgs.Empty);
        }

        public static void NotifyRemoveItem(object obj)
        {
            if (RemoveItem != null)
                RemoveItem(obj, EventArgs.Empty);
        }

        public static void NotifyUpdateItemCollection()
        {
            if (UpdateItemCollection != null)
                UpdateItemCollection(null, EventArgs.Empty);
        }

        public static void NotifyUpdateProgressValue()
        {
            if (UpdateProgressValue != null)
                UpdateProgressValue(null, EventArgs.Empty);
        }

        public static void NotifyItemCollectionChanged(object obj)
        {
            if (ItemCollectionChanged != null)
                ItemCollectionChanged(obj, EventArgs.Empty);
        }

        public static void NotifyCloseAddWindow(object obj)
        {
            if (CloseAddWindow != null)
                CloseAddWindow(obj, EventArgs.Empty);
        }
    }
}

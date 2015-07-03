﻿using log4net.Appender;
using log4net.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace ChocolateyMilk
{
    [Magic]
    public class Diagnostics : IAppender
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Logging { get; } = new ObservableCollection<string>();
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        string IAppender.Name { get; set; }

        void IAppender.Close() { }

        void IAppender.DoAppend(LoggingEvent loggingEvent)
        {
            dispatcher.Invoke(() =>
            {
                Logging.Add(loggingEvent.MessageObject.ToString());

                if (Logging.Count > 5100)
                {
                    while (Logging.Count > 5000)
                    {
                        Logging.RemoveAt(0);
                    }
                }
            });
        }

        private void RaisePropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

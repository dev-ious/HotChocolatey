﻿using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;

namespace ChocolateyMilk
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ChocolateyController Controller { get; } = new ChocolateyController();
        public Packages Packages { get; } = new Packages();
        public ObservableCollection<IFilter> FilterSelections { get; } = new ObservableCollection<IFilter>();
        public IFilter Filter
        {
            get { return selection; }
            set
            {
                if (selection != value)
                {
                    selection = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }

        private IFilter selection;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeFilter();

            await Controller.GetVersion();
            await Refresh();
        }

        private async Task Refresh()
        {
            (await Controller.GetInstalled()).ForEach(Packages.Add);
            (await Controller.GetUpgradable()).ForEach(Packages.Add);
            (await Controller.GetAvailable()).ForEach(Packages.Add);
        }

        private void InitializeFilter()
        {
            FilterFactory.AvailableFilters.ForEach(FilterSelections.Add);
            Filter = FilterSelections[0];
        }

        private void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Packages.ApplyFilter(Filter.Filter);
        }

        private async void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private void OnMarkAllUpgradesClick(object sender, RoutedEventArgs e)
        {
            Packages.Items.Where(t => t.IsInstalledUpgradable).ToList().ForEach(t => t.IsMarkedForInstallation = true);
        }
    }
}

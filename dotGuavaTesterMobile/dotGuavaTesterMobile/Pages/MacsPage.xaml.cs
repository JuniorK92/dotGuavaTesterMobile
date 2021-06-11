using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dotGuavaTesterMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MacsPage : ContentPage
    {
        class MacAddress
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Detail { get; set; }
        }
        class MacsViewModel : INotifyPropertyChanged
        {
            #region Properties            

            ObservableCollection<MacAddress> _macAddressList;
            public ObservableCollection<MacAddress> MacAddressList
            {
                get { return _macAddressList; }
                set
                {
                    _macAddressList = value;
                    OnPropertyChanged();
                }
            }

            bool _isLoading;
            public bool IsLoading
            {
                get { return _isLoading; }
                set
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }

            #endregion

            #region Constructor
            public MacsViewModel()
            {
                MacAddressList = new ObservableCollection<MacAddress>();
                LoadData();
            }
            #endregion

            #region Commands

            public Command LoadDataCommand
            {
                get
                {
                    return new Command(LoadData);
                }
            }

            #endregion

            #region Methods

            async void LoadData()
            {
                await Task.Run(() => FullfillMacList());
            }
            void FullfillMacList()
            {
                IsLoading = true;

                MacAddressList.Clear();

                try
                {
                    int counter = 0;
                    foreach (var x in dotGuava.Essential.Mac.GetDeviceNetworkInterfaces())
                    {
                        MacAddressList.Add(new MacAddress { ID = ++counter, Name = x.Name, Detail = $"{dotGuava.Essential.Mac.ParseAddress(x.GetPhysicalAddress().ToString())} ({x.OperationalStatus})" });
                    }
                }
                catch(Exception)
                {
                    MacAddressList.Clear();
                }

                IsLoading = false;
            }

            #endregion

            #region INotifyPropertyChanged Implementation

            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }

        public MacsPage()
        {
            InitializeComponent();

            BindingContext = new MacsViewModel();
        }
    }
}
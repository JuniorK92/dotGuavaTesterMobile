using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace dotGuavaTesterMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CypherPage : ContentPage
    {
        class CypherViewModel : INotifyPropertyChanged
        {
            #region Properties            

            private string _encryptionKey;
            public string EncryptionKey
            {
                get { return _encryptionKey; }
                set
                {
                    _encryptionKey = value;
                    OnPropertyChanged();
                }
            }

            private string _entry;
            public string Entry
            {
                get { return _entry; }
                set
                {
                    _entry = value;
                    OnPropertyChanged();
                }
            }

            private string _result;
            public string Result
            {
                get { return _result; }
                set
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }

            #endregion

            #region Constructor
            public CypherViewModel()
            {

            }
            #endregion

            #region Commands

            public Command ResetDataCommand
            {
                get
                {
                    return new Command(ResetData);
                }
            }
            public Command EncryptCommand
            {
                get
                {
                    return new Command(EncryptEntry);
                }
            }
            public Command DecryptCommand
            {
                get
                {
                    return new Command(DecryptEntry);
                }
            }
            public Command CopyEntryCommand
            {
                get
                {
                    return new Command(CopyEntry);
                }
            }
            public Command PasteEntryCommand
            {
                get
                {
                    return new Command(PasteEntry);
                }
            }
            public Command CopyResultCommand
            {
                get
                {
                    return new Command(CopyResult);
                }
            }
            public Command ShareResultCommand
            {
                get
                {
                    return new Command(ShareResult);
                }
            }

            #endregion

            #region Methods

            void ResetData()
            {
                EncryptionKey = string.Empty;
                Entry = string.Empty;
                Result = string.Empty;
            }
            async void EncryptEntry()
            {
                if (string.IsNullOrEmpty(Entry))
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "You gotta introduce something to encrypt", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(EncryptionKey))
                {
                    var result = string.Empty;

                    try
                    {
                        result = dotGuava.Security.Cypher.Encrypt(Entry);
                    }
                    catch (Exception)
                    {
                        result = "Error: Input couldn't be encrypted";
                    }
                    finally
                    {
                        Result = result;
                    }                    
                }
                else
                {                    
                    var result = string.Empty;

                    try
                    {
                        result = dotGuava.Security.Cypher.Encrypt(Entry, EncryptionKey);
                    }
                    catch (Exception)
                    {
                        result = "Error: Input couldn't be encrypted";
                    }
                    finally
                    {
                        Result = result;
                    }
                }
            }
            async void DecryptEntry()
            {
                if (string.IsNullOrEmpty(Entry))
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "You gotta introduce something to decrypt", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(EncryptionKey))
                {
                    var result = string.Empty;

                    try
                    {
                        result = dotGuava.Security.Cypher.Decrypt(Entry);
                    }
                    catch (Exception)
                    {
                        result = "Error: Input couldn't be decrypted";
                    }
                    finally
                    {
                        Result = result;
                    }
                }
                else
                {                    
                    var result = string.Empty;

                    try
                    {
                        result = dotGuava.Security.Cypher.Decrypt(Entry, EncryptionKey);
                    }
                    catch (Exception)
                    {
                        result = "Error: Input couldn't be decrypted";
                    }
                    finally
                    {
                        Result = result;
                    }
                }
            }
            async void CopyEntry()
            {
                await Clipboard.SetTextAsync(Entry);
            }
            async void PasteEntry()
            {
                if (Clipboard.HasText)
                {
                    Entry = await Clipboard.GetTextAsync();
                }
            }
            async void CopyResult()
            {
                await Clipboard.SetTextAsync(Result);
            }
            async void ShareResult()
            {
                await Share.RequestAsync(new Xamarin.Essentials.ShareTextRequest
                {
                    Text = Result,
                    Title = "Share result"
                });
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

        public CypherPage()
        {
            InitializeComponent();

            BindingContext = new CypherViewModel();
        }
    }
}
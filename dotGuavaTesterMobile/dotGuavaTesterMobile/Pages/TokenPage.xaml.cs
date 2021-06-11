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
    public partial class TokenPage : ContentPage
    {
        class Token
        {
            public int TokenSetNo_Left { get; set; }
            public int TokenSetNo_Right { get; set; }
            public string TokenSetCode_Left { get; set; }
            public string TokenSetCode_Right { get; set; }
            public string TokenSetInfo_Left => $"{TokenSetNo_Left.ToString().PadLeft(2,'0')} - {TokenSetCode_Left}";
            public string TokenSetInfo_Right => $"{TokenSetNo_Right} - {TokenSetCode_Right}";
        }
        class TokenViewModel : INotifyPropertyChanged
        {
            #region Properties            

            ObservableCollection<Token> _tokenList;
            public ObservableCollection<Token> TokenList
            {
                get { return _tokenList; }
                set
                {
                    _tokenList = value;
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

            string _referenceKey;
            public string ReferenceKey
            {
                get { return _referenceKey; }
                set
                {
                    _referenceKey = value;
                    OnPropertyChanged();
                }
            }

            uint _tokenLenght;
            public string TokenLenght
            {
                get
                {
                    if (_tokenLenght == 0)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return _tokenLenght.ToString();
                    }
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _tokenLenght = 0;
                        OnPropertyChanged();
                        return;
                    }

                    uint.TryParse(value, out uint tokenLenght);

                    _tokenLenght = tokenLenght;
                    OnPropertyChanged();
                }
            }

            string _tokenExpirationInfo;
            public string TokenExpirationInfo
            {
                get { return _tokenExpirationInfo; }
                set
                {
                    _tokenExpirationInfo = value;
                    OnPropertyChanged();
                }
            }

            #endregion

            #region Constructor
            public TokenViewModel()
            {
                ResetData();
            }
            #endregion

            #region Commands

            public Command LoadTokensCommand
            {
                get
                {
                    return new Command(LoadTokens);
                }
            }
            public Command ResetDataCommand
            {
                get
                {
                    return new Command(ResetData);
                }
            }

            #endregion

            #region Methods

            void ResetData()
            {
                ReferenceKey = string.Empty;
                TokenLenght = string.Empty;
                TokenList = new ObservableCollection<Token>();
                TokenExpirationInfo = "Token expiration info not ready yet";
            }
            async void LoadTokens()
            {
                if (string.IsNullOrEmpty(ReferenceKey))
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Introduced reference key is invalid", "OK");
                    IsLoading = false;
                    return;
                }
                if (string.IsNullOrEmpty(TokenLenght))
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Introduced token lenght is invalid", "OK");
                    IsLoading = false;
                    return;
                }

                await Task.Run(() => FullfillTokenList());
            }
            void FullfillTokenList()
            {
                IsLoading = true;

                TokenList.Clear();

                try
                {
                    var tokenList = dotGuava.Essential.Token.GetStringTokenList(uint.Parse(TokenLenght), ReferenceKey);
                    for (int i = 0; i < 20; i++)
                    {
                        TokenList.Add(new Token
                        {
                            TokenSetNo_Left = (i + 1),
                            TokenSetCode_Left = tokenList.ElementAt(i),
                            TokenSetNo_Right = (i + 21),
                            TokenSetCode_Right = tokenList.ElementAt(i + 20)
                        });
                    }
                }
                catch (Exception)
                {
                    TokenList.Clear();
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
        public TokenPage()
        {
            InitializeComponent();

            BindingContext = new TokenViewModel();
        }
    }
}
using LoopbackTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LoopbackTest.Pages
{
    public partial class LoginPage : ContentPage
    {
        private bool isRegister;

        public bool IsRegister
        {
            get { return isRegister; }
            set { isRegister = value;
                OnPropertyChanged("IsRegister");
            }
        }

        private bool isLogin;

        public bool IsLogin
        {
            get { return isLogin; }
            set { isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }

        private string toggleText;

        public string ToggleText
        {
            get { return toggleText; }
            set { toggleText = value;
                OnPropertyChanged("ToggleText");
            }
        }
        private string toggleBtn;

        public string ToggleBtn
        {
            get { return toggleBtn; }
            set
            {
                toggleBtn = value;
                OnPropertyChanged("ToggleBtn");
            }
        }

        private string firstname;

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value;
                OnPropertyChanged("Firstname");
            }
        }

        private string lastname;

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value;
                OnPropertyChanged("Lastname");
            }
        }

        private DateTime birthdate;

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value;
                OnPropertyChanged("Birthdate");
            }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set {
                username = value;
                OnPropertyChanged("Username");
            }
        }


        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }

        }
        private string user;

        public string User 
        {
            get { return user; }
            set { user = value;
                OnPropertyChanged("User");
            }
            
        }

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
            NavigationPage.SetHasNavigationBar(this, false);
            ToggleText = "First time in Pfush?";
            ToggleBtn = "Register";
            IsLogin = true;
            IsRegister = false;
            OnPropertyChanged("IsLogin");
            OnPropertyChanged("IsRegister");
            DatePicker.IsVisible = false;
            RegisterLbl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => RegisterToggleClicked()),
            });

        }

     

        async public void LoginClicked(object sender, EventArgs args)
        {
            try
            {
                isLogin = true;
                if(await LoginHelper.Login(User, Password))
                {
                    await Navigation.PushAsync(new MainPage());
                }else
                {
                    await DisplayAlert("Error", "Invalid Login credentials", "OK");
                }
               
            }catch(Exception e)
            {
                await DisplayAlert("Error", e.Message.ToString(), "OK");
            }

           
        }
        async public void RegisterToggleClicked()
        {
            try
            {
                if (isLogin)
                {
                    ToggleText = "Already have an account?";
                    isLogin = false;
                    IsRegister = true;
                    OnPropertyChanged("IsLogin");
                    OnPropertyChanged("IsRegister");
                    DatePicker.IsVisible = true;
                }
                else
                {
                    ToggleText = "First time in Pfush?";
                    isLogin = true;
                    IsRegister = false;
                    OnPropertyChanged("IsLogin");
                    OnPropertyChanged("IsRegister");
                    DatePicker.IsVisible = false;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message.ToString(), "OK");
            }


        }
        async public void RegisterClicked(object sender, EventArgs args)
        {
            try
            {
                isLogin = false;
                if (await LoginHelper.Register(User, Password, Username, Birthdate, Firstname, Lastname))
                {
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid Login credentials", "OK");
                }

            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message.ToString(), "OK");
            }


        }

    }
}

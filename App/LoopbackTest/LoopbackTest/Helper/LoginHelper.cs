using LBXamarinSDK;
using LBXamarinSDK.LBRepo;
using LoopbackTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoopbackTest.Helper
{
    class LoginHelper
    {
        private const int UNAUTHORIZED = 401;
        public static async Task<bool> Login(string email, string password)
        {
            var user = new User()
            {
                email = email,
                password = password
            };
            try
            {
                
                AccessToken accessToken = await Users.login(user);
                Gateway.SetAccessToken(accessToken);
                SessionData.Register<AccessToken>(accessToken);
            }
            catch (RestException ex)
            {
                if (ex.StatusCode == UNAUTHORIZED)
                {
                    return false;
                }
                throw;
            }
           
            return true;
        }

        public static async Task<bool> Logout()
        {
            try
            {
               await Users.logout();
            }
            catch (RestException ex)
            {
                if (ex.StatusCode == UNAUTHORIZED)
                {
                    return false;
                }
                throw;
            }
            return true;
        }

        public static async Task<bool> Register(string email, string password, string username, DateTime birthdate, string firstname, string lastname)
        {
            var user = new User()
            {
                email = email,
                password = password,
                username=username,
                birthdate=birthdate,
                firstname=firstname,
                lastname=lastname,
                verificationToken=Convert.ToBase64String(Guid.NewGuid().ToByteArray())

            };
            try
            {
                var test=user.verificationToken;
                var response=await Users.Create(user);

                //DependencyService.Get<IAndroidMethods>().sendVerificationEmail(email, response.id, user.verificationToken);




            }
            catch (RestException ex)
            {
                if (ex.StatusCode == UNAUTHORIZED)
                {
                    return false;
                }
                int i = 0;
                throw;
            }
  
       
           // await Login(email, password);
            return true;
        }

        public static async Task<User> findById(string id)
        {
            User user = await Users.FindById(id);
            return user;
        }
       
    }
}

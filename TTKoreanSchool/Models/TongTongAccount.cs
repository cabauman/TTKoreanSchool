using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace TTKoreanSchool.Models
{
    public class TongTongAccount
    {
        public TongTongAccount()
        {
            XamarinAuthAccount = new Account();
        }

        public TongTongAccount(Account xamarinAuthAccount)
        {
            XamarinAuthAccount = xamarinAuthAccount;
        }

        public Account XamarinAuthAccount { get; set; }

        public string AuthToken
        {
            get { return XamarinAuthAccount.Properties["AuthToken"]; }
        }

        public string FirebaseAuthJson
        {
            get
            {
                return XamarinAuthAccount.Properties["FirebaseAuthJson"];
            }

            set
            {
                XamarinAuthAccount.Properties["FirebaseAuthJson"] = value;
            }
        }
    }
}
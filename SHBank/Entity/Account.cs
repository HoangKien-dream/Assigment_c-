using System;
using System.Collections.Generic;
using SHBank.Util;

namespace SHBank.Entity
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string NameAccount { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public  string Salt { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }

        public double Balance { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Account()
        {
           //GenerateAccountNumber();
        }

        public Dictionary<string,string> CheckValid()
        {
            var errors = new Dictionary<string,string>();
            if (string.IsNullOrEmpty(UserName))
            {
                errors.Add("username","Username cannot be null or empty !");
            }

            if (string.IsNullOrEmpty(NameAccount))
            {
                errors.Add("nameAccount", "NameAccount cannot be null or empty! ");
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password cannot be null or empty !");
            }

            if (!Password.Equals(PasswordConfirm))
            {
                errors.Add("confirmPassword","Password and confirm password is not matched !");
            }

            if (string.IsNullOrEmpty(Address))
            {
                errors.Add("address","Address cannot be null or empty !");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                errors.Add("phone","Phone cannot be null or empty !");
            }
            return errors;
        }

        public override string ToString()
        {
            return string.Format("Account:{0} - AccountNumber: {1} - UserName: {2} - Address: {3} - Phone: {4} - Balance: {5} - Status: {6}",NameAccount, AccountNumber, UserName, Address, Phone, Balance, Status);
        }

        public void EncryptPassword()
        {
            Salt = HashUtil.RandomString(7);
            //3.2 Băm.
            PasswordHash = HashUtil.GenerateSaltedSHA1(Password, Salt);
            //3.3 Set.
        }

        public void GenerateAccountNumber()
        {
            AccountNumber = Guid.NewGuid().ToString();
        }

        public Dictionary<string,string> CheckValidLogin()
        {
            var errors = new Dictionary<string,string>();
            if (string.IsNullOrEmpty(NameAccount))
            {
                errors.Add("nameAccount","NameAccount cannot be null or empty !");
            }
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password cannot be null or empty !");
            }
            return errors;
        }
    }
}
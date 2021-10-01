using System.Collections.Generic;
using SHBank.Util;

namespace SHBank.Entity
{
    public class Admin
    {
        public string NameAccount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public  string Salt { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        
        public void EncryptPassword()
        {
            Salt = HashUtil.RandomString(7);
            //3.2 Băm.
            PasswordHash = HashUtil.GenerateSaltedSHA1(Password, Salt);
            //3.3 Set.
        }
        public Dictionary<string,string> CheckValidLogin()
        {
            var errors = new Dictionary<string,string>();
            if (string.IsNullOrEmpty(NameAccount))
            {
                errors.Add("username","Name Account cannot be null or empty !");
            }
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password cannot be null or empty !");
            }
            return errors;
        }
        public override string ToString()
        {
            return $"Username: {UserName},  Password: {Password}, PasswordHash: {PasswordHash}, Salt: {Salt}";
        }
        public Dictionary<string,string> CheckValid()
        {
            var errors = new Dictionary<string,string>();
            if (string.IsNullOrEmpty(UserName))
            {
                errors.Add("username","Username cannot be null or empty !");
            }

            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("password","Password cannot be null or empty !");
            }

            if (!Password.Equals(PasswordConfirm))
            {
                errors.Add("confirmPassword","Password and confirm password is not matched !");
            }

            if (string.IsNullOrEmpty(NameAccount))
            {
                errors.Add("nameAccount","NameAccount cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                errors.Add("phone","Phone cannot be null or empty");
            }

            if (string.IsNullOrEmpty(Email))
            {
                errors.Add("email","Email cannot be null or empty");
            }

            if (string.IsNullOrEmpty(Address))
            {
                errors.Add("address","Address cannot be null or empty");
            }
            return errors;
        }

    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Accounts")]
    public class Account : DomainEntity<int>
    {
        public Account()
        {
        }
        public Account(string domain, string userName, int passwordId, string password, string hiddenPassword, string phone, 
            string securityEmail, string url, string note, int order, int level)
        {
            Domain = domain;
            UserName = userName;
            PasswordId = passwordId;
            Phone = phone;
            SecurityEmail = securityEmail;
            Url = url;
            Note = note;
            Order = order;
            Level = level;           
            HiddenPassword = hiddenPassword;
            Password = password;
        }
        public string Domain { set; get; }
        public string UserName { set; get; }
        public int PasswordId { set; get; }
        public string Password { set; get; }
        public string HiddenPassword { set; get; }
        public string Phone { set; get; }
        public string SecurityEmail { set; get; }
        public string Url { set; get; }
        public string Note { set; get; }     
        public int Order { set; get; }
        public int Level { set; get; }          
    }
}
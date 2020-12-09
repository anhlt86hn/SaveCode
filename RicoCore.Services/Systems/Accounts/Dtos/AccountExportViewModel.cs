using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RicoCore.Services.Systems.Accounts.Dtos
{
    public class AccountExportViewModel
    { 
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

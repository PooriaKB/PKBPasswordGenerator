using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public class User
    {
        
        public string UserName { get; set; }


        public string MasterPassword { get; set; } = string.Empty;


        public void SaveUserName(string userNameFilePath) => File.WriteAllText(userNameFilePath, UserName);
        public string LoadUserName(string userFilePath) => File.ReadAllText(userFilePath);
        

    }
}

using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class PasswordDataAccess
    {
        public List<Password> Passwords = new List<Password>();

        // Finds a password by its name and value
        public Password FindPasswordByNameAndValue(string name, string value) => Passwords.First(p => p.Name == name && p.Value == value);

        // Adds a new password to the collection
        public void AddPassword(Password password)
        {
            int newId = GetNextId();
            password.Id = newId;
            Passwords.Add(password);
        }

        // Removes a password by its ID
        public void RemovePassword(int id)
        {
            Password passwordToRemove = Passwords.First(p => p.Id == id);
            if (passwordToRemove != null)
            {
                Passwords.Remove(passwordToRemove);
            }
        }

        // Generates the next unique ID for a new password
        public int GetNextId()
        {
            if (Passwords.Count == 0)
                return 1;
            return Passwords[^1].Id + 1;
        }

        

       
    }
}

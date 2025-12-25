using System.Text;
using System.Security.Cryptography;

namespace DataAccess.Models
{
    public class Password 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Value { get; set; }
        public int Length { get; set; }
        public bool UseAlphabetChars { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseSpecialChars { get; set; }


        public Password(string Name, int Length, bool UseAlphabetChars, bool UseNumbers, bool UseSpecialChars)
        {   
            this.Name = Name;
            this.Length = Length;
            this.UseAlphabetChars = UseAlphabetChars;
            this.UseNumbers = UseNumbers;
            this.UseSpecialChars = UseSpecialChars;
        }

        public string GeneratePassword(int length, bool useAlphabetChars, bool useNumbers, bool useSpecialChars)
        {
            const string alphabets = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";
            


            StringBuilder characterSet = new StringBuilder();
            StringBuilder passwordBuilder = new StringBuilder();

            if (useAlphabetChars && useNumbers && useSpecialChars)
            {
                characterSet.Append(alphabets).Append(numbers).Append(specialChars);
            }
            else if (useAlphabetChars && useNumbers)
            {
                characterSet.Append(alphabets).Append(numbers);
            }
            else if (useAlphabetChars && useSpecialChars)
            {
                characterSet.Append(alphabets).Append(specialChars);
            }
            else if (useNumbers && useSpecialChars)
            {
                characterSet.Append(numbers).Append(specialChars);
            }
            else if (useAlphabetChars)
            {
                characterSet.Append(alphabets);
            }
            else if (useNumbers)
            {
                characterSet.Append(numbers);
            }
            else if (useSpecialChars)
            {
                characterSet.Append(specialChars);
            }
            else
            {
                throw new ArgumentException("At least one character type must be selected.");
            }

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characterSet.Length);
                passwordBuilder.Append(characterSet[index]);
            }

            var Value = passwordBuilder.ToString();
            return Value;
        }

        

        

    }
}

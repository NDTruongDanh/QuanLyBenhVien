using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace QuanLyBenhVien.Classes
{
    public class CommonChecks
    {
        public static bool HasDigit(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return false;
            return Regex.IsMatch(input, @"\d");
        }

        public static bool IsNumber(string input)
        {
            if(string.IsNullOrEmpty(input)) 
                return false;
            return double.TryParse(input, out _);
        }

        public static bool IsEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            //Cai nay kho hieu lam nen ke di :)
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, emailPattern);
        }
    }
}

using System;
using System.Linq;

namespace Models
{
    public class Inputter : IInputter
    {
        public string GetAnyInput()
        {
            return Console.ReadLine();
        }
        public string GetStringInput()
        {
            bool flag = true;
            string input = null;
            while(flag)
            {
                input = Console.ReadLine();
                if(input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    flag = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Numbers and special characters are not permitted.");
                }
            }
            return input;
        }

        public int GetIntegerInput()
        {
            int option = -1;
            while(option < 0)
            {
                try
                {
                    option = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            return option;
        }

        public decimal GetDecimalInput()
        {
            decimal ret = -1.0M;
            while(ret < 0)
            {
                try
                {
                    ret = decimal.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a decimal number.");
                }
            }
            return ret;
        }
    }
}
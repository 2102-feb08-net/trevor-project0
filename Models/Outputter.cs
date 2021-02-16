using System;

namespace Models
{
    public class Outputter : IOutputter
    {
        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}
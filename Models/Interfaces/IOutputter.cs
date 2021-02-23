using System;

namespace Models
{
    public interface IOutputter
    {
        void Write(string s);
        void WriteLine(string s);
    }
}
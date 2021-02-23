using System;

namespace Models
{
    public interface IInputter
    {
        string GetStringInput();

        int GetIntegerInput();

        decimal GetDecimalInput();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountDemo.Exceptions;

public class ModelException : Exception
{
    public string PropertyName { get; set; } = string.Empty;
    public ModelException(string message, string property) : base(message)
    {
        PropertyName = property;
    }
}
using System;

namespace FlumeAgent
{
    public class CollectorNotDefinedException : Exception
    {
        public CollectorNotDefinedException(string msg) : base(msg) { }
    } public class SourceNotDefinedException : Exception
    {
        public SourceNotDefinedException(string msg) : base(msg) { }
    }
}
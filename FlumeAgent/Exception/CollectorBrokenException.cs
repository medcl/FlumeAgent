using System;

namespace FlumeAgent
{
    public class BrokenCollectorException:Exception
    {
         public BrokenCollectorException(string msg):base(msg){}
    }
}
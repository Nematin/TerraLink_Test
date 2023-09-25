using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxApp.Models.Transmitter
{
    public abstract class BaseTransmitter
    {
        public abstract Task Send<T>(T message);
    }
}

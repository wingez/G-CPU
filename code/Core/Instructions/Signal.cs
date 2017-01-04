using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instructions
{
    public class Signal
    {
        public string Name { get; set; } = null;
        public string Desc { get; set; } = null;

        public List<WireState> WireStates { get; set; } = new List<WireState>();

    }
}

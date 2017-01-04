using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instructions
{
    public class InstructionDB
    {
        public List<Wire> Wires { get; internal set; } = new List<Wire>();
        public List<Signal> Signals { get; internal set; } = new List<Signal>();

        public List<Macro> Macros { get; internal set; } = new List<Macro>();

        public List<Instruction> Instructions { get; internal set; } = new List<Instruction>();


    }
}

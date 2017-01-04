using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instructions
{
    public class Instruction
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Index { get; set; }

        public List<string> Aliases { get; set; } = new List<string>();

        public List<InstructionStage> Stages { get; set; } = new List<InstructionStage>();

    }
    public class InstructionStage
    {
        public List<Signal> Signals { get; set; } = new List<Signal>();
    }
    public class Macro
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<InstructionStage> Stages { get; set; } = new List<InstructionStage>();

    }
}

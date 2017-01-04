using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ControlROM
{
    

    public class ControlRomInstruction
    {
        public byte[,] Data { get; private set; }
        public string Name { get; private set; }
        public int Stages { get; private set; }
        public int Index { get; private set; }

        public ControlRomInstruction(string name, int stages, int chips, int index)
        {
            Name = name;
            Stages = stages;
            Index = index;
            Data = new byte[stages, chips];
        }

        public byte this[int stage, int chip]
        {
            get { return Data[stage, chip]; }
            set { Data[stage, chip] = value; }
        }


    }


}

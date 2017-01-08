using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Instructions;
using System.IO;

namespace Core.ControlROM
{



    public class ControlRomCreator
    {
        byte[] defaults;

        List<ControlRomInstruction> instructions;

        int chips;
        bool loaded = false;


        public void Create()
        {
            InstructionParser parser = new InstructionParser();
            InstructionDB db = parser.GetDB();

            chips = 0;
            foreach (var wire in db.Wires)
            {
                if (wire.Chip + 1 > chips)
                    chips = wire.Chip + 1;
            }

            defaults = new byte[chips];

            foreach (var wire in db.Wires)
            {
                if (wire.ActiveState == false)
                {
                    defaults[wire.Chip] |= (byte)(1 << wire.Index);
                }
            }

            instructions = new List<ControlRomInstruction>();

            foreach (var instrinfo in db.Instructions)
            {
                ControlRomInstruction instr = new ControlRomInstruction(instrinfo.Name, 6, chips, instrinfo.Index);

                for (int stage = 0; stage < 6; stage++)
                {
                    for (int chip = 0; chip < chips; chip++)
                        instr[stage, chip] = defaults[chip];

                    if (stage < instrinfo.Stages.Count)
                    {
                        InstructionStage instrstage = instrinfo.Stages[stage];

                        foreach (var signal in instrstage.Signals)
                        {
                            foreach (var wirestate in signal.WireStates)
                            {
                                if (wirestate.State == true)
                                {
                                    instr[stage, wirestate.Wire.Chip] ^= (byte)(1 << wirestate.Wire.Index);
                                }
                            }
                        }
                    }

                }
                instructions.Add(instr);
            }

            this.loaded = true;
        }



        public void Write(StreamWriter stream)
        {
            if (!loaded)
                throw new Exception("Instruction data not created");


            //stream.AutoFlush = false;
            stream.WriteLine();
            stream.WriteLine($"//Generated {DateTime.Now.ToString()}");

            stream.WriteLine();
            stream.WriteLine($"#numchips={chips}");

            stream.WriteLine();

            stream.WriteLine();
            stream.WriteLine();


            foreach (var instruction in instructions)
            {
                stream.WriteLine($"//Instruction: {instruction.Name}");
                stream.Write("//INSTR\tSTAGE");
                for (int i = chips - 1; i >= 0; i--)
                    stream.Write($"\tCHIP{i}");

                stream.WriteLine();

                for (int stage = 0; stage < instruction.Stages; stage++)
                {
                    stream.Write($"  {instruction.Index.ToString("X4")}\t{stage.ToString("X4")}");
                    for (int chip = chips - 1; chip >= 0; chip--)
                    {
                        stream.Write($"\t{instruction[stage, chip].ToString("X4")}");
                    }
                    stream.WriteLine();
                }

                stream.WriteLine();
                stream.WriteLine();
            }

            stream.Flush();

        }


    }
}

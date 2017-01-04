using Core.Instructions;
using Core.ControlROM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            InstructionParser parser = new InstructionParser();

            InstructionDB db = parser.GetDB();

            ControlRomCreator rom = new ControlRomCreator();
            rom.Create();

            var sw = new StreamWriter(Console.OpenStandardOutput());

            Console.SetOut(sw);
            sw.AutoFlush = true;
            Console.WriteLine("hello");
            Console.WriteLine();
            rom.Write(sw);

            Console.ReadLine();
        }
    }
}

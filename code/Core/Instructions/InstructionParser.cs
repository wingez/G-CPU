using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Core;

namespace Core.Instructions
{
    public class InstructionParser
    {
        static InstructionDB db = null;


        public InstructionDB GetDB(bool forceUpdate = false)
        {
            if (db == null || forceUpdate)
                ParseDB();

            return db;
        }

        private void ParseDB()
        {
            Config config = new Config();

            XmlReader reader = config.GetXmlReader("instructions");

            db = new InstructionDB();


            ParseWires(reader);
            ParseSignals(reader);
            ParseMacros(reader);
            ParseInstructions(reader);
        }

        void ParseWires(XmlReader reader)
        {

            ReadFollowing(reader, "Wires");

            ReadDescendant(reader, "Wire");
            do
            {

                Wire wire = new Wire();

                ReadDescendant(reader, "Name");
                wire.Name = reader.ReadElementContentAsString();

                ReadSibling(reader, "Chip");
                wire.Chip = reader.ReadElementContentAsInt();

                ReadSibling(reader, "Index");
                int index = reader.ReadElementContentAsInt();
                if (index >= 8 || index < 0)
                    throw new XmlException($"Wire index out of bounds. Line: {GetLineNumber(reader)}");
                wire.Index = index;



                ReadSibling(reader, "ActiveState");
                wire.ActiveState = reader.ReadElementContentAsBoolean();


                foreach (var w in db.Wires)
                {
                    if (w.Chip == wire.Chip && w.Index == wire.Index)
                        throw new XmlException($"A wire with name {wire.Name} already decleared. Line: {GetLineNumber(reader)}");
                    if (w.Name == wire.Name)
                        throw new XmlException($"A wire with index {wire.Chip}:{wire.Index} already decleared. Line: {GetLineNumber(reader)}");
                }



                db.Wires.Add(wire);

                reader.ReadEndElement();

            } while (reader.ReadToNextSibling("Wire"));

        }

        void ParseSignals(XmlReader reader)
        {
            ReadFollowing(reader, "Signals");
            ReadDescendant(reader, "Signal");
            do
            {
                Signal signal = new Signal();

                ReadDescendant(reader, "Name");
                signal.Name = reader.ReadElementContentAsString();

                ReadSibling(reader, "Wires");
                if (reader.ReadToDescendant("Wire"))
                {
                    do
                    {
                        string wirename = GetAttribute(reader, "name");
                        bool wirestate = bool.Parse(GetAttribute(reader, "state"));

                        Wire wire = null;

                        foreach (var w in db.Wires)
                        {
                            if (w.Name == wirename)
                            {
                                wire = w;
                                break;
                            }
                        }

                        if (wire == null)
                            throw new XmlException($"Unkown wire {wirename}. Line:{GetLineNumber(reader)}");

                        signal.WireStates.Add(new WireState() { Wire = wire, State = wirestate });

                    } while (reader.ReadToNextSibling("Wire"));
                }

                db.Signals.Add(signal);

                reader.ReadEndElement();
                reader.ReadEndElement();
            } while (reader.ReadToNextSibling("Signal"));


        }

        void ParseMacros(XmlReader reader)
        {
            ReadFollowing(reader, "Macros");
            ReadDescendant(reader, "Macro");
            do
            {
                Macro macro = new Macro();

                ReadDescendant(reader, "Name");
                macro.Name = reader.ReadElementContentAsString();

                ReadSibling(reader, "Desc");
                macro.Description = reader.ReadElementContentAsString();

                ReadSibling(reader, "Stages");
                ReadDescendant(reader, "Stage");
                do
                {
                    macro.Stages.Add(parseStage(reader));

                } while (reader.ReadToNextSibling("Stage"));

                db.Macros.Add(macro);

                reader.ReadEndElement();
                reader.ReadEndElement();
            } while (reader.ReadToNextSibling("Macro"));
        }

        void ParseInstructions(XmlReader reader)
        {
            ReadFollowing(reader, "Instructions");
            ReadDescendant(reader, "Instruction");
            do
            {
                Instruction instruction = new Instruction();

                ReadDescendant(reader, "Name");
                instruction.Name = reader.ReadElementContentAsString();

                ReadSibling(reader, "Desc");
                instruction.Description = reader.ReadElementContentAsString();

                ReadSibling(reader, "Index");
                instruction.Index = reader.ReadElementContentAsInt();

                ReadSibling(reader, "Stages");

                while (true)
                {
                    reader.Read();
                    reader.Read();
                    if (reader.Name == "Macro")
                    {
                        string macroname = GetAttribute(reader, "name");
                        Macro macro = null;
                        foreach (var m in db.Macros)
                        {
                            if (m.Name == macroname)
                            {
                                macro = m;
                                break;
                            }
                        }
                        if (macro == null)
                            throw new XmlException($"Macro {macroname} not found. Line: {GetLineNumber(reader)}");

                        instruction.Stages.AddRange(macro.Stages);
                    }
                    else if (reader.Name == "Stage")
                    {
                        instruction.Stages.Add(parseStage(reader));
                        //reader.Read();
                    }
                    else
                    {
                        break;
                    }
                }
                db.Instructions.Add(instruction);

                reader.Read();
                reader.Read();
            } while (reader.ReadToNextSibling("Instruction"));

        }


        InstructionStage parseStage(XmlReader reader)
        {
            InstructionStage stage = new InstructionStage();

            if (!reader.ReadToDescendant("Signal"))
                throw new XmlException($"Expected node Signal. Line:{((IXmlLineInfo)reader).LineNumber}");

            do
            {
                string signalname = reader.GetAttribute("name");
                if (signalname == null)
                    throw new XmlException($"Expected attribute name. Line:{((IXmlLineInfo)reader).LineNumber}");

                Signal signal = null;
                foreach (var s in db.Signals)
                {
                    if (s.Name == signalname)
                    {
                        signal = s;
                        break;
                    }
                }
                if (signal == null)
                    throw new XmlException($"Unknown signal {signalname}. Line:{((IXmlLineInfo)reader).LineNumber}");

                if (stage.Signals.Count > 0)
                {
                    bool wireInUse = false;
                    foreach (var csignal in stage.Signals)
                        foreach (var cwirestate in csignal.WireStates)
                            foreach (var wirestate in signal.WireStates)
                                if (cwirestate.Wire.Chip == wirestate.Wire.Chip && cwirestate.Wire.Index == wirestate.Wire.Index)
                                    wireInUse = true;

                    if (wireInUse)
                        throw new XmlException($"Signal {signal.Name} causes double use of wire. Line: {GetLineNumber(reader)}");
                }
                stage.Signals.Add(signal);

            } while (reader.ReadToNextSibling("Signal"));

            return stage;
        }




        int GetLineNumber(XmlReader reader)
        {
            return ((IXmlLineInfo)reader).LineNumber;
        }

        string GetAttribute(XmlReader reader, string attribute)
        {
            string val = reader.GetAttribute(attribute);
            if (val == null)
            {
                throw new XmlException($"Expected attribute {attribute}. Line:{GetLineNumber(reader)}");
            }
            return val;
        }

        void ReadFollowing(XmlReader reader, string node)
        {
            if (!reader.ReadToFollowing(node))
                throw new XmlException($"Expected node {node}. Line:{GetLineNumber(reader)}");
        }

        void ReadDescendant(XmlReader reader, string node)
        {
            if (!reader.ReadToDescendant(node))
                throw new XmlException($"Expected node {node}. Line:{GetLineNumber(reader)}");

        }
        void ReadSibling(XmlReader reader, string node)
        {
            if (!reader.ReadToNextSibling(node))
                throw new XmlException($"Expected node {node}. Line:{GetLineNumber(reader)}");
        }





        public InstructionCollection GetInstructions()
        {
            throw new NotImplementedException();



        }




    }
}

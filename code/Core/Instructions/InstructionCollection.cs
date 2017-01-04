using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Instructions
{
    public class InstructionCollection : IEnumerable<Instruction>
    {
        public Instruction this[int i]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }





        class InstructionEnumerator : IEnumerator<Instruction>
        {
            public Instruction Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}

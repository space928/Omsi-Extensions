﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiHook
{
    /// <summary>
    /// ChronoChangeDel - *no properites defined*
    /// </summary>
    public class OmsiChronoChangeDel : OmsiChronoChange
    {
        public OmsiChronoChangeDel() : base() { }

        internal OmsiChronoChangeDel(Memory memory, int address) : base(memory, address) { }

    }
}

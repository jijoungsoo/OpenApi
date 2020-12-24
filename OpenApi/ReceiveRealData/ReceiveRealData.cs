using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;

namespace OpenApi.ReceiveRealData
{
    public abstract class ReceiveRealData
    {
        public SpellOpt spell;

        public abstract void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e);
    }


}

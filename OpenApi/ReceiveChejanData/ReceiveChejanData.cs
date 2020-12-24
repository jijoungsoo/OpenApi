using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;

namespace OpenApi.ReceiveChejanData
{
    public abstract class ReceiveChejanData
    {
        public SpellOpt spell;
        public abstract void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e);

    }

}

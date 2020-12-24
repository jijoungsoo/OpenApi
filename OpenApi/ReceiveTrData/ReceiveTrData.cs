using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;

namespace OpenApi.ReceiveTrData
{
    public abstract class ReceiveTrData
    {
        public SpellOpt spell;

        public abstract void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e);
        public abstract int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell);
        public void Send(String key, String message,String className) {
            FileLog.PrintF(String.Format("Send [className][Key][Message]=>[{0}][{1}][{2}]",className,key,message));
        }


        protected Boolean isNotNull(String value)
        {
            if (value != null && !value.Trim().Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void putReceivedQueueAndsetNextSpell(String key,int prevNext, String lastStockDate)
        {
            OpenApi.Spell.SpellOpt tmp = spell.ShallowCopy();
            tmp.nPrevNext = prevNext;
            tmp.lastStockDate = lastStockDate;
            AppLib.getClass1Instance().removeSpellDictionary(key);
            AppLib.getClass1Instance().AddSpellDictionary(key, tmp);
        }

    }


}

//#define  REAL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KiwoomCode;
using System.Collections.Concurrent;
using System.IO;

namespace OpenApi
{
    class Commission
    {
        /*
[모의투자]는  매도,매입수수료가 0.35%   (10원미만절사)
[실제투자]는  매도,매입수수료가 0.015%   (10원미만절사)
제세금(거래소종목) = 현재가 * 수량* 0.15(원미만 절사) *2
제세금(코스닥종목) = 현재가 * 수량* 0.3 (원미만절사)

살때는 제세금이 안붙고 팔때만 세금이 붙는다.
            */
#if (REAL)
        private static float _kiwoomCommissionSell = 0.15f/100;
        private static float _kiwoomCommissionBuy = 0.15f/100;
#else
        private static float _kiwoomCommissionSell = 0.35f/100;   //%이니  나누기 100을 한다.
        private static float _kiwoomCommissionBuy = 0.35f / 100;
#endif

        private static float _taxSell = 0.3f / 100;  //%이니  나누기 100을 한다.
        private static float _taxBuy = 0;// 살때는 세금이 없다.

        public static int GetKiwoomCommissionSell(int value)
        {
            return Convert.ToInt32(Convert.ToInt32(value * Commission._kiwoomCommissionSell) / 10) * 10;
        }

        public static int GetKiwoomCommissionBuy(int value)
        {
            return Convert.ToInt32(Convert.ToInt32(value * Commission._kiwoomCommissionBuy) / 10) * 10;
        }

        public  static int GetTaxSell(int value)
        {
            FileLog.PrintF("GetTaxSell value=>" + value);
            FileLog.PrintF("GetTaxSell Convert.ToInt32(value * Commission._taxSell)=>" + Convert.ToInt32(value * Commission._taxSell));
            return Convert.ToInt32(Convert.ToInt32(value * Commission._taxSell) ) ;  //원미만 절삭이라  
        }
    }
}

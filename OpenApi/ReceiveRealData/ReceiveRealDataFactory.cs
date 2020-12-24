using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OpenApi.ReceiveRealData
{
    public class ReceiveRealDataFactory
    {
        private ReceiveRealDataFactory()
        {

        }
        
        private static Boolean _multiThread = true;
        private static Object object1 = new Object();
        private static ReceiveRealDataFactory _class1 = null;
        public static ReceiveRealDataFactory getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new ReceiveRealDataFactory();
                }
            } else {
                if (_class1 == null) {
                    lock (object1) {
                        _class1 = new ReceiveRealDataFactory();
                    }
                }
            }
            return _class1;
        }

        public void runReceiveRealData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e) {
            ReceiveRealData rt = getReceiveRealData(e.sRealType);
            if(rt!=null) { 
                rt.ReceivedData(axKHOpenAPI, e);
            }
        }

        public ReceiveRealData getReceiveRealData(String sRealType) {
            /*
안녕하십니까. 키움증권 운영자입니다.
먼저 키움증권에 관심을 가져주시고 이용해주시는 고객님께 감사 인사드립니다.
1. 주식시세
"주식시세"는 특정 종목이 (주식)기세일때 발생하는 데이터로 체결없이 현재가가 변경되는 대량매매나 종목종가 데이터 보정시
발생하며수신되는 경우를 보기 힘든 데이터입니다.
(여기서 (주식)기세란 거래없이(거래량 0) 종목가격이 변경되는 경우로 자세한 내용은 관련도서나 인터넷 검색을 참고하세요)
하지만 주식거래 데이터를 빠지지 않고 받으려면 발생빈도가 아주 적은 "주식시세"도 실시간 수신 처리를 해야할 것입니다.
2. 주식체결
실시간 타입 "주식체결"은 시장거래로 종목체결이 되면 관련 데이터를 전달해 주는것입니다.

감사합니다.

            실제로 내가 생각하는 실시간 데이터는 주식 체결 데이터 였다.


            주식예상체결
            9시 이전에 발생한 내용이다.
            이게 무엇일까??
*/
            return null;
            /*
            if ("주식시세".Equals(sRealType)) {
                return new REAL10001(); //[ REAL10001 : 주식시세 ] 
            } else if("주식체결".Equals(sRealType)) {
                return new REAL10002(); //[ REAL10002 : 주식체결 ] 
            } else if ("주식우선호가".Equals(sRealType)) {
                return new REAL10003(); //[ REAL10003 : 주식우선호가 ] 
            } else if ("주식호가잔량".Equals(sRealType)) {
                return new REAL10004(); //[ REAL10004 : 주식호가잔량 ] 
            } else if ("잔고".Equals(sRealType))
            {
                return new REAL_BALANCE(); //[ REAL_BALANCE : 잔고 ] 
            }
            else {
                return null;
            }*/
            
        }
    }
}

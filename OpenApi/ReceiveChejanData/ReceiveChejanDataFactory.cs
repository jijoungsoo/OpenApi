using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OpenApi.ReceiveChejanData
{
    public class ReceiveChejanDataFactory
    {
        private ReceiveChejanDataFactory()
        {

        }
        
        private static Boolean _multiThread = true;
        private static Object object1 = new Object();
        private static ReceiveChejanDataFactory _class1 = null;
        public static ReceiveChejanDataFactory getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new ReceiveChejanDataFactory();
                }
            } else {
                if (_class1 == null) {
                    lock (object1) {
                        _class1 = new ReceiveChejanDataFactory();
                    }
                }
            }
            return _class1;
        }
   
        public void runReceiveChejanData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e) { 
            ReceiveChejanData rt = getReceiveChejanData(e.sGubun.Trim(), axKHOpenAPI.GetChejanData(913).ToString().Trim());
            if(rt!=null) { 
                rt.ReceivedData(axKHOpenAPI, e);
            }
        }

        public ReceiveChejanData getReceiveChejanData(String Gubun,String 주문상태) {
            FileLog.PrintF(String.Format("Gubun=> {0} ", Gubun));
            FileLog.PrintF(String.Format("주문상태=> {0} ", 주문상태));
            if ("0".Equals(Gubun))
            { //0은 주문접수 주문체결
                if ("접수".Equals(주문상태)|| "체결".Equals(주문상태)) {
                    return new Order();
                } else {
                    return null;
                }
            }
            else if ("1".Equals(Gubun)) {  //1은 반영된잔고를 보여준다.
                return new Balance(); //[ REAL10002 : 주식체결 ] 
            } else if ("3".Equals(Gubun)) {   //구분 : 특이신호 
                return null;
            } else { //1은 반영된잔고를 보여준다.
                return null;
            }
                
        }
    }
}

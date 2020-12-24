using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OpenApi.ReceiveTrData
{
    public class ReceiveTrDataFactory
    {
        private ReceiveTrDataFactory()
        {

        }
        
        private static Boolean _multiThread = true;
        private static Object object1 = new Object();
        private static ReceiveTrDataFactory _class1 = null;
        public static ReceiveTrDataFactory getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new ReceiveTrDataFactory();
                }
            } else {
                if (_class1 == null) {
                    lock (object1) {
                        _class1 = new ReceiveTrDataFactory();
                    }
                }
            }
            return _class1;
        }

        public void runReceiveTrData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e) {
            if (IgnoreReceiveTrData(e.sTrCode) == false) { 
                ReceiveTrData rt = getReceiveTrData(e.sTrCode);
                if(rt!=null) { 
                    rt.ReceivedData(axKHOpenAPI, e);
                }
            }


        }

        private Boolean IgnoreReceiveTrData(String sTrCode)
        {
            if ("KOA_NORMAL_BUY_KQ_ORD".Equals(sTrCode))  //KOA_NORMAL_BUY_KQ_ORD  ==>피에스엠씨 구매할때 나왓던것 내용은 모의투자매매금지
            {
                return true;     //SendOrder 할때 나오는 키값 무시하자   
            } else if ("KOA_NORMAL_BUY_KP_ORD".Equals(sTrCode))
            {
                return true;     //SendOrder 할때 나오는 키값 무시하자
            } else if ("KOA_NORMAL_SELL_KQ_ORD".Equals(sTrCode))//KOA_NORMAL_SELL_KP_ORD
            {
                return true;     //SendOrder 할때 매도시 발생
            } else if ("KOA_NORMAL_SELL_KP_ORD".Equals(sTrCode)) {
                return true;     //SendOrder 할때 매도시 발생
            }
            else {
                return false;
            }
        }

        public ReceiveTrData getReceiveTrData(String sTrCode) {
            FileLog.PrintF("getReceiveTrData  sTrCode"+ sTrCode);
            if ("OPW00004".Equals(sTrCode)) {
                return new OPW00004(); //[ OPW00004 : 계좌평가현황요청] 
            } else if ("OPW00003".Equals(sTrCode)) {
                return new OPW00003(); //추정자산조회요청
            } else if ("OPT10059".Equals(sTrCode) ) {
                return new OPT10059(); //[ OPT10060 : 종목별투자자기관별차트요청]
            } else if ("OPT10014".Equals(sTrCode)) {
                return new OPT10014(); //[ OPT10014 : 공매도추이요청]
            } else if ("OPT10015".Equals(sTrCode)) {
                return new OPT10015(); //[ OPT10015 : 일별거래상세요청]
            } else if ("OPT10001".Equals(sTrCode)) {
                return new OPT10001(); //[ OPT10001 : 주식기본정보요청]
            } else if ("OPT10080".Equals(sTrCode)) {
                return new OPT10080(); //[ OPT10080 : 주식분봉차트조회요청]
            } else if ("OPT10085".Equals(sTrCode)) {
                return new OPT10085(); //[ OPT10085 : 계좌수익률요청]'
            } else if ("OPT10075".Equals(sTrCode)) {
                return new OPT10075(); //[ OPT10075 : 실시간미체결요청]'
            } else if ("OPTKWFID".Equals(sTrCode)) {
                return new OPTKWFID(); //[ OPTKWFID : 관심종목정보요청]'                
            } else {
                return null;
            }


            /*KOA_NORMAL_KP_CANCEL*/
        }
    }
}

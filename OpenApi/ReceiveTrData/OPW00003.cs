using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{

    public class OPW00003 : ReceiveTrData
    {/* [ OPW00003 : 추정자산조회요청 ]*/
        public OPW00003(){
            FileLog.PrintF("OPW00003");
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
           FileLog.PrintF("ReceivedData OPW00003");
           try {
                StringBuilder sbAll = new StringBuilder();
                StringBuilder sb = new StringBuilder();


                String 조회일자 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                /*추정예탁자산*/
                sb.Append("{0}|{1}|{2}");
                String tmp = sb.ToString();
                String 계좌번호 = "XXXXXXXXXX";
                //int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                계좌번호 = AppLib.getClass1Instance().getStockCode(keyStockCode);
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|accountNum:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 계좌번호
                );
    //            int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);  이거 무조건 0이다.
                String 추정예탁자산 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "추정예탁자산").Trim();//[0]
                String tmp1 = String.Format(tmp
                , 조회일자
                , 계좌번호
                , 추정예탁자산
                );
                sbAll.AppendLine(tmp1);
                String ret = sbAll.ToString();
                FileLog.PrintF("ReceivedData OPW00003 ret=>" + ret);
                ScreenNumber.getClass1Instance().DisconnectRealData(e.sScrNo);
                //this.SetRealRemove(axKHOpenAPI, "ALL", "ALL");
                AppLib.getClass1Instance().removeStockCodeDictionary(keyStockCode);
                AppLib.getClass1Instance().removeSpellDictionary(key);
                Send(key, ret, "OPW00003");
            } catch (Exception ex)
           {
               FileLog.PrintF("[ALERT-ReceivedData-OPW00003]Exception ex=" + ex.Message);
            
           }
        }

        public new void Send(String key, String message, String className)
        {
            FileLog.PrintF(String.Format("Send [className][Key][Message]=>[{0}][{1}][{2}]", className, key, message));
            OpenApi.rubyreceive.receive_portClient tmp = new OpenApi.rubyreceive.receive_portClient();
            tmp.receive_opw00003(message);
        }

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
            
 [ OPW00003 : 추정자산조회요청 ]
 1. Open API 조회 함수 입력값을 설정합니다.
	계좌번호 = 전문 조회할 보유계좌번호
	SetInputValue("계좌번호"	,  "입력값 1");
	비밀번호 = 사용안함(공백)
	SetInputValue("비밀번호"	,  "입력값 2");
	상장폐지조회구분 = 0:전체, 1:상장폐지종목제외
	SetInputValue("상장폐지조회구분"	,  "입력값 3");    
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "OPW00003"	,  "0"	,  "화면번호"); 
            */

            FileLog.PrintF("OPT10001:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10001:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10001:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10001:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10001:Run 계좌번호=>" + spell.accountNum);
            FileLog.PrintF("OPT10001:Run 비밀번호=>" + spell.password);

            axKHOpenAPI.SetInputValue("계좌번호", spell.accountNum);
            axKHOpenAPI.SetInputValue("비밀번호", spell.password);
            axKHOpenAPI.SetInputValue("상장폐지조회구분", "0");// 0:전체, 1:상장폐지종목제외
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}

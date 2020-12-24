using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;
using MySql.Data.MySqlClient;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi.ReceiveTrData
{

    public class OPT10075 : ReceiveTrData
    {
        public OPT10075(){
            FileLog.PrintF("OPT10075");
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10075");
            //try { 
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                String 계좌번호 = "XXXXXXXXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
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

            List < OPT10075_Data >  OPT10075_DataList = new List<OPT10075_Data>();


                if (nCnt > 0) {
                    for (int i = 0; i < nCnt; i++) {

                    FileLog.PrintF("OPT10075 ReceivedData 계좌번호 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "계좌번호").ToString().Trim());//[0]
                    FileLog.PrintF("OPT10075 ReceivedData 주문번호 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문번호").ToString().Trim());//[1]
                    FileLog.PrintF("OPT10075 ReceivedData 관리사번 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "관리사번").ToString().Trim());//[2]
                    FileLog.PrintF("OPT10075 ReceivedData 종목코드 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").ToString().Trim());//[3]
                    FileLog.PrintF("OPT10075 ReceivedData 업무구분 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "업무구분").ToString().Trim());//[4]
                    FileLog.PrintF("OPT10075 ReceivedData 주문상태 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문상태").ToString().Trim());//[5]
                    FileLog.PrintF("OPT10075 ReceivedData 종목명 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").ToString().Trim());//[6]
                    FileLog.PrintF("OPT10075 ReceivedData 주문수량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문수량").ToString().Trim());//[7]
                    FileLog.PrintF("OPT10075 ReceivedData 주문가격 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문가격").ToString().Trim());//[8]
                    FileLog.PrintF("OPT10075 ReceivedData 미체결수량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미체결수량").ToString().Trim());//[9]
                    FileLog.PrintF("OPT10075 ReceivedData 체결누적금액 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결누적금액").ToString().Trim());//[10]
                    FileLog.PrintF("OPT10075 ReceivedData 원주문번호 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "원주문번호").ToString().Trim());//[11]
                    FileLog.PrintF("OPT10075 ReceivedData 주문구분 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문구분").ToString().Trim());//[12]
                    FileLog.PrintF("OPT10075 ReceivedData 매매구분 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매매구분").ToString().Trim());//[13]
                    FileLog.PrintF("OPT10075 ReceivedData 시간 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시간").ToString().Trim());//[14]
                    FileLog.PrintF("OPT10075 ReceivedData 체결번호 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결번호").ToString().Trim());//[15]
                    FileLog.PrintF("OPT10075 ReceivedData 체결가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결가").ToString().Trim());//[16]
                    FileLog.PrintF("OPT10075 ReceivedData 체결량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결량").ToString().Trim());//[17]
                    FileLog.PrintF("OPT10075 ReceivedData 현재가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").ToString().Trim());//[18]
                    FileLog.PrintF("OPT10075 ReceivedData 매도호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도호가").ToString().Trim());//[19]
                    FileLog.PrintF("OPT10075 ReceivedData 매수호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수호가").ToString().Trim());//[20]
                    FileLog.PrintF("OPT10075 ReceivedData 단위체결가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "단위체결가").ToString().Trim());//[21]
                    FileLog.PrintF("OPT10075 ReceivedData 단위체결량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "단위체결량").ToString().Trim());//[22]
                    FileLog.PrintF("OPT10075 ReceivedData 당일매매수수료 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매수수료").ToString().Trim());//[23]
                    FileLog.PrintF("OPT10075 ReceivedData 단일매매세금 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "단일매매세금").ToString().Trim());//[24]
                    FileLog.PrintF("OPT10075 ReceivedData 개인투자자 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "개인투자자").ToString().Trim());//[25]

                    OPT10075_Data opt10075_Data = new OPT10075_Data();

                    String 현재일자 = DateTime.Now.ToString("yyyy-MM-dd");
                    String 체결시간TMP = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시간").ToString().Trim(); //[14]
                                                                                                                   //체결시간이 6자리이므로 HHMMSS ==> HH:MM:SS로 바꿔야한다.
                    String 체결시간 = 체결시간TMP.Substring(0, 2) + ":" + 체결시간TMP.Substring(2, 2) + ":" + 체결시간TMP.Substring(4, 2);
                    opt10075_Data.체결시간 = 현재일자 +" "+ 체결시간;



                    opt10075_Data.계좌번호 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "계좌번호").ToString().Trim();//[0]
                    opt10075_Data.주문번호 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문번호").ToString().Trim();//[1]
                    opt10075_Data.관리사번 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "관리사번").ToString().Trim(); //[2]
                    opt10075_Data.종목코드 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").ToString().Trim(); //[3]
                    opt10075_Data.업무구분 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "업무구분").ToString().Trim(); //[4]
                    opt10075_Data.주문상태 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문상태").ToString().Trim(); //[5]
                    opt10075_Data.종목명 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").ToString().Trim(); //[6]
                    opt10075_Data.주문수량 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문수량").ToString().Trim()); //[7]
                    opt10075_Data.주문가격 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문가격").ToString().Trim()); //[8]
                    opt10075_Data.미체결수량 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미체결수량").ToString().Trim()); //[9]
                    opt10075_Data.체결누계금액 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결누계금액").ToString().Trim()); //[10]
                    opt10075_Data.원주문번호 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "원주문번호").ToString().Trim(); //[11]
                    opt10075_Data.주문구분 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주문구분").ToString().Trim(); //[12]
                    opt10075_Data.매매구분 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매매구분").ToString().Trim(); //[13]
                    opt10075_Data.체결번호 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결번호").ToString().Trim(); //[15]
                    opt10075_Data.체결가 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결가").ToString().Trim()); //[16]
                    opt10075_Data.체결량 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결량").ToString().Trim()); //[17]
                    opt10075_Data.현재가 = int.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").ToString().Trim()); //[18]
                    String str매도호가= axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도호가").ToString().Trim(); //[19]
                    opt10075_Data.매도호가 = 0;
                    if (!str매도호가.Equals(""))
                    {
                        opt10075_Data.매도호가 = int.Parse(str매도호가);
                    }
                    String str매수호가 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수호가").ToString().Trim(); //[20]
                    opt10075_Data.매수호가 = 0;
                    if (!str매수호가.Equals(""))
                    {
                        opt10075_Data.매수호가 = int.Parse(str매수호가);
                    }
                    String str단위체결가 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "단위체결가").ToString().Trim(); //[21]
                    opt10075_Data.단위체결가 = 0;
                    if (!str단위체결가.Equals(""))
                    {
                        opt10075_Data.단위체결가 = int.Parse(str단위체결가);
                    }
                    String str단위체결량 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "단위체결량").ToString().Trim(); //[22]
                    opt10075_Data.단위체결량 = 0;
                    if (!str단위체결량.Equals(""))
                    {
                        opt10075_Data.단위체결량 = int.Parse(str단위체결량);
                    }
                    String str당일매매수수료 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매수수료").ToString().Trim(); //[23]
                    opt10075_Data.당일매매수수료 = 0;
                    if (!str당일매매수수료.Equals(""))
                    {
                        opt10075_Data.당일매매수수료 = int.Parse(str당일매매수수료);
                    }
                    String str당일매매세금 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매세금").ToString().Trim(); //[24]
                    opt10075_Data.당일매매세금 = 0;
                    if (!str당일매매세금.Equals(""))
                    {
                        opt10075_Data.당일매매세금 = int.Parse(str당일매매세금);
                    }
                    opt10075_Data.개인투자자 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "개인투자자").ToString().Trim(); //[25]
                    

                    //(8025-8089)/8089*100
                    //24005=23775+160+70
                    //24005-24575=-570
                    //24805
                    OPT10075_DataList.Add(opt10075_Data);
                    }
                }
           

            //이것은 연속적이지 않기 때문에 바로 제거 한다.
            AppLib.getClass1Instance().removeSpellDictionary(spell.key);
            int position = spell.key.LastIndexOf("|");
            String key1 = spell.key.Substring(0, position);
            AppLib.getClass1Instance().removeStockCodeDictionary(key1);
            //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
            AppLib.getClass1Instance().setOpt10081(spell.sTrCode);

            //DailyData dd = new DailyData();
            //dd.insertOpt10001(tmp);
        }

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
 [ opt10075 : 실시간미체결요청 ]
  1. Open API 조회 함수 입력값을 설정합니다.
	계좌번호 = 전문 조회할 보유계좌번호
	SetInputValue("계좌번호"	,  "입력값 1");
    체결구분 = 0:전체, 1:미체결
	SetInputValue("체결구분"	,  "입력값 2");
    매매구분 = 0:전체, 1:매도, 2:매수
	SetInputValue("매매구분"	,  "입력값 3");
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "opt10075"	,  "0"	,  "화면번호"); 
*/
            FileLog.PrintF("OPT10075:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10075:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10075:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10075:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10075:Run 계좌번호=>" + spell.accountNum);

            axKHOpenAPI.SetInputValue("계좌번호", spell.accountNum);
            axKHOpenAPI.SetInputValue("체결구분", spell.orderStatus);
            axKHOpenAPI.SetInputValue("매매구분", spell.orderGubun);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}

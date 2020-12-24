using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{
    /// <summary>
    ///  [ OPT10080 : 주식분봉차트조회요청 ]
    ///</summary>
    public class OPT10080: ReceiveTrData
    {
        public OPT10080() {
            FileLog.PrintF("OPT10080");
        }
        
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {

            /*
       sScrNo – 화면번호
       sRQName – 사용자구분 명
       sTrCode – Tran 명
       sRecordName – Record 명
       sPreNext – 연속조회 유무
       */
            FileLog.PrintF("ReceivedData OPT10080");
            try
            {
                StringBuilder sbAll = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                //sb.Append("일자:{0}|종목코드:{1}|현재가:{2}|거래량:{3}|거래대금:{4}|시가:{5}|고가:{6}|저가:{7}");
                //sb.Append("stock_date:{0}|stock_code:{1}|current_price:{2}|trade_quantity:{3}|trade_price:{4}|start_price:{5}|high_price:{6}|low_price:{7}");
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}");
                String tmp = sb.ToString();
                String 종목코드="XXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
            
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                종목코드 = AppLib.getClass1Instance().getStockCode(keyStockCode);

                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 종목코드
                );
            
                spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();
                String startDate = spell.startDate;
                String lastStockDate = "";
                long startDate일자 = 0;
                //시분초를 더함
                if (!long.TryParse(startDate+"000000", out startDate일자))
                {
                    startDate일자 = 0;
                }

                if (nCnt > 0)
                {
                    String 종목코드1 = "";
                    for (int i = 0; i < nCnt; i++)
                    {
                        if (i == 0)
                        {
                            종목코드1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();           //[0] 싱글 -첫번째에만 나타남
                        }
                    
                        int 현재가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());      //[1]
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());      //[2]
                        String 체결시간 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결시간").Trim();              //[3]
                        int 시가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim());            //[4]
                        int 고가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim());            //[5]
                        int 저가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim());            //[6]
                        int 수정주가구분 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가구분") !=null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가구분").ToString().Trim().Equals("")){
                            수정주가구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가구분").Trim());    //[7]
                        }
                        float 수정비율 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정비율") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정비율").ToString().Trim().Equals("")){
                            수정비율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정비율").Trim());            //[8]
                        }
                        int 대업종구분 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대업종구분") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대업종구분").ToString().Trim().Equals("")){
                            대업종구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대업종구분").Trim());        //[9]
                        }
                        int 소업종구분 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "소업종구분") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "소업종구분").ToString().Trim().Equals(""))
                        {
                            소업종구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "소업종구분").Trim());        //[10]
                        }
                        int 종목정보 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목정보") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목정보").ToString().Trim().Equals(""))
                        {
                            종목정보 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목정보").Trim());            //[11]
                        }
                        int 수정주가이벤트 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가이벤트") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가이벤트").ToString().Trim().Equals(""))
                        {
                            수정주가이벤트 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가이벤트").Trim());//[12]
                        }
                        int 전일종가 = 0;
                        if (axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일종가") != null && !axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일종가").ToString().Trim().Equals(""))
                        {
                            전일종가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일종가").Trim());            //[13]
                        }
    
                        String tmp1 = String.Format(tmp
                            , 종목코드1
                            , 현재가
                            , 거래량
                            , 체결시간
                            , 시가
                            , 고가
                            , 저가
                            , 수정주가구분
                            , 수정비율
                            , 대업종구분
                            , 소업종구분
                            , 종목정보
                            , 수정주가이벤트
                            , 전일종가
                        );
                        //String str일자 = 체결시간.Substring(0, 8);
                        long 일자 = long.Parse(체결시간);
                        lastStockDate = 체결시간;

                        if (startDate != null)
                        {
                            if (startDate.Equals("ZERO") || startDate.Equals("TWO"))
                            {
                                sbAll.AppendLine(tmp1);
                            }
                            else
                            {
                                if (일자 >= startDate일자)
                                {
                                    sbAll.AppendLine(tmp1);
                                } else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            sbAll.AppendLine(tmp1);
                        }
                    }
                } else
                {
                    종목코드 = "00000";
                }

                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext))
                {
                    prevNext = 0;
                }

                ScreenNumber.getClass1Instance().DisconnectRealData(e.sScrNo);
                ScreenNumber.getClass1Instance().SetRealRemove("ALL", "ALL");
                putReceivedQueueAndsetNextSpell(key,prevNext, lastStockDate);
             }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10080]Exception ex=" + ex.Message);
            }

        }  

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
            /*
            [opt10080: 주식분봉차트조회요청]
            1. Open API 조회 함수 입력값을 설정합니다.
            종목코드 = 전문 조회할 종목코드
            SetInputValue("종목코드"	,  "입력값 1");
            틱범위 = 1:1분, 3:3분, 5:5분, 10:10분, 15:15분, 30:30분, 45:45분, 60:60분
            SetInputValue("틱범위"	,  "입력값 2");
            수정주가구분 = 0 or 1, 수신데이터 1:유상증자, 2:무상증자, 4:배당락, 8:액면분할, 16:액면병합, 32:기업합병, 64:감자, 256:권리락
            SetInputValue("수정주가구분"	,  "입력값 3");
            2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
            CommRqData( "RQName"	,  "opt10080"	,  "0"	,  "화면번호");
            */
            FileLog.PrintF("OPT10080:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10080:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10080:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10080:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10080:Run 종목코드=> " + spell.stockCode);
            FileLog.PrintF("OPT10080:Run 틱범위=> " + spell.tick);
            FileLog.PrintF("OPT10080:Run 수정주가구분=> 1");

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            axKHOpenAPI.SetInputValue("틱범위", spell.tick);
            axKHOpenAPI.SetInputValue("수정주가구분", "1");/*수정주가 구분 이것도 무조건 1로 하자. */
                                                     /*  수정주가구분 = 0 or 1, 수신데이터 1:유상증자, 2:무상증자, 4:배당락, 8:액면분할, 16:액면병합, 32:기업합병, 64:감자, 256:권리락
                                                         SetInputValue("수정주가구분"	,  "입력값 3"); */
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}

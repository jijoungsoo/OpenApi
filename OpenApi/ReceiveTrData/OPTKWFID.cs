using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi.ReceiveTrData
{

    public class OPTKWFID : ReceiveTrData
    {
        public OPTKWFID(){
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPTKWFID");
            //try { 
            /*
            sScrNo – 화면번호
            sRQName – 사용자구분 명
            sTrCode – Tran 명
            sRecordName – Record 명
            sPreNext – 연속조회 유무
            */
            String 종목코드 = "XXXX";
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

            List<TB_OPTKWFID> lst = new List<TB_OPTKWFID>();


            if (nCnt > 0)
            {
                for (int i = 0; i < nCnt; i++)
                {
                    /*
                    FileLog.PrintF("OPTKWFID ReceivedData 종목코드 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim());//[1]
                    FileLog.PrintF("OPTKWFID ReceivedData 종목명 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim());//[2]
                    FileLog.PrintF("OPTKWFID ReceivedData 현재가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());//[3]
                    FileLog.PrintF("OPTKWFID ReceivedData 기준가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기준가").Trim());//[4]
                    FileLog.PrintF("OPTKWFID ReceivedData 전일대비 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim());//[5]
                    FileLog.PrintF("OPTKWFID ReceivedData 전일대비기호 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비기호").Trim());//[6]
                    FileLog.PrintF("OPTKWFID ReceivedData 등락율 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());//[7]
                    FileLog.PrintF("OPTKWFID ReceivedData 거래량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());//[8]
                    FileLog.PrintF("OPTKWFID ReceivedData 거래대금 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim());//[9]
                    FileLog.PrintF("OPTKWFID ReceivedData 체결량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결량").Trim());//[10]
                    FileLog.PrintF("OPTKWFID ReceivedData 체결강도 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결강도").Trim());//[11]
                    FileLog.PrintF("OPTKWFID ReceivedData 전일거래량대비 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일거래량대비").Trim());//[12]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도호가").Trim());//[13]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수호가").Trim());//[14]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도1차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도1차호가").Trim());//[15]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도2차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도2차호가").Trim());//[16]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도3차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도3차호가").Trim());//[17]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도4차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도4차호가").Trim());//[18]
                    FileLog.PrintF("OPTKWFID ReceivedData 매도5차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도5차호가").Trim());//[19]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수1차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수1차호가").Trim());//[20]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수2차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수2차호가").Trim());//[21]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수3차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수3차호가").Trim());//[22]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수4차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수4차호가").Trim());//[23]
                    FileLog.PrintF("OPTKWFID ReceivedData 매수5차호가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수5차호가").Trim());//[24]
                    FileLog.PrintF("OPTKWFID ReceivedData 상한가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "상한가").Trim());//[25]
                    FileLog.PrintF("OPTKWFID ReceivedData 하한가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "하한가").Trim());//[26]
                    FileLog.PrintF("OPTKWFID ReceivedData 시가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim());//[27]
                    FileLog.PrintF("OPTKWFID ReceivedData 고가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim());//[28]
                    FileLog.PrintF("OPTKWFID ReceivedData 저가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim());//[29]
                    FileLog.PrintF("OPTKWFID ReceivedData 종가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종가").Trim());//[30]
                    FileLog.PrintF("OPTKWFID ReceivedData 체결시간 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결시간").Trim());//[31]
                    FileLog.PrintF("OPTKWFID ReceivedData 예상체결가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결가").Trim());//[32]
                    FileLog.PrintF("OPTKWFID ReceivedData 예상체결량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결량").Trim());//[33]
                    FileLog.PrintF("OPTKWFID ReceivedData 자본금 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "자본금").Trim());//[34]
                    FileLog.PrintF("OPTKWFID ReceivedData 액면가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "액면가").Trim());//[35]
                    FileLog.PrintF("OPTKWFID ReceivedData 시가총액 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가총액").Trim());//[36]
                    FileLog.PrintF("OPTKWFID ReceivedData 주식수 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "주식수").Trim());//[37]
                    FileLog.PrintF("OPTKWFID ReceivedData 호가시간 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "호가시간").Trim());//[38]
                    FileLog.PrintF("OPTKWFID ReceivedData 일자 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[39]
                    FileLog.PrintF("OPTKWFID ReceivedData 우선매도잔량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매도잔량").Trim());//[40]
                    FileLog.PrintF("OPTKWFID ReceivedData 우선매수잔량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매수잔량").Trim());//[41]
                    FileLog.PrintF("OPTKWFID ReceivedData 우선매도건수 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매도건수").Trim());//[42]
                    FileLog.PrintF("OPTKWFID ReceivedData 우선매수건수 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매수건수").Trim());//[43]
                    FileLog.PrintF("OPTKWFID ReceivedData 총매도잔량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매도잔량").Trim());//[44]
                    FileLog.PrintF("OPTKWFID ReceivedData 총매수잔량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매수잔량").Trim());//[45]
                    FileLog.PrintF("OPTKWFID ReceivedData 총매도건수 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매도건수").Trim());//[46]
                    FileLog.PrintF("OPTKWFID ReceivedData 총매수건수 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매수건수").Trim());//[47]
                    FileLog.PrintF("OPTKWFID ReceivedData 패리티 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "패리티").Trim());//[48]
                    FileLog.PrintF("OPTKWFID ReceivedData 기어링 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기어링").Trim());//[49]
                    FileLog.PrintF("OPTKWFID ReceivedData 손익분기 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "손익분기").Trim());//[50]
                    FileLog.PrintF("OPTKWFID ReceivedData ELW행사가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "ELW행사가").Trim());//[51]
                    FileLog.PrintF("OPTKWFID ReceivedData 전환비율 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전환비율").Trim());//[52]
                    FileLog.PrintF("OPTKWFID ReceivedData ELW만기일 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "ELW만기일").Trim());//[53]
                    FileLog.PrintF("OPTKWFID ReceivedData 미결제약정 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미결제약정").Trim());//[54]
                    FileLog.PrintF("OPTKWFID ReceivedData 미결제전일대비 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미결제전일대비").Trim());//[55]
                    FileLog.PrintF("OPTKWFID ReceivedData 이론가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "이론가").Trim());//[56]
                    FileLog.PrintF("OPTKWFID ReceivedData 내재변동성 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "내재변동성").Trim());//[57]
                    FileLog.PrintF("OPTKWFID ReceivedData 델타 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "델타").Trim());//[58]
                    FileLog.PrintF("OPTKWFID ReceivedData 감마 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "감마").Trim());//[59]
                    FileLog.PrintF("OPTKWFID ReceivedData 세타 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "세타").Trim());//[60]
                    FileLog.PrintF("OPTKWFID ReceivedData 베가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "베가").Trim());//[61]
                    FileLog.PrintF("OPTKWFID ReceivedData 로 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "로").Trim());//[62]
                    */


                    /*
                     *[2020-04-18 00:45:39]OPTKWFID ReceivedData 종목코드 =>000390
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 종목명 =>삼화페인트
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 현재가 =>+5110
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 기준가 =>5040
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 전일대비 =>+70
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 전일대비기호 =>2
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 등락율 =>+1.39
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 거래량 =>50291
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 거래대금 =>256
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 체결량 =>+1
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 체결강도 =>108.71
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 전일거래량대비 =>+283.20
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도호가 =>+5120
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수호가 =>+5110
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도1차호가 =>+5120
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도2차호가 =>+5130
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도3차호가 =>+5140
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도4차호가 =>+5150
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매도5차호가 =>+5160
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수1차호가 =>+5110
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수2차호가 =>+5100
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수3차호가 =>+5090
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수4차호가 =>+5080
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 매수5차호가 =>+5070
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 상한가 =>+6550
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 하한가 =>-3530
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 시가 =>5040
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 고가 =>+5170
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 저가 =>5040
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 종가 =>+5110
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 체결시간 =>155926
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 예상체결가 =>+5110
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 예상체결량 =>334
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 자본금 =>132
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 액면가 =>500
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 시가총액 =>1351
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 주식수 =>26439
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 호가시간 =>160000
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 일자 =>20200417
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 우선매도잔량 =>270
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 우선매수잔량 =>117
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 우선매도건수 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 우선매수건수 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 총매도잔량 =>7991
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 총매수잔량 =>12270
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 총매도건수 =>-1000
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 총매수건수 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 패리티 =>0.00
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 기어링 =>0.00
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 손익분기 =>0.00
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData ELW행사가 =>0
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 전환비율 =>0.0000
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData ELW만기일 =>00000000
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 미결제약정 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 미결제전일대비 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 이론가 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 내재변동성 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 델타 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 감마 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 세타 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 베가 =>
                    [2020-04-18 00:45:39]OPTKWFID ReceivedData 로 =>
                    */



                    TB_OPTKWFID tb_optkwfid = new TB_OPTKWFID();

                    tb_optkwfid.stock_cd= axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();//[0]
                    tb_optkwfid.stock_nm = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim();//[1]

                    int 현재가 = 0;                   
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim(), out 현재가);
                    tb_optkwfid.curr_amt = 현재가;

                    int 기준가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기준가").Trim(), out 기준가);
                    tb_optkwfid.yesterday_amt = 기준가;

                    int 전일대비 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim(), out 전일대비);
                    tb_optkwfid.contrast_yesterday = 전일대비;

                    int 전일대비기호 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비기호").Trim(), out 전일대비기호);
                    tb_optkwfid.contrast_yesterday_symbol = 전일대비기호;

                    float 등락율 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim(), out 등락율);
                    tb_optkwfid.fluctuation_rt = 등락율;

                    int 거래량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim(), out 거래량);
                    tb_optkwfid.trade_qty = 거래량;

                    int 거래대금 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim(), out 거래대금);
                    tb_optkwfid.trade_amt = 거래대금;

                    int 체결량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결량").Trim(), out 체결량);
                    tb_optkwfid.contract_qty = 체결량;

                    float 체결강도 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결강도").Trim(), out 체결강도);
                    tb_optkwfid.contract_strength = 체결강도;

                    float 전일거래량대비 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일거래량대비").Trim(), out 전일거래량대비);
                    tb_optkwfid.yesterday_contrast_trade_rt = 전일거래량대비;

                    int 매도호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도호가").Trim(), out 매도호가);
                    tb_optkwfid.offered_amt = 매도호가;

                    int 매수호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수호가").Trim(), out 매수호가);
                    tb_optkwfid.bid_amt = 매수호가;

                    int 매도1차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도1차호가").Trim(), out 매도1차호가);
                    tb_optkwfid.offered_amt_one = 매도1차호가;

                    int 매도2차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도2차호가").Trim(), out 매도2차호가);
                    tb_optkwfid.offered_amt_two = 매도2차호가;

                    int 매도3차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도3차호가").Trim(), out 매도3차호가);
                    tb_optkwfid.offered_amt_three = 매도3차호가;

                    int 매도4차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도4차호가").Trim(), out 매도4차호가);
                    tb_optkwfid.offered_amt_four = 매도4차호가;

                    int 매도5차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도5차호가").Trim(), out 매도5차호가);
                    tb_optkwfid.offered_amt_five = 매도5차호가;

                    int 매수1차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수1차호가").Trim(), out 매수1차호가);
                    tb_optkwfid.bid_amt_one = 매수1차호가;

                    int 매수2차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수2차호가").Trim(), out 매수2차호가);
                    tb_optkwfid.bid_amt_two = 매수2차호가;

                    int 매수3차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수3차호가").Trim(), out 매수3차호가);
                    tb_optkwfid.bid_amt_three = 매수3차호가;

                    int 매수4차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수4차호가").Trim(), out 매수4차호가);
                    tb_optkwfid.bid_amt_four = 매수4차호가;

                    int 매수5차호가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매수5차호가").Trim(), out 매수5차호가);
                    tb_optkwfid.bid_amt_five = 매수5차호가;


                    int 상한가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "상한가").Trim(), out 상한가);
                    tb_optkwfid.upper_amt_lmt = 상한가;

                    int 하한가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "하한가").Trim(), out 하한가);
                    tb_optkwfid.lower_amt_lmt = 하한가;

                    int 시작가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시작가").Trim(), out 시작가);
                    tb_optkwfid.start_amt = 시작가;

                    int 고가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim(), out 고가);
                    tb_optkwfid.high_amt = 고가;

                    int 저가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim(), out 저가);
                    tb_optkwfid.low_amt = 저가;

                    int 종가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종가").Trim(), out 종가);
                    tb_optkwfid.clsg_amt = 종가;

                    tb_optkwfid.contract_time = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "체결시간").Trim();//[1]

                    int 예상체결가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결가").Trim(), out 예상체결가);
                    tb_optkwfid.expectation_contract_amt = 예상체결가;


                    int 예상체결수량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결수량").Trim(), out 예상체결수량);
                    tb_optkwfid.expectation_contract_qty = 예상체결수량;

                    int 자본금 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "자본금").Trim(), out 자본금);
                    tb_optkwfid.capital_amt = 자본금;

                    int 액면가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "액면가").Trim(), out 액면가);
                    tb_optkwfid.face_amt = 액면가;

                    int 시가총액 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가총액").Trim(), out 시가총액);
                    tb_optkwfid.total_mrkt_amt = 시가총액;

                    int 상장주식수 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "상장주식수").Trim(), out 상장주식수);
                    tb_optkwfid.stock_cnt = 상장주식수;

                    tb_optkwfid.hoga_time = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "호가시간").Trim();//[1]
                    tb_optkwfid.stock_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim();//[1]

                    int 우선매도잔량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매도잔량").Trim(), out 우선매도잔량);
                    tb_optkwfid.fst_offered_balance = 우선매도잔량;

                    int 우선매수잔량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매수잔량").Trim(), out 우선매수잔량);
                    tb_optkwfid.fst_bid_balance = 우선매수잔량;

                    int 우선매도건수 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매도건수").Trim(), out 우선매도건수);
                    tb_optkwfid.fst_offered_qty = 우선매도건수;

                    int 우선매수건수 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "우선매수건수").Trim(), out 우선매수건수);
                    tb_optkwfid.fst_bid_qty = 우선매수건수;

                    int 총매도잔량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매도잔량").Trim(), out 총매도잔량);
                    tb_optkwfid.tot_offered_balance = 총매도잔량;

                    int 총매수잔량 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매수잔량").Trim(), out 총매수잔량);
                    tb_optkwfid.tot_bid_balance = 총매수잔량;

                    int 총매도건수 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매도건수").Trim(), out 총매도건수);
                    tb_optkwfid.tot_offered_qty = 총매도건수;

                    int 총매수건수 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매수건수").Trim(), out 총매수건수);
                    tb_optkwfid.tot_bid_qty = 총매수건수;

                    float 패리티 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "패리티").Trim(), out 패리티);
                    tb_optkwfid.parity_rt = 패리티;

                    float 기어링 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기어링").Trim(), out 기어링);
                    tb_optkwfid.gearing = 기어링;

                    float 손익분기 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "손익분기").Trim(), out 손익분기);
                    tb_optkwfid.break_even_point = 기어링;

                    int ELW행사가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "ELW행사가").Trim(), out ELW행사가);
                    tb_optkwfid.elw_strike_amt = ELW행사가;

                    float 전환비율 = 0;
                    float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전환비율").Trim(), out 전환비율);
                    tb_optkwfid.conversion_rt = 전환비율;

                    tb_optkwfid.elw_expiry_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "ELW만기일").Trim();//[1]

                    int 미결제약정 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미결제약정").Trim(), out 미결제약정);
                    tb_optkwfid.open_interest = 미결제약정;

                    int 미결제전일대비 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "미결제전일대비").Trim(), out 미결제전일대비);
                    tb_optkwfid.contrast_open_interest = 미결제전일대비;


                    int 이론가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "이론가").Trim(), out 이론가);
                    tb_optkwfid.theorist_amt = 이론가;

                    int 내재변동성 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "내재변동성").Trim(), out 내재변동성);
                    tb_optkwfid.implied_volatility = 내재변동성;

                    int 델타 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "델타").Trim(), out 델타);
                    tb_optkwfid.delta = 델타;

                    int 감마 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "감마").Trim(), out 감마);
                    tb_optkwfid.gamma = 감마;


                    int 세타 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "세타").Trim(), out 세타);
                    tb_optkwfid.theta = 세타;

                    int 베가 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "베가").Trim(), out 베가);
                    tb_optkwfid.vega = 베가;

                    int 로 = 0;
                    int.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "로").Trim(), out 로);
                    tb_optkwfid.lo = 로;

                    //(8025-8089)/8089*100
                    //24005=23775+160+70
                    //24005-24575=-570
                    //24805
                    lst.Add(tb_optkwfid);
                }
            }

            //이것은 연속적이지 않기 때문에 바로 제거 한다.
            AppLib.getClass1Instance().removeSpellDictionary(spell.key);
            int position = spell.key.LastIndexOf("|");
            String key1 = spell.key.Substring(0, position);
            AppLib.getClass1Instance().removeStockCodeDictionary(key1);
            //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
            AppLib.getClass1Instance().setOpt10081(spell.sTrCode);

            DailyData dd = new DailyData();
            dd.insertOptkwfid(lst);
        }
     
        
        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
 [ opt10001 : 주식기본정보요청 ]
  1. Open API 조회 함수 입력값을 설정합니다.
	종목코드 = 전문 조회할 종목코드
	SetInputValue("종목코드"	,  "181710;066570");    
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "opt10001"	,  "0"	,  "화면번호"); 
            */
            /*정상동작확인 주석
            FileLog.PrintF("OPT10001:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10001:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10001:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10001:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10001:Run 종목코드=>" + spell.stockCode);
            */
            string[] stockCodep = spell.stockCode.Split(';');


            int ret = axKHOpenAPI.CommKwRqData(spell.stockCode, spell.nPrevNext, stockCodep.Length, 0 /*0 주식종목 , 3 선물옵션종목 */,spell.sRQNAME, spell.sScreenNo);
            return ret;
        }
    }
}

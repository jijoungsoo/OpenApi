using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{


    /// <summary>
    ///  [ OPW00004 : 계좌평가현황요청 ]
    ///[0346]실시간계좌관리(T)-잔고확인
    ///특이점 인덱스가 0 이어도 값이 있는 상태이다.
    ///0은 내 계좌 정보를 나타냄
    ///</summary>
    public class OPW00004: ReceiveTrData
    {


        public OPW00004() {
        }
        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            /*
            sScrNo – 화면번호
            sRQName – 사용자구분 명
            sTrCode – Tran 명
            sRecordName – Record 명
            sPreNext – 연속조회 유무
            */

            String stockDate = DateTime.Today.ToString("yyyyMMdd");
            

            StringBuilder sbAll = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            String 계좌명 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "계좌명").Trim();  //account_name
            String 지점명 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "지점명").Trim();  //place_name
            int 예수금= Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "예수금").Trim());   //deposit
            int D2추정예수금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "D+2추정예수금").Trim()); //twodays_after_deposit
            int 유가잔고평가액 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "유가잔고평가액").Trim()); //stock_evaluation,
            int 예탁자산평가액 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "예탁자산평가액").Trim()); //:stock_balance_evaluation,
            int 총매입금액 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "총매입금액").Trim()); //:total_amount_of_purchase,
            int 추정예탁자산 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "추정예탁자산").Trim()); //:estimation_deposit
            int 매도담보대출금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "매도담보대출금").Trim()); //stock_collacteral_loan,
            int 당일투자원금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당일투자원금").Trim()); //:today_investment_money,
            int 당월투자원금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당월투자원금").Trim()); //:this_month_investment_money,
            int 누적투자원금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "누적투자원금").Trim()); //:accumulative_investment_money,
            int 당일투자손익 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당일투자손익").Trim()); //:today_profit_and_loss,
            int 당월투자손익 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당월투자손익").Trim()); //:today_profit_and_loss,
            int 누적투자손익 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "누적투자손익").Trim()); //:today_profit_and_loss,
            int 당일손익율 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당일손익율").Trim());   //:today_profit_and_loss_rate
            int 당월손익율 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당월손익율").Trim()); //:this_month_profit_and_loss_rate
            int 누적손익율 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "누적손익율").Trim());  //:accumulative_profit_and_loss_rate
            int 출력건수 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "출력건수").Trim()); //:accumulative_profit_and_loss_rate
            sb.Append("날짜:{0}|계좌명:{1}|지점명:{2}|예수금:{3}|D+2추정예수금:{4}|유가잔고평가액:{5}|예탁자산평가액:{6}|총매입금액:{7}|추정예탁자산:{8}");
            sb.Append("|매도담보대출금:{9}|당일투자원금:{10}|당월투자원금:{11}|누적투자원금:{12}|당일투자손익:{13}|당월투자손익:{14}|누적투자손익:{15}");
            sb.Append("|당일손익율:{16}|당월손익율:{17}|누적손익율:{18}|출력건수:{19}");
            String tmp = sb.ToString();
            String tmp1=String.Format(tmp
                , stockDate
                , 계좌명
                , 지점명
                , 예수금
                , D2추정예수금
                , 유가잔고평가액
                , 예탁자산평가액
                , 총매입금액
                , 추정예탁자산
                , 매도담보대출금
                , 당일투자원금
                , 당월투자원금
                , 누적투자원금
                , 당일투자손익
                , 당월투자손익
                , 누적투자손익
                , 당일손익율
                , 당월손익율
                , 누적손익율
                , 출력건수
            );
            sbAll.AppendLine(tmp1);
            
            int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName); 
            for(int i = 1; i < nCnt; i++)
            {
                String 계좌명1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "계좌명").Trim();
                String 지점명1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "지점명").Trim(); 
                int 예수금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예수금").Trim()); 
                int D2추정예수금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "D+2추정예수금").Trim()); 
                int 유가잔고평가액1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "유가잔고평가액").Trim()); 
                int 예탁자산평가액1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예탁자산평가액").Trim()); 
                int 총매입금액1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "총매입금액").Trim()); 
                int 추정예탁자산1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "추정예탁자산").Trim()); 
                int 매도담보대출금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매도담보대출금").Trim()); 
                int 당일투자원금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일투자원금").Trim()); 
                int 당월투자원금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당월투자원금").Trim()); 
                int 누적투자원금1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "누적투자원금").Trim()); 
                int 당일투자손익1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일투자손익").Trim()); 
                int 당월투자손익1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당월투자손익").Trim()); 
                int 누적투자손익1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "누적투자손익").Trim()); 
                int 당일손익율1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일손익율").Trim());
                int 당월손익율1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "당월손익율").Trim());
                int 누적손익율1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "누적손익율").Trim()); 
                int 출력건수1 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "출력건수").Trim());

                tmp1=String.Format(tmp
                    , stockDate
                    , 계좌명1
                    , 지점명1
                    , 예수금1
                    , D2추정예수금1
                    , 유가잔고평가액1
                    , 예탁자산평가액1
                    , 총매입금액1
                    , 추정예탁자산1
                    , 매도담보대출금1
                    , 당일투자원금1
                    , 당월투자원금1
                    , 누적투자원금1
                    , 당일투자손익1
                    , 당월투자손익1
                    , 누적투자손익1
                    , 당일손익율1
                    , 당월손익율1
                    , 누적손익율1
                    , 출력건수1
                );
                sbAll.AppendLine(tmp1);
            }
            
            path = System.IO.Path.GetDirectoryName(path);
            path = path + "\\OPW00004_"+stockDate+".log";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path, false);
            file.Write(sbAll.ToString());
            file.Close();
        }

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
/*
[OPW00004: 계좌평가현황요청]
1. Open API 조회 함수 입력값을 설정합니다.
계좌번호 = 전문 조회할 보유계좌번호
SetInputValue("계좌번호"	,  "입력값 1");
비밀번호 = 사용안함(공백)
SetInputValue("비밀번호"	,  "입력값 2");
상장폐지조회구분 = 0:전체, 1:상장폐지종목제외
SetInputValue("상장폐지조회구분"	,  "입력값 3");
비밀번호입력매체구분 = 00
SetInputValue("비밀번호입력매체구분"	,  "입력값 4");
2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
CommRqData( "RQName"	,  "OPW00004"	,  "0"	,  "화면번호");
*/
            axKHOpenAPI.SetInputValue("계좌번호", spell.stockCode);
            axKHOpenAPI.SetInputValue("비밀번호", spell.endDate);
            axKHOpenAPI.SetInputValue("상장폐지조회구분", "0");
            axKHOpenAPI.SetInputValue("비밀번호입력매체구분", "00");
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}

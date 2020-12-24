using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPT10059
    {
        public String stock_cd; //종목코드
        public String stock_dt; //일자
        public String buy_sell; //1매수, 2 매도   ==> 2개 합치면 순매수 확인 가능
        public String amt_amount; //1금액,  2 수량  
        public int domestic_investor; //개인투자자
        public int foreign_investor; //외국인투자자
        public int institution; //기관계
        public int financial_investment; //금융투자
        public int insurance; //보험
        public int investment_trust; //투신
        public int etc_financial;// 기타금융
        public int bank;	//	은행
        public int pension_fund;	//	연기금등
        public int private_equity_fund;	//	사모펀드
        public int nation;  //	국가
        public int etc_corporation;  //	기타법인
        public int foregin_investment_in_korea;  //	내외국인
        public string prev_next;//0이면 종료 2이면 진행

    }
}
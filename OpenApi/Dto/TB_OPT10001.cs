using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPT10001
    {
        public String stock_cd; //종목코드
        public String stock_nm; //종목명
        public int face_amt; //액면가
        public int capital_amt; //자본금
        public int stock_cnt; //상장주식수
        public float credit_rt; //신용비율
        public String settlement_mm; //결산월
        public int year_high_amt; //연중최고
        public int year_low_amt;// 연중최저
        public int total_mrkt_amt;	//	시가총액
        public float total_mrkt_amt_rt;	//	시가총액비중
        public float foreigner_exhaustion_rt;	//	외인소진률
        public int substitute_amt;	//	대용가
        public float per;	//	주가수익률
        public float eps;	//	주당순이익
        public float roe;	//	자기자본이익률
        public float pbr;	//	주가순자산비율
        public float ev;	//	이자비용,법인세비용,     감가상각비용을     공제하기    전의      이익
        public float bps;	//	주당순자산가치
        public int sales;	//	매출액
        public int business_profits;	//	영업이익
        public int d250_high_amt;	//	D250최고
        public int d250_low_amt;	//	D250최저
        public int start_amt;	//	시작가
        public int high_amt;	//	고가
        public int low_amt;	//	저가
        public int upper_amt_lmt;	//	상한가
        public int lower_amt_lmt;	//	하한가
        public int yesterday_amt;	//	기준가
        public int expectation_contract_amt;	//	예상체결가
        public int expectation_contract_qty;	//	예상체결수량
        public String d250_high_dt;	//	D250최고가일
        public float d250_high_rt;	//	D250최고가대비율
        public String d250_low_dt;	//	D250최저가일
        public float d250_low_rt;	//	D250최저가대비율
        public int curr_amt;	//	현재가
        public int contrast_symbol;	//	대비기호
        public int contrast_yesterday;	//	전일대비
        public float fluctuation_rt;	//	등락율
        public int trade_qty;	//	거래량
        public float trade_contrast;	//	거래대비
        public String face_amt_unit;	//	액면가단위
        public int  distribution_stock;	//	유통주식
        public float distribution_stock_rt;	//	유통비율


    }
}

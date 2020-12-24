using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPTKWFID
    {
        public String stock_cd;                 //종목코드[0]
        public String stock_nm;                 //종목명[1]
        public int curr_amt;                    //현재가[2]
        public int yesterday_amt;               //기준가(어제종가)[3]
        public int contrast_yesterday;          //전일대비[4]
        public int contrast_yesterday_symbol;   //전일대비기호[5]
        public float fluctuation_rt;            //등락율[6]
        public int trade_qty;                   //거래량[7]
        public int trade_amt;                   //거래대금[8]
        public int contract_qty;	            //체결량[9]
        public float contract_strength;     	//체결강도[10]
        public float yesterday_contrast_trade_rt;	//전일거래량대비[11]
        public int offered_amt;	//	매도호가[12]
        public int bid_amt;	//	매수호가[13]  integer
        public int offered_amt_one;	//	매도1차호가[14]
        public int offered_amt_two;	//	매도2차호가[15]
        public int offered_amt_three;	//	매도3차호가[16]
        public int offered_amt_four;	//	매도4차호가[17]
        public int offered_amt_five;	//	매도5차호가[18]
        public int bid_amt_one;	//	매수1차호가[19]
        public int bid_amt_two;	//	매수2차호가[20]
        public int bid_amt_three;	//	매수3차호가[21]
        public int bid_amt_four;	//	매수4차호가[22]
        public int bid_amt_five;	//	매수5차호가[23]

        public int upper_amt_lmt;	//	상한가[24]
        public int lower_amt_lmt;	//	하한가[25]
        public int start_amt;	//	시작가[26]
        public int high_amt;	//	고가[27] integer
        public int low_amt;	    //	저가[28]
        public int clsg_amt;    //	종가[29]

        public string contract_time;	//	체결시간[30] 6자리 문자열
        public int expectation_contract_amt;	//	예상체결가[31]
        public int expectation_contract_qty;	//	예상체결수량[32]
        public int capital_amt;	//	자본금[33]
        public int face_amt;	//	액면가[34]
        public int total_mrkt_amt;	//	시가총액[45]
        public int stock_cnt;   //	상장주식수[46]


        public string hoga_time;	//	호가시간[47]  6자리 문자열
        public string stock_dt;	//	일자[48] 8자리 문자열
        public int fst_offered_balance;	//	우선매도잔량[49]  integer
        public int fst_bid_balance;	//	우선매수잔량[50]
        public int fst_offered_qty;	//	우선매도건수[51]
        public int fst_bid_qty;	//	우선매수건수[52]
        public int tot_offered_balance;	//	총매도잔량[53]
        public int tot_bid_balance; //	총매수잔량[54]
        public int tot_offered_qty;	//	총매도건수[55]
        public int tot_bid_qty;	//	총매수건수[56]


        public float parity_rt;	//	패리티[57]
        public float gearing;	//	기어링[58]
        public float break_even_point;	//	손익분기[59]  (Break Even Point)
        public int elw_strike_amt;	//	ELW행사가[60] (Strike Price)  --옵션의 약속된가격
        public float conversion_rt;	//	전환비율[61]
        public String elw_expiry_dt;	//	ELW만기일[62] 8자리 문자열
        public int open_interest;	//	미결제약정[63]   -- https://lifeisfatal.tistory.com/entry/%EB%AF%B8%EA%B2%B0%EC%A0%9C-%EC%95%BD%EC%A0%95open-interest
        public int contrast_open_interest;	//	미결제전일대비[64]
        public int theorist_amt;  //이론가[65]
        public int implied_volatility;  //내재변동성[66]  --https://blog.naver.com/PostView.nhn?blogId=stochastic73&logNo=221722483534&parentCategoryNo=1&categoryNo=20&viewDate=&isShowPopularPosts=false&from=postView

        public int delta; //델타[67]
        public int gamma; //감마[68]
        public int theta; //세타[69]
        public int vega; //베가[70]
        public int lo; //로[71]
    }
}

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Spell
{
    public class SpellOpt
    {
        public String key;
        public String value;
        public String sRQNAME;
        public String sTrCode;
        public String stockCode;
        public String startDate;   
        public String endDate;          /*일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)*/
        public String sScreenNo;
        public int nPrevNext;
        public String priceOrAmount;    /*금액수량구분 = 1:금액, 2:수량*/
        public String buyOrSell;       /*매매구분 = 0:순매수, 1:매수, 2:매도*/
        public String lastStockDate;
        public String reportGubun;  /*FILE ==>파일로 남기기 SOAP==>SOAP전송*/
        public String tick; /*틱범위 = 1:1분, 3:3분, 5:5분, 10:10분, 15:15분, 30:30분, 45:45분, 60:60분*/
        public DateTime startRunTime ;
        public String accountNum;//계좌번호
        public String password; //계좌비밀번호
        public String orderGubun; //매매구분 => 0:전체, 1:매도, 2:매수
        public String orderStatus; //체결구분 =>  0:전체, 1:미체결

        public SpellOpt ShallowCopy() 
        {
            return (SpellOpt)this.MemberwiseClone();
        }


        public Boolean isNext()
        {
            if (this.sTrCode.Equals("OPT10001"))
            {
                FileLog.PrintF("isNext() nPrevNext=" + nPrevNext);
                return false;
            }

                if (this.nPrevNext > 0 && this.startDate.Equals("TWO"))
            {
                return true;
            }
            else
            {
                if(this.startDate.Equals("ZERO"))
                {
                    return false;
                } else 
                {
                    if(this.nPrevNext>0) { 
                        long startDate일자 = 0;
                        long lastStockDate일자 = 0;

                        String tmpStartDate = "";
                        if (this.startDate.Length == 8) { 
                            tmpStartDate= this.startDate + "000000";//시분초를 더함
                        } else
                        {
                            tmpStartDate = this.startDate;
                        }
                        String tmpLastStockDate = "";
                        if (this.lastStockDate.Length == 8)
                        {
                            tmpLastStockDate = this.lastStockDate + "000000";//시분초를 더함
                        } else
                        {
                            tmpLastStockDate = this.lastStockDate;
                        }
                        if (!long.TryParse(tmpStartDate, out startDate일자))
                        {
                            startDate일자 = 0;
                        }
                          
                        if (!long.TryParse(tmpLastStockDate, out lastStockDate일자))
                        {
                            lastStockDate일자 = 0;
                        }
                        //FileLog.PrintF("isNext() startDate일자=" + startDate일자.ToString() + ",lastStockDate일자=" + lastStockDate일자.ToString());
                        if (startDate일자 < lastStockDate일자)
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    } else
                    {
                        return false;
                    }
                }               
            }
        }
    }
}

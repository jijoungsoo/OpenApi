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

namespace OpenApi.ReceiveRealData
{
    /// <summary>
    ///  [ REAL10004 : 주식호가잔량---일단거의 쓸일이 없을것 같긴하다... ]
    ///</summary>
    public class REAL10004 : ReceiveRealData
    {
        public REAL10004(){
            FileLog.PrintF("REAL10003");
        }
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /*
    [21] = 호가시간                 //(0)
	[41] = 매도호가1                //(1)
	[61] = 매도호가수량1            //(2)
	[81] = 매도호가직전대비1        //(3)
	[51] = 매수호가1                //(4)
	[71] = 매수호가수량1            //(5)
	[91] = 매수호가직전대비1        //(6)
	[42] = 매도호가2                //(7)
	[62] = 매도호가수량2            //(8)
	[82] = 매도호가직전대비2        //(9)
	[52] = 매수호가2                //(10)
	[72] = 매수호가수량2            //(11)
	[92] = 매수호가직전대비2        //(12)
	[43] = 매도호가3                //(13)
	[63] = 매도호가수량3            //(14)
	[83] = 매도호가직전대비3        //(15)
	[53] = 매수호가3                //(16)
	[73] = 매수호가수량3            //(17)
	[93] = 매수호가직전대비3        //(18)
	[44] = 매도호가4                //(19)
	[64] = 매도호가수량4            //(20)
	[84] = 매도호가직전대비4        //(21)
	[54] = 매수호가4                //(22)
	[74] = 매수호가수량4            //(23)
	[94] = 매수호가직전대비4        //(24)
	[45] = 매도호가5                //(25)
	[65] = 매도호가수량5            //(26)
	[85] = 매도호가직전대비5        //(27)
	[55] = 매수호가5                //(28)
	[75] = 매수호가수량5            //(29)
	[95] = 매수호가직전대비5        //(30)
	[46] = 매도호가6                //(31)
	[66] = 매도호가수량6            //(32)
	[86] = 매도호가직전대비6        //(33)
	[56] = 매수호가6                //(34)
	[76] = 매수호가수량6            //(35)
	[96] = 매수호가직전대비6        //(36)
	[47] = 매도호가7                //(37)
	[67] = 매도호가수량7            //(38)
	[87] = 매도호가직전대비7        //(39)
	[57] = 매수호가7                //(40)
	[77] = 매수호가수량7            //(41)
	[97] = 매수호가직전대비7        //(42)
	[48] = 매도호가8                //(43)
	[68] = 매도호가수량8            //(44)
	[88] = 매도호가직전대비8        //(45)
	[58] = 매수호가8                //(46)
	[78] = 매수호가수량8            //(47)
	[98] = 매수호가직전대비8        //(48)
	[49] = 매도호가9                //(49)
	[69] = 매도호가수량9            //(50)
	[89] = 매도호가직전대비9        //(51)
	[59] = 매수호가9                //(52)
	[79] = 매수호가수량9            //(53)
	[99] = 매수호가직전대비9        //(54)
	[50] = 매도호가10               //(55)
	[70] = 매도호가수량10           //(56)
	[90] = 매도호가직전대비10       //(57)
	[60] = 매수호가10               //(58)
	[80] = 매수호가수량10           //(59)
	[100] = 매수호가직전대비10      //(60)
	[121] = 매도호가총잔량          //(61)
	[122] = 매도호가총잔량직전대비  //(62)
	[125] = 매수호가총잔량          //(63)
	[126] = 매수호가총잔량직전대비  //(64)
	[23] = 예상체결가               //(65)
	[24] = 예상체결수량             //(66)
	[128] = 순매수잔량              //(67)
	[129] = 매수비율                //(68)
	[138] = 순매도잔량              //(69)
	[139] = 매도비율                //(70)
	[200] = 예상체결가전일종가대비  //(71)
	[201] = 예상체결가전일종가대비등락율  //(72)
	[238] = 예상체결가전일종가대비기호  //(73)
	[291] = 예상체결가  //(74)
	[292] = 예상체결량1  //(75)
	[293] = 예상체결가전일대비기호 //(76)
	[294] = 예상체결가전일대비 //(77)
	[295] = 예상체결가전일대비등락율 //(78)
	[621] = LP매도호가수량1  //(79)
	[631] = LP매수호가수량1  //(80)
	[622] = LP매도호가수량2  //(81)
	[632] = LP매수호가수량2  //(82)
	[623] = LP매도호가수량3  //(83)
	[633] = LP매수호가수량3  //(84)
	[624] = LP매도호가수량4  //(85)
	[634] = LP매수호가수량4  //(86)
	[625] = LP매도호가수량5  //(87)
	[635] = LP매수호가수량5  //(88)
	[626] = LP매도호가수량6  //(89)
	[636] = LP매수호가수량6  //(90)
	[627] = LP매도호가수량7  //(91)
	[637] = LP매수호가수량7  //(92)
	[628] = LP매도호가수량8  //(93)
	[638] = LP매수호가수량8  //(94)
	[629] = LP매도호가수량9  //(95)
	[639] = LP매수호가수량9  //(96)
	[630] = LP매도호가수량10 //(97)
	[640] = LP매수호가수량10 //(98)
	[13] = 누적거래량        //(99)
	[299] = 전일거래량대비예상체결률  //(100)
	[215] = 장운영구분       //(101)
	[216] = 투자자별ticker   //(102)
            */
            FileLog.PrintF(String.Format("호가시간 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 21).Trim()));   //[0]
            FileLog.PrintF(String.Format("매도호가1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 41).Trim()));     //[1]
            FileLog.PrintF(String.Format("매도호가수량1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 61).Trim()));     //[2]
            FileLog.PrintF(String.Format("매도호가직전대비1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 81).Trim()));     //[3]
            FileLog.PrintF(String.Format("매수호가1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 51).Trim()));     //[4]
            FileLog.PrintF(String.Format("매수호가수량1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 71).Trim()));     //[5]
            FileLog.PrintF(String.Format("매수호가직전대비1 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 91).Trim()));     //[6]
            FileLog.PrintF(String.Format("매도호가2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 42).Trim()));     //[7]
            FileLog.PrintF(String.Format("매도호가수량2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 62).Trim()));     //[8]
            FileLog.PrintF(String.Format("매도호가직전대비2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 82).Trim()));     //[9]
            FileLog.PrintF(String.Format("매수호가2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 52).Trim()));     //[10]
            FileLog.PrintF(String.Format("매수호가수량2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 72).Trim()));     //[11]
            FileLog.PrintF(String.Format("매수호가직전대비2 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 92).Trim()));     //[12]
            FileLog.PrintF(String.Format("매도호가3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 43).Trim()));     //[13]
            FileLog.PrintF(String.Format("매도호가수량3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 63).Trim()));     //[14]
            FileLog.PrintF(String.Format("매도호가직전대비3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 83).Trim()));     //[15]
            FileLog.PrintF(String.Format("매수호가3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 53).Trim()));     //[16]
            FileLog.PrintF(String.Format("매수호가수량3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 73).Trim()));     //[17]
            FileLog.PrintF(String.Format("매수호가직전대비3 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 93).Trim()));     //[18]
            FileLog.PrintF(String.Format("매도호가4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 44).Trim()));     //[19]
            FileLog.PrintF(String.Format("매도호가수량4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 64).Trim()));     //[20]
            FileLog.PrintF(String.Format("매도호가직전대비4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 84).Trim()));     //[21]
            FileLog.PrintF(String.Format("매수호가4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 54).Trim()));     //[22]
            FileLog.PrintF(String.Format("매수호가수량4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 74).Trim()));     //[23]
            FileLog.PrintF(String.Format("매수호가직전대비4 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 94).Trim()));     //[24]
            FileLog.PrintF(String.Format("매도호가5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 45).Trim()));     //[25]
            FileLog.PrintF(String.Format("매도호가수량5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 65).Trim()));     //[26]
            FileLog.PrintF(String.Format("매도호가직전대비5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 85).Trim()));     //[27]
            FileLog.PrintF(String.Format("매수호가5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 55).Trim()));     //[28]
            FileLog.PrintF(String.Format("매수호가수량5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 75).Trim()));     //[29]
            FileLog.PrintF(String.Format("매수호가직전대비5 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 95).Trim()));     //[30]
            FileLog.PrintF(String.Format("매도호가6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 46).Trim()));     //[31]
            FileLog.PrintF(String.Format("매도호가수량6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 66).Trim()));     //[32]
            FileLog.PrintF(String.Format("매도호가직전대비6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 86).Trim()));     //[33]
            FileLog.PrintF(String.Format("매수호가6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 56).Trim()));     //[34]
            FileLog.PrintF(String.Format("매수호가수량6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 76).Trim()));     //[35]
            FileLog.PrintF(String.Format("매수호가직전대비6 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 96).Trim()));     //[36]
            FileLog.PrintF(String.Format("매도호가7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 47).Trim()));     //[37]
            FileLog.PrintF(String.Format("매도호가수량7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 67).Trim()));     //[38]
            FileLog.PrintF(String.Format("매도호가직전대비7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 87).Trim()));     //[39]
            FileLog.PrintF(String.Format("매수호가7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 57).Trim()));     //[40]
            FileLog.PrintF(String.Format("매수호가수량7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 77).Trim()));     //[41]
            FileLog.PrintF(String.Format("매수호가직전대비7 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 97).Trim()));     //[42]
            FileLog.PrintF(String.Format("매도호가8 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 48).Trim()));     //[43]
            FileLog.PrintF(String.Format("매도호가수량8 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 68).Trim()));     //[44]
            FileLog.PrintF(String.Format("매도호가직전대비8 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 88).Trim())); //[45]
            FileLog.PrintF(String.Format("매수호가8 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 58).Trim())); //[46]
            FileLog.PrintF(String.Format("매수호가수량8 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 78).Trim()));     //[47]
            FileLog.PrintF(String.Format("매수호가직전대비8 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 98).Trim()));     //[48]
            FileLog.PrintF(String.Format("매도호가9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 49).Trim()));     //[49]
            FileLog.PrintF(String.Format("매도호가수량9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 69).Trim()));     //[50]
            FileLog.PrintF(String.Format("매도호가직전대비9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 89).Trim()));     //[51]
            FileLog.PrintF(String.Format("매수호가9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 59).Trim())); //[52]
            FileLog.PrintF(String.Format("매수호가수량9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 79).Trim())); //[53]
            FileLog.PrintF(String.Format("매수호가직전대비9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 99).Trim())); //[54]
            FileLog.PrintF(String.Format("매도호가10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 50).Trim()));   //[55]
            FileLog.PrintF(String.Format("매도호가수량10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 70).Trim()));   //[56]
            FileLog.PrintF(String.Format("매도호가직전대비10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 90).Trim()));   //[57]
            FileLog.PrintF(String.Format("매수호가10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 60).Trim()));   //[58]
            FileLog.PrintF(String.Format("매수호가수량10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 80).Trim()));   //[59]
            FileLog.PrintF(String.Format("매수호가직전대비10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 100).Trim()));   //[60]
            FileLog.PrintF(String.Format("매도호가총잔량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 121).Trim()));   //[61]
            FileLog.PrintF(String.Format("매도호가총잔량직전대비 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 122).Trim()));   //[62]
            FileLog.PrintF(String.Format("매수호가총잔량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 125).Trim()));   //[63]
            FileLog.PrintF(String.Format("매수호가총잔량직전대비 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 126).Trim()));   //[64]
            FileLog.PrintF(String.Format("예상체결가 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 23).Trim()));   //[65]
            FileLog.PrintF(String.Format("예상체결수량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 24).Trim()));   //[66]
            FileLog.PrintF(String.Format("순매수잔량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 128).Trim()));   //[67]
            FileLog.PrintF(String.Format("매수비율 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 129).Trim()));   //[68]
            FileLog.PrintF(String.Format("순매도잔량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 138).Trim()));   //[69]
            FileLog.PrintF(String.Format("매도비율 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 139).Trim()));   //[70]
            FileLog.PrintF(String.Format("예상체결가전일종가대비 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 200).Trim()));   //[71]
            FileLog.PrintF(String.Format("예상체결가전일종가대비등락율 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 201).Trim()));   //[72]
            FileLog.PrintF(String.Format("예상체결가전일종가대비기호 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 238).Trim()));   //[73]
            FileLog.PrintF(String.Format("예상체결가1 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 291).Trim()));   //[74]
            FileLog.PrintF(String.Format("예상체결량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 292).Trim()));   //[75]
            FileLog.PrintF(String.Format("예상체결가전일대비기호 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 293).Trim()));   //[76]
            FileLog.PrintF(String.Format("예상체결가전일대비 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 294).Trim()));   //[77]
            FileLog.PrintF(String.Format("예상체결가전일대비등락율 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 295).Trim()));   //[78]
            FileLog.PrintF(String.Format("LP매도호가수량1 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 621).Trim()));   //[79]
            FileLog.PrintF(String.Format("LP매수호가수량1 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 631).Trim()));   //[80]
            FileLog.PrintF(String.Format("LP매도호가수량2 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 622).Trim()));   //[81]
            FileLog.PrintF(String.Format("LP매수호가수량2 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 632).Trim()));   //[82]
            FileLog.PrintF(String.Format("LP매도호가수량3 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 623).Trim()));   //[83]
            FileLog.PrintF(String.Format("LP매수호가수량3 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 633).Trim()));   //[84]
            FileLog.PrintF(String.Format("LP매도호가수량4 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 624).Trim()));   //[85]
            FileLog.PrintF(String.Format("LP매수호가수량4 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 634).Trim()));   //[86]
            FileLog.PrintF(String.Format("LP매도호가수량5 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 625).Trim()));   //[87]
            FileLog.PrintF(String.Format("LP매수호가수량5 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 635).Trim()));   //[88]
            FileLog.PrintF(String.Format("LP매도호가수량6 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 626).Trim()));   //[89]
            FileLog.PrintF(String.Format("LP매수호가수량6 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 636).Trim()));   //[90]
            FileLog.PrintF(String.Format("LP매도호가수량7 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 627).Trim()));   //[91]
            FileLog.PrintF(String.Format("LP매수호가수량7 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 637).Trim()));   //[92]
            FileLog.PrintF(String.Format("LP매도호가수량8 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 628).Trim()));   //[93]
            FileLog.PrintF(String.Format("LP매수호가수량8 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 638).Trim()));   //[94]
            FileLog.PrintF(String.Format("LP매도호가수량9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 629).Trim()));   //[95]
            FileLog.PrintF(String.Format("LP매수호가수량9 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 639).Trim()));   //[96]
            FileLog.PrintF(String.Format("LP매도호가수량10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 630).Trim()));   //[97]
            FileLog.PrintF(String.Format("LP매수호가수량10 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 640).Trim()));   //[98]
            FileLog.PrintF(String.Format("누적거래량 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim()));   //[99]
            FileLog.PrintF(String.Format("전일거래량대비예상체결률 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 299).Trim()));   //[100]
            FileLog.PrintF(String.Format("장운영구분 : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 215).Trim()));   //[101]
            FileLog.PrintF(String.Format("투자자별ticker : {0}==>", axKHOpenAPI.GetCommRealData(e.sRealType, 216).Trim()));   //[102]
            FileLog.PrintF(String.Format("RealName : {0} ==>", e.sRealType.ToString().Trim()));
            FileLog.PrintF(String.Format("sRealData : {0} ==>", e.sRealData.ToString().Trim()));
            
            String 현재시간 = DateTime.Now.ToString("yyyy-MM-dd");
            


            String 현재일자 = DateTime.Now.ToString("yyyy-MM-dd");
            String 호가시간TMP = axKHOpenAPI.GetCommRealData(e.sRealType, 21).Trim();   //[0]
            //체결시간이 6자리이므로 HHMMSS ==> HH:MM:SS로 바꿔야한다.
            String 호가시간 = 호가시간TMP.Substring(0, 2) + ":" + 호가시간TMP.Substring(2, 2) + ":" + 호가시간TMP.Substring(4, 2);
            호가시간 = 현재일자 + " " + 호가시간;

            REAL10004_Data real10004_data = new REAL10004_Data();
            real10004_data.호가시간 = 호가시간;
            real10004_data.매도호가1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 41).Trim());     //[1]
            real10004_data.매도호가수량1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 61).Trim());     //[2]
            real10004_data.매도호가직전대비1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 81).Trim());     //[3]
            real10004_data.매수호가1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 51).Trim());     //[4]
            real10004_data.매수호가수량1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 71).Trim());     //[5]
            real10004_data.매수호가직전대비1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 91).Trim());     //[6]
            real10004_data.매도호가2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 42).Trim());     //[7]
            real10004_data.매도호가수량2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 62).Trim());     //[8]
            real10004_data.매도호가직전대비2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 82).Trim());     //[9]
            real10004_data.매수호가2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 52).Trim());     //[10]
            real10004_data.매수호가수량2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 72).Trim());     //[11]
            real10004_data.매수호가직전대비2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 92).Trim());     //[12]
            real10004_data.매도호가3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 43).Trim());     //[13]
            real10004_data.매도호가수량3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 63).Trim());     //[14]
            real10004_data.매도호가직전대비3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 83).Trim());     //[15]
            real10004_data.매수호가3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 53).Trim());     //[16]
            real10004_data.매수호가수량3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 73).Trim());     //[17]
            real10004_data.매수호가직전대비3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 93).Trim());     //[18]
            real10004_data.매도호가4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 44).Trim());     //[19]
            real10004_data.매도호가수량4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 64).Trim());     //[20]
            real10004_data.매도호가직전대비4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 84).Trim());     //[21]
            real10004_data.매수호가4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 54).Trim());     //[22]
            real10004_data.매수호가수량4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 74).Trim());     //[23]
            real10004_data.매수호가직전대비4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 94).Trim());     //[24]
            real10004_data.매도호가5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 45).Trim());     //[25]
            real10004_data.매도호가수량5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 65).Trim());     //[26]
            real10004_data.매도호가직전대비5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 85).Trim());     //[27]
            real10004_data.매수호가5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 55).Trim());     //[28]
            real10004_data.매수호가수량5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 75).Trim());     //[29]
            real10004_data.매수호가직전대비5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 95).Trim());     //[30]
            real10004_data.매도호가6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 46).Trim());     //[31]
            real10004_data.매도호가수량6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 66).Trim());     //[32]
            real10004_data.매도호가직전대비6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 86).Trim());     //[33]
            real10004_data.매수호가6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 56).Trim());     //[34]
            real10004_data.매수호가수량6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 76).Trim());     //[35]
            real10004_data.매수호가직전대비6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 96).Trim());     //[36]
            real10004_data.매도호가7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 47).Trim());     //[37]
            real10004_data.매도호가수량7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 67).Trim());     //[38]
            real10004_data.매도호가직전대비7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 87).Trim());     //[39]
            real10004_data.매수호가7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 57).Trim());     //[40]
            real10004_data.매수호가수량7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 77).Trim());     //[41]
            real10004_data.매수호가직전대비7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 97).Trim());     //[42]
            real10004_data.매도호가8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 48).Trim());     //[43]
            real10004_data.매도호가수량8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 68).Trim());     //[44]
            real10004_data.매도호가직전대비8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 88).Trim()); //[45]
            real10004_data.매수호가8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 58).Trim()); //[46]
            real10004_data.매수호가수량8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 78).Trim());     //[47]
            real10004_data.매수호가직전대비8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 98).Trim());     //[48]
            real10004_data.매도호가9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 49).Trim());     //[49]
            real10004_data.매도호가수량9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 69).Trim());     //[50]
            real10004_data.매도호가직전대비9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 89).Trim());     //[51]
            real10004_data.매수호가9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 59).Trim()); //[52]
            real10004_data.매수호가수량9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 79).Trim()); //[53]
            real10004_data.매수호가직전대비9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 99).Trim()); //[54]
            real10004_data.매도호가10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 50).Trim());   //[55]
            real10004_data.매도호가수량10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 70).Trim());   //[56]
            real10004_data.매도호가직전대비10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 90).Trim());   //[57]
            real10004_data.매수호가10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 60).Trim());   //[58]
            real10004_data.매수호가수량10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 80).Trim());   //[59]
            real10004_data.매수호가직전대비10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 100).Trim());   //[60]
            real10004_data.매도호가총잔량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 121).Trim());   //[61]
            real10004_data.매도호가총잔량직전대비 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 122).Trim());   //[62]
            real10004_data.매수호가총잔량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 125).Trim());   //[63]
            real10004_data.매수호가총잔량직전대비 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 126).Trim());   //[64]
            real10004_data.예상체결가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 23).Trim());   //[65]
            real10004_data.예상체결수량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 24).Trim());   //[66]
            real10004_data.순매수잔량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 128).Trim());   //[67]
            real10004_data.매수비율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 129).Trim());   //[68]
            real10004_data.순매도잔량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 138).Trim());   //[69]
            real10004_data.매도비율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 139).Trim());   //[70]
            real10004_data.예상체결가전일종가대비 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 200).Trim());   //[71]
            real10004_data.예상체결가전일종가대비등락율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 201).Trim());   //[72]
            real10004_data.예상체결가전일종가대비기호 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 238).Trim());   //[73]
            real10004_data.예상체결가1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 291).Trim());   //[74]
            real10004_data.예상체결량1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 292).Trim());   //[75]
            real10004_data.예상체결가전일대비기호1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 293).Trim());   //[76]
            real10004_data.예상체결가전일대비1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 294).Trim());   //[77]
            real10004_data.예상체결가전일대비등락율1 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 295).Trim());   //[78]
            real10004_data.LP매도호가수량1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 621).Trim());   //[79]
            real10004_data.LP매수호가수량1 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 631).Trim());   //[80]
            real10004_data.LP매도호가수량2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 622).Trim());   //[81]
            real10004_data.LP매수호가수량2 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 632).Trim());   //[82]
            real10004_data.LP매도호가수량3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 623).Trim());   //[83]
            real10004_data.LP매수호가수량3 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 633).Trim());   //[84]
            real10004_data.LP매도호가수량4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 624).Trim());   //[85]
            real10004_data.LP매수호가수량4 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 634).Trim());   //[86]
            real10004_data.LP매도호가수량5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 625).Trim());   //[87]
            real10004_data.LP매수호가수량5 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 635).Trim());   //[88]
            real10004_data.LP매도호가수량6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 626).Trim());   //[89]
            real10004_data.LP매수호가수량6 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 636).Trim());   //[90]
            real10004_data.LP매도호가수량7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 627).Trim());   //[91]
            real10004_data.LP매수호가수량7 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 637).Trim());   //[92]
            real10004_data.LP매도호가수량8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 628).Trim());   //[93]
            real10004_data.LP매수호가수량8 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 638).Trim());   //[94]
            real10004_data.LP매도호가수량9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 629).Trim());   //[95]
            real10004_data.LP매수호가수량9 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 639).Trim());   //[96]
            real10004_data.LP매도호가수량10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 630).Trim());   //[97]
            real10004_data.LP매수호가수량10 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 640).Trim());   //[98]
            real10004_data.누적거래량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim());   //[99]
            real10004_data.전일거래량대비예상체결률 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 299).Trim());   //[100]
            real10004_data.장운영구분 = axKHOpenAPI.GetCommRealData(e.sRealType, 215).Trim();   //[101]
            real10004_data.투자자별ticker = axKHOpenAPI.GetCommRealData(e.sRealType, 216).Trim();   //[102]
            real10004_data.종목코드 = e.sRealKey.ToString().Trim(); //[2]
            real10004_data.RealName = e.sRealType.ToString().Trim(); //[3]
                          
            
                SendDirectFile(real10004_data);
                SendDirectDb(real10004_data);
        }

        private void SendDirectFile(REAL10004_Data real10004_data)
        {

            String tmp = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}|{54}|{55}|{56}|{57}|{58}|{59}|{60}|{61}|{62}|{63}|{64}|{65}|{66}|{67}|{68}|{69}|{70}|{71}|{72}|{73}|{74}|{75}|{76}|{77}|{78}|{79}|{80}|{81}|{82}|{83}|{84}|{85}|{86}|{87}|{88}|{89}|{90}|{91}|{92}|{93}|{94}|{95}|{96}|{97}|{98}|{99}|{100}|{101}|{102}|{103}|{104}";
            String tmp1 = String.Format(tmp
,real10004_data.호가시간
,real10004_data.매도호가1
,real10004_data.매도호가수량1
,real10004_data.매도호가직전대비1
,real10004_data.매수호가1
,real10004_data.매수호가수량1
,real10004_data.매수호가직전대비1
,real10004_data.매도호가2
,real10004_data.매도호가수량2
,real10004_data.매도호가직전대비2
,real10004_data.매수호가2
,real10004_data.매수호가수량2
,real10004_data.매수호가직전대비2
,real10004_data.매도호가3
,real10004_data.매도호가수량3
,real10004_data.매도호가직전대비3
,real10004_data.매수호가3
,real10004_data.매수호가수량3
,real10004_data.매수호가직전대비3
,real10004_data.매도호가4
,real10004_data.매도호가수량4
,real10004_data.매도호가직전대비4
,real10004_data.매수호가4
,real10004_data.매수호가수량4
,real10004_data.매수호가직전대비4
,real10004_data.매도호가5
,real10004_data.매도호가수량5
,real10004_data.매도호가직전대비5
,real10004_data.매수호가5
,real10004_data.매수호가수량5
,real10004_data.매수호가직전대비5
,real10004_data.매도호가6
,real10004_data.매도호가수량6
,real10004_data.매도호가직전대비6
,real10004_data.매수호가6
,real10004_data.매수호가수량6
,real10004_data.매수호가직전대비6
,real10004_data.매도호가7
,real10004_data.매도호가수량7
,real10004_data.매도호가직전대비7
,real10004_data.매수호가7
,real10004_data.매수호가수량7
,real10004_data.매수호가직전대비7
,real10004_data.매도호가8
,real10004_data.매도호가수량8
,real10004_data.매도호가직전대비8
,real10004_data.매수호가8
,real10004_data.매수호가수량8
,real10004_data.매수호가직전대비8
,real10004_data.매도호가9
,real10004_data.매도호가수량9
,real10004_data.매도호가직전대비9
,real10004_data.매수호가9
,real10004_data.매수호가수량9
,real10004_data.매수호가직전대비9
,real10004_data.매도호가10
,real10004_data.매도호가수량10
,real10004_data.매도호가직전대비10
,real10004_data.매수호가10
,real10004_data.매수호가수량10
,real10004_data.매수호가직전대비10
,real10004_data.매도호가총잔량
,real10004_data.매도호가총잔량직전대비
,real10004_data.매수호가총잔량
,real10004_data.매수호가총잔량직전대비
,real10004_data.예상체결가
,real10004_data.예상체결수량
,real10004_data.순매수잔량
,real10004_data.매수비율
,real10004_data.순매도잔량
,real10004_data.매도비율
,real10004_data.예상체결가전일종가대비
,real10004_data.예상체결가전일종가대비등락율
,real10004_data.예상체결가전일종가대비기호
,real10004_data.예상체결가1
,real10004_data.예상체결량1
,real10004_data.예상체결가전일대비기호1
,real10004_data.예상체결가전일대비1
,real10004_data.예상체결가전일대비등락율1
,real10004_data.LP매도호가수량1
,real10004_data.LP매수호가수량1
,real10004_data.LP매도호가수량2
,real10004_data.LP매수호가수량2
,real10004_data.LP매도호가수량3
,real10004_data.LP매수호가수량3
,real10004_data.LP매도호가수량4
,real10004_data.LP매수호가수량4
,real10004_data.LP매도호가수량5
,real10004_data.LP매수호가수량5
,real10004_data.LP매도호가수량6
,real10004_data.LP매수호가수량6
,real10004_data.LP매도호가수량7
,real10004_data.LP매수호가수량7
,real10004_data.LP매도호가수량8
,real10004_data.LP매수호가수량8
,real10004_data.LP매도호가수량9
,real10004_data.LP매수호가수량9
,real10004_data.LP매도호가수량10
,real10004_data.LP매수호가수량10
,real10004_data.누적거래량
,real10004_data.전일거래량대비예상체결률
,real10004_data.장운영구분
,real10004_data.투자자별ticker
,real10004_data.종목코드
,real10004_data.RealName
             );

            System.IO.StreamWriter file = new System.IO.StreamWriter(Config.GetPath() + "\\주식호가잔량.txt", true);
            file.WriteLine(tmp1.ToString());
            file.Close();
        }
        private void SendDirectDb(REAL10004_Data real10004_data)
        {
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String logTime = real10004_data.호가시간;
                DateTime dayT = DateTime.ParseExact(dayTime, "yyyy-MM-dd HH:mm:ss", null);
                DateTime logT = DateTime.ParseExact(logTime, "yyyy-MM-dd HH:mm:ss", null);
                TimeSpan t = dayT - logT;
                string sql = @"INSERT into realtime_offered_and_bids (
stock_date
,stock_code
,offered_price1
,offered_quantity1
,offered_price_contrast1
,bid_price1
,bid_quantity1
,bid_price_contrast1
,offered_price2
,offered_quantity2
,offered_price_contrast2
,bid_price2
,bid_quantity2
,bid_price_contrast2
,offered_price3
,offered_quantity3
,offered_price_contrast3
,bid_price3
,bid_quantity3
,bid_price_contrast3
,offered_price4
,offered_quantity4
,offered_price_contrast4
,bid_price4
,bid_quantity4
,bid_price_contrast4
,offered_price5
,offered_quantity5
,offered_price_contrast5
,bid_price5
,bid_quantity5
,bid_price_contrast5
,offered_price6
,offered_quantity6
,offered_price_contrast6
,bid_price6
,bid_quantity6
,bid_price_contrast6
,offered_price7
,offered_quantity7
,offered_price_contrast7
,bid_price7
,bid_quantity7
,bid_price_contrast7
,offered_price8
,offered_quantity8
,offered_price_contrast8
,bid_price8
,bid_quantity8
,bid_price_contrast8
,offered_price9
,offered_quantity9
,offered_price_contrast9
,bid_price9
,bid_quantity9
,bid_price_contrast9
,offered_price10
,offered_quantity10
,offered_price_contrast10
,bid_price10
,bid_quantity10
,bid_price_contrast10
,offered_total_residual_quantity
,offered_total_residual_quantity_contrast
,bid_total_residual_quantity
,bid_total_residual_quantity_contrast
,expectation_contract_price
,expectation_contract_quantity
,net_buy_residual_quantity
,bid_rate
,net_sell_residual_quantity
,offered_rate
,expectation_contract_yesterday_contrast_price
,expectation_contract_yesterday_contrast_fluctuation_rate
,expectation_contract_yesterday_contrast_symbol
,expectation_contract_price1
,expectation_contract_quantity1
,expectation_contract_yesterday_contrast_symbol1
,expectation_contract_yesterday_contrast_price1
,expectation_contract_yesterday_contrast_fluctuation_rate1
,lp_offered_quantity1
,lp_bid_quantity1
,lp_offered_quantity2
,lp_bid_quantity2
,lp_offered_quantity3
,lp_bid_quantity3
,lp_offered_quantity4
,lp_bid_quantity4
,lp_offered_quantity5
,lp_bid_quantity5
,lp_offered_quantity6
,lp_bid_quantity6
,lp_offered_quantity7
,lp_bid_quantity7
,lp_offered_quantity8
,lp_bid_quantity8
,lp_offered_quantity9
,lp_bid_quantity9
,lp_offered_quantity10
,lp_bid_quantity10
,accumulated_trade_quantity
,expectation_contract_yesterday_contrast_rate
,market_gubun
,investor_ticker
,created_at
,updated_at
,time_diff
)
VALUES
(
@호가시간
,@종목코드
,@매도호가1
,@매도호가수량1
,@매도호가직전대비1
,@매수호가1
,@매수호가수량1
,@매수호가직전대비1
,@매도호가2
,@매도호가수량2
,@매도호가직전대비2
,@매수호가2
,@매수호가수량2
,@매수호가직전대비2
,@매도호가3
,@매도호가수량3
,@매도호가직전대비3
,@매수호가3
,@매수호가수량3
,@매수호가직전대비3
,@매도호가4
,@매도호가수량4
,@매도호가직전대비4
,@매수호가4
,@매수호가수량4
,@매수호가직전대비4
,@매도호가5
,@매도호가수량5
,@매도호가직전대비5
,@매수호가5
,@매수호가수량5
,@매수호가직전대비5
,@매도호가6
,@매도호가수량6
,@매도호가직전대비6
,@매수호가6
,@매수호가수량6
,@매수호가직전대비6
,@매도호가7
,@매도호가수량7
,@매도호가직전대비7
,@매수호가7
,@매수호가수량7
,@매수호가직전대비7
,@매도호가8
,@매도호가수량8
,@매도호가직전대비8
,@매수호가8
,@매수호가수량8
,@매수호가직전대비8
,@매도호가9
,@매도호가수량9
,@매도호가직전대비9
,@매수호가9
,@매수호가수량9
,@매수호가직전대비9
,@매도호가10
,@매도호가수량10
,@매도호가직전대비10
,@매수호가10
,@매수호가수량10
,@매수호가직전대비10
,@매도호가총잔량
,@매도호가총잔량직전대비
,@매수호가총잔량
,@매수호가총잔량직전대비
,@예상체결가
,@예상체결수량
,@순매수잔량
,@매수비율
,@순매도잔량
,@매도비율
,@예상체결가전일종가대비
,@예상체결가전일종가대비등락율
,@예상체결가전일종가대비기호
,@예상체결가1
,@예상체결량1
,@예상체결가전일대비기호1
,@예상체결가전일대비1
,@예상체결가전일대비등락율1
,@LP매도호가수량1
,@LP매수호가수량1
,@LP매도호가수량2
,@LP매수호가수량2
,@LP매도호가수량3
,@LP매수호가수량3
,@LP매도호가수량4
,@LP매수호가수량4
,@LP매도호가수량5
,@LP매수호가수량5
,@LP매도호가수량6
,@LP매수호가수량6
,@LP매도호가수량7
,@LP매수호가수량7
,@LP매도호가수량8
,@LP매수호가수량8
,@LP매도호가수량9
,@LP매수호가수량9
,@LP매도호가수량10
,@LP매수호가수량10
,@누적거래량
,@전일거래량대비예상체결률
,@장운영구분
,@투자자별ticker
,current_timestamp
,current_timestamp
,@time_diff
);
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@호가시간",real10004_data.호가시간);
                cmd.Parameters.AddWithValue("@종목코드",real10004_data.종목코드);
                cmd.Parameters.AddWithValue("@매도호가1",real10004_data.매도호가1);
                cmd.Parameters.AddWithValue("@매도호가수량1",real10004_data.매도호가수량1);
                cmd.Parameters.AddWithValue("@매도호가직전대비1",real10004_data.매도호가직전대비1);
                cmd.Parameters.AddWithValue("@매수호가1",real10004_data.매수호가1);
                cmd.Parameters.AddWithValue("@매수호가수량1",real10004_data.매수호가수량1);
                cmd.Parameters.AddWithValue("@매수호가직전대비1",real10004_data.매수호가직전대비1);
                cmd.Parameters.AddWithValue("@매도호가2",real10004_data.매도호가2);
                cmd.Parameters.AddWithValue("@매도호가수량2",real10004_data.매도호가수량2);
                cmd.Parameters.AddWithValue("@매도호가직전대비2",real10004_data.매도호가직전대비2);
                cmd.Parameters.AddWithValue("@매수호가2",real10004_data.매수호가2);
                cmd.Parameters.AddWithValue("@매수호가수량2",real10004_data.매수호가수량2);
                cmd.Parameters.AddWithValue("@매수호가직전대비2",real10004_data.매수호가직전대비2);
                cmd.Parameters.AddWithValue("@매도호가3",real10004_data.매도호가3);
                cmd.Parameters.AddWithValue("@매도호가수량3",real10004_data.매도호가수량3);
                cmd.Parameters.AddWithValue("@매도호가직전대비3",real10004_data.매도호가직전대비3);
                cmd.Parameters.AddWithValue("@매수호가3",real10004_data.매수호가3);
                cmd.Parameters.AddWithValue("@매수호가수량3",real10004_data.매수호가수량3);
                cmd.Parameters.AddWithValue("@매수호가직전대비3",real10004_data.매수호가직전대비3);
                cmd.Parameters.AddWithValue("@매도호가4",real10004_data.매도호가4);
                cmd.Parameters.AddWithValue("@매도호가수량4",real10004_data.매도호가수량4);
                cmd.Parameters.AddWithValue("@매도호가직전대비4",real10004_data.매도호가직전대비4);
                cmd.Parameters.AddWithValue("@매수호가4",real10004_data.매수호가4);
                cmd.Parameters.AddWithValue("@매수호가수량4",real10004_data.매수호가수량4);
                cmd.Parameters.AddWithValue("@매수호가직전대비4",real10004_data.매수호가직전대비4);
                cmd.Parameters.AddWithValue("@매도호가5",real10004_data.매도호가5);
                cmd.Parameters.AddWithValue("@매도호가수량5",real10004_data.매도호가수량5);
                cmd.Parameters.AddWithValue("@매도호가직전대비5",real10004_data.매도호가직전대비5);
                cmd.Parameters.AddWithValue("@매수호가5",real10004_data.매수호가5);
                cmd.Parameters.AddWithValue("@매수호가수량5",real10004_data.매수호가수량5);
                cmd.Parameters.AddWithValue("@매수호가직전대비5",real10004_data.매수호가직전대비5);
                cmd.Parameters.AddWithValue("@매도호가6",real10004_data.매도호가6);
                cmd.Parameters.AddWithValue("@매도호가수량6",real10004_data.매도호가수량6);
                cmd.Parameters.AddWithValue("@매도호가직전대비6",real10004_data.매도호가직전대비6);
                cmd.Parameters.AddWithValue("@매수호가6",real10004_data.매수호가6);
                cmd.Parameters.AddWithValue("@매수호가수량6",real10004_data.매수호가수량6);
                cmd.Parameters.AddWithValue("@매수호가직전대비6",real10004_data.매수호가직전대비6);
                cmd.Parameters.AddWithValue("@매도호가7",real10004_data.매도호가7);
                cmd.Parameters.AddWithValue("@매도호가수량7",real10004_data.매도호가수량7);
                cmd.Parameters.AddWithValue("@매도호가직전대비7",real10004_data.매도호가직전대비7);
                cmd.Parameters.AddWithValue("@매수호가7",real10004_data.매수호가7);
                cmd.Parameters.AddWithValue("@매수호가수량7",real10004_data.매수호가수량7);
                cmd.Parameters.AddWithValue("@매수호가직전대비7",real10004_data.매수호가직전대비7);
                cmd.Parameters.AddWithValue("@매도호가8",real10004_data.매도호가8);
                cmd.Parameters.AddWithValue("@매도호가수량8",real10004_data.매도호가수량8);
                cmd.Parameters.AddWithValue("@매도호가직전대비8",real10004_data.매도호가직전대비8);
                cmd.Parameters.AddWithValue("@매수호가8",real10004_data.매수호가8);
                cmd.Parameters.AddWithValue("@매수호가수량8",real10004_data.매수호가수량8);
                cmd.Parameters.AddWithValue("@매수호가직전대비8",real10004_data.매수호가직전대비8);
                cmd.Parameters.AddWithValue("@매도호가9",real10004_data.매도호가9);
                cmd.Parameters.AddWithValue("@매도호가수량9",real10004_data.매도호가수량9);
                cmd.Parameters.AddWithValue("@매도호가직전대비9",real10004_data.매도호가직전대비9);
                cmd.Parameters.AddWithValue("@매수호가9",real10004_data.매수호가9);
                cmd.Parameters.AddWithValue("@매수호가수량9",real10004_data.매수호가수량9);
                cmd.Parameters.AddWithValue("@매수호가직전대비9",real10004_data.매수호가직전대비9);
                cmd.Parameters.AddWithValue("@매도호가10",real10004_data.매도호가10);
                cmd.Parameters.AddWithValue("@매도호가수량10",real10004_data.매도호가수량10);
                cmd.Parameters.AddWithValue("@매도호가직전대비10",real10004_data.매도호가직전대비10);
                cmd.Parameters.AddWithValue("@매수호가10",real10004_data.매수호가10);
                cmd.Parameters.AddWithValue("@매수호가수량10",real10004_data.매수호가수량10);
                cmd.Parameters.AddWithValue("@매수호가직전대비10",real10004_data.매수호가직전대비10);
                cmd.Parameters.AddWithValue("@매도호가총잔량",real10004_data.매도호가총잔량);
                cmd.Parameters.AddWithValue("@매도호가총잔량직전대비",real10004_data.매도호가총잔량직전대비);
                cmd.Parameters.AddWithValue("@매수호가총잔량",real10004_data.매수호가총잔량);
                cmd.Parameters.AddWithValue("@매수호가총잔량직전대비",real10004_data.매수호가총잔량직전대비);
                cmd.Parameters.AddWithValue("@예상체결가",real10004_data.예상체결가);
                cmd.Parameters.AddWithValue("@예상체결수량",real10004_data.예상체결수량);
                cmd.Parameters.AddWithValue("@순매수잔량",real10004_data.순매수잔량);
                cmd.Parameters.AddWithValue("@매수비율",real10004_data.매수비율);
                cmd.Parameters.AddWithValue("@순매도잔량",real10004_data.순매도잔량);
                cmd.Parameters.AddWithValue("@매도비율",real10004_data.매도비율);
                cmd.Parameters.AddWithValue("@예상체결가전일종가대비",real10004_data.예상체결가전일종가대비);
                cmd.Parameters.AddWithValue("@예상체결가전일종가대비등락율",real10004_data.예상체결가전일종가대비등락율);
                cmd.Parameters.AddWithValue("@예상체결가전일종가대비기호",real10004_data.예상체결가전일종가대비기호);
                cmd.Parameters.AddWithValue("@예상체결가1",real10004_data.예상체결가1);
                cmd.Parameters.AddWithValue("@예상체결량1",real10004_data.예상체결량1);
                cmd.Parameters.AddWithValue("@예상체결가전일대비기호1",real10004_data.예상체결가전일대비기호1);
                cmd.Parameters.AddWithValue("@예상체결가전일대비1",real10004_data.예상체결가전일대비1);
                cmd.Parameters.AddWithValue("@예상체결가전일대비등락율1",real10004_data.예상체결가전일대비등락율1);
                cmd.Parameters.AddWithValue("@LP매도호가수량1",real10004_data.LP매도호가수량1);
                cmd.Parameters.AddWithValue("@LP매수호가수량1",real10004_data.LP매수호가수량1);
                cmd.Parameters.AddWithValue("@LP매도호가수량2",real10004_data.LP매도호가수량2);
                cmd.Parameters.AddWithValue("@LP매수호가수량2",real10004_data.LP매수호가수량2);
                cmd.Parameters.AddWithValue("@LP매도호가수량3",real10004_data.LP매도호가수량3);
                cmd.Parameters.AddWithValue("@LP매수호가수량3",real10004_data.LP매수호가수량3);
                cmd.Parameters.AddWithValue("@LP매도호가수량4",real10004_data.LP매도호가수량4);
                cmd.Parameters.AddWithValue("@LP매수호가수량4",real10004_data.LP매수호가수량4);
                cmd.Parameters.AddWithValue("@LP매도호가수량5",real10004_data.LP매도호가수량5);
                cmd.Parameters.AddWithValue("@LP매수호가수량5",real10004_data.LP매수호가수량5);
                cmd.Parameters.AddWithValue("@LP매도호가수량6",real10004_data.LP매도호가수량6);
                cmd.Parameters.AddWithValue("@LP매수호가수량6",real10004_data.LP매수호가수량6);
                cmd.Parameters.AddWithValue("@LP매도호가수량7",real10004_data.LP매도호가수량7);
                cmd.Parameters.AddWithValue("@LP매수호가수량7",real10004_data.LP매수호가수량7);
                cmd.Parameters.AddWithValue("@LP매도호가수량8",real10004_data.LP매도호가수량8);
                cmd.Parameters.AddWithValue("@LP매수호가수량8",real10004_data.LP매수호가수량8);
                cmd.Parameters.AddWithValue("@LP매도호가수량9",real10004_data.LP매도호가수량9);
                cmd.Parameters.AddWithValue("@LP매수호가수량9",real10004_data.LP매수호가수량9);
                cmd.Parameters.AddWithValue("@LP매도호가수량10",real10004_data.LP매도호가수량10);
                cmd.Parameters.AddWithValue("@LP매수호가수량10",real10004_data.LP매수호가수량10);
                cmd.Parameters.AddWithValue("@누적거래량",real10004_data.누적거래량);
                cmd.Parameters.AddWithValue("@전일거래량대비예상체결률",real10004_data.전일거래량대비예상체결률);
                cmd.Parameters.AddWithValue("@장운영구분",real10004_data.장운영구분);
                cmd.Parameters.AddWithValue("@투자자별ticker",real10004_data.투자자별ticker);
                cmd.Parameters.AddWithValue("@time_diff", t.TotalSeconds);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

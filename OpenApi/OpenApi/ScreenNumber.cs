using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;

namespace OpenApi
{
    class ScreenNumber
    {
        private int _scrNum = 5000;
        private int _scrNumRealTime = 1500;

        private int _scrNumAnyTime = 1000;

        private static Boolean _multiThread = true;

        private AxKHOpenAPI axKHOpenAPI=AppLib.getClass1Instance().getAxKHOpenAPIInstance();

        private static ScreenNumber _screenNumber = null;
        private static Object _object1 = new Object();
        public static ScreenNumber getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_screenNumber == null)
                {
                    _screenNumber = new ScreenNumber();
                }
            }
            else {
                if (_screenNumber == null)
                {
                    lock (_object1)
                    {
                        _screenNumber = new ScreenNumber();
                    }
                }
            }
            return _screenNumber;
        }


        // EOS 연결 종료
        public void DisconnectAllEOSData()
        {
            FileLog.PrintF("DisconnectAllEOSData");
            for (int i = _scrNum; i >= 5000; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }
            _scrNum = 5000;
            axKHOpenAPI.CommTerminate();
        }


        //EOS 화면번호 생산
        public string GetEosScrNum()
        {
            //스크린번호는 0000 부터 9999까지 쓸수있으나.  사용갯수가 200개로 한정되어있다.
            //조회는 초당 5회 제한이 있다. 0.2초..
            //어차피 큐에 넣고 쓰니까!! 200개 한정은 크게 의미 없다.
            if (_scrNum < 9999)
                _scrNum++;
            else
                _scrNum = 5000;

            return _scrNum.ToString();
        }


        public void DisconnectAllAnyTimeData()
        {
            for (int i = _scrNumAnyTime; i >= 1500; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }
            _scrNumAnyTime = 1500;
            axKHOpenAPI.CommTerminate();
        }

        public void DisconnectAllRealTimeData()
        {
            for (int i = _scrNumRealTime; i >= 1000; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }
            _scrNumRealTime = 1000;
            axKHOpenAPI.CommTerminate();
        }

        // 화면번호 생산
        public string GetAnyTimeScrNum()
        {

            if (_scrNumAnyTime < 5000)
                _scrNumAnyTime++;
            else
                _scrNumAnyTime = 1500;

            return _scrNumAnyTime.ToString();
        }

        // 화면번호 생산
        public string GetRealTimeScrNum()
        {
            if (_scrNumRealTime < 1500)
                _scrNumRealTime++;
            else
                _scrNumRealTime = 1000;


            return _scrNumRealTime.ToString();
        }


        public void DisconnectRealData(String sScrNo)
        {
            
            axKHOpenAPI.DisconnectRealData(sScrNo);
        }

        public void SetRealRemove(String sScrNo, String strDelCode)
        {
            axKHOpenAPI.SetRealRemove(sScrNo, strDelCode);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KiwoomCode;
using System.Collections.Concurrent;
using System.IO;

namespace OpenApi
{
    class FileLog
    {
        //private static FileInfo fileInfo = new FileInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\log.log");
        private static readonly object locker = new object();
        public static void PrintF(String line)
        {
            String day = DateTime.Now.ToString("yyyyMMdd");
            FileInfo fileInfo = new FileInfo(Config.GetPath() + "log_"+ day + ".log");
            //   Console.WriteLine("PrintF");
            /*
            음 이것도.. UI 쓰레드에서 돌리면 부하가 걸리는구나..
            그럼 결국엔.. 이것도.. 큐에 넣었다가
            쓰레드로 파일에 쓰는 작업을 해야겠네..

            */
            lock (locker)
            {
                /*
                String tmp1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                line = "[" + tmp1 + "]" + line;
                System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                file.WriteLine(line);
                file.Close();
                */
                String tmp1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                line = "[" + tmp1 + "]" + line;
                using (StreamWriter FileWriter = fileInfo.AppendText())
                {
                    FileWriter.WriteLine(line);
                }
            }

            /* 오히려 쓰레드가 더 느리네.. 쓰레드 삭제.
            Thread t1 = new Thread(() => WirteFile(line));
            t1.Start();
            */
        }
        private static void WirteFile(String line)
        {
      
        }


    // 로그를 출력합니다.
    public static void PrintF(Log type, string format, params Object[] args)
        {
            string message = String.Format(format, args);
            
            switch (type)
            {
                case Log.조회:
                    message = "[조회]" + message;
                    break;
                case Log.에러:
                    message = "[에러]" + message;
                    break;
                case Log.일반:
                    message = "[일반]" + message;
                    break;
                case Log.실시간:
                    message = "[실시간]" + message;
                    break;
                case Log.디버깅:
                    message = "[디버깅]" + message;
                    break;
                default:
                    break;
            }
            PrintF(message);
        }
        
        public static void alert(String message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Net;
using System.IO;
using System.Threading;

namespace OpenApi
{
    class StockFile
    {

        public static void EOS_CompressZip()
        {
            while (true)
            {
                FileLog.PrintF("EOS_CompressZip");
                //1. 주식일봉차트조회
                string zipPath = Config.GetPath() + "OPT10081_" + AppLib.getClass1Instance().endDateEos + ".zip";
                string zipCheckPath = Config.GetPath() + "OPT10081.dat";
                string ftpCheckPath = Config.GetPath() + "OPT10081_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10081 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10081 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10081_", "");
                        DeleteFile(zipPath, "OPT10081_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                //2. 일별거래상세요청
                zipPath = Config.GetPath() + "OPT10015_" + AppLib.getClass1Instance().endDateEos + ".zip";
                zipCheckPath = Config.GetPath() + "OPT10015.dat";
                ftpCheckPath = Config.GetPath() + "OPT10015_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10015 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10015 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10015_", "");
                        DeleteFile(zipPath, "OPT10015_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                //3. 종목별투자자기관별차트
                zipPath = Config.GetPath() + "OPT10059_" + AppLib.getClass1Instance().endDateEos + "_1_1.zip";
                zipCheckPath = Config.GetPath() + "OPT10059_1_1.dat";
                ftpCheckPath = Config.GetPath() + "OPT10059_1_1_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_1 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_1 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_1_1");
                        DeleteFile(zipPath, "OPT10059_", "_1_1");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = Config.GetPath() + "OPT10059_" + AppLib.getClass1Instance().endDateEos + "_1_2.zip";
                zipCheckPath = Config.GetPath() + "OPT10059_1_2.dat";
                ftpCheckPath = Config.GetPath() + "OPT10059_1_2_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_2 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_2 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_1_2");
                        DeleteFile(zipPath, "OPT10059_", "_1_2");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = Config.GetPath() + "OPT10059_" + AppLib.getClass1Instance().endDateEos + "_2_1.zip";
                zipCheckPath = Config.GetPath() + "OPT10059_2_1.dat";
                ftpCheckPath = Config.GetPath() + "OPT10059_2_1_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_1 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_1 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_2_1");
                        DeleteFile(zipPath, "OPT10059_", "_2_1");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = Config.GetPath() + "OPT10059_" + AppLib.getClass1Instance().endDateEos + "_2_2.zip";
                zipCheckPath = Config.GetPath() + "OPT10059_2_2.dat";
                ftpCheckPath = Config.GetPath() + "OPT10059_2_2_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_2 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_2 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_2_2");
                        DeleteFile(zipPath, "OPT10059_", "_2_2");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                //4. 공매도추이요청
                zipPath = Config.GetPath() + "OPT10014_" + AppLib.getClass1Instance().endDateEos + ".zip";
                zipCheckPath = Config.GetPath() + "OPT10014.dat";
                ftpCheckPath = Config.GetPath() + "OPT10014_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10014 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10014 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10014_", "");
                        DeleteFile(zipPath, "OPT10014_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }

                //5. 주식일봉차트조회요청
                zipPath = Config.GetPath() + "OPT10080_" + AppLib.getClass1Instance().endDateEos + ".zip";
                zipCheckPath = Config.GetPath() + "OPT10080.dat";
                ftpCheckPath = Config.GetPath() + "OPT10080_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10080 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10080 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10080_", "");
                        DeleteFile(zipPath, "OPT10080_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }

                //5. 주식일봉차트조회요청
                zipPath = Config.GetPath() + "OPT10001_" + AppLib.getClass1Instance().endDateEos + ".zip";
                zipCheckPath = Config.GetPath() + "OPT10001.dat";
                ftpCheckPath = Config.GetPath() + "OPT10001_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10001 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10001 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10001_", "");
                        DeleteFile(zipPath, "OPT10001_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                Thread.Sleep(300000);//5분이면 압축이 되겠지???
            }
        }

        private static void DeleteFile(string zipPath, string startsWith, string endsWith)
        {
            FileLog.PrintF("DeleteFile zipPath=" + zipPath);
            FileLog.PrintF("DeleteFile startsWith=" + startsWith);
            FileLog.PrintF("DeleteFile endsWith=" + endsWith);
            DirectoryInfo directorySelected = new DirectoryInfo(Config.GetPath());
            //Directory.GetFiles(path,"*.txt",SearchOption.TopDirectoryOnly).Where(s=>s.StartsWith(startsWith));
            FileInfo[] fileList = directorySelected.GetFiles(startsWith + "*" + endsWith + ".txt");
            if (fileList.Count() > 0)
            {
                foreach (FileInfo file in fileList)
                {
                    file.Delete();
                }

            }

        }

        public static void UploadEosFiles()
        {
            while (true)
            {
                FileLog.PrintF("UploadEosFiles");
                uploadZipFiles();
                Thread.Sleep(300000); // 5분에 한번씩 검사해서 ftp 업로드
            }
        }

        private static void uploadZipFiles()
        {
            FtpUtil ftpUtil = new FtpUtil("192.168.0.30", Config.GetFtpUser(), Config.GetFtpPwd(), "21");
            DirectoryInfo directorySelected = new DirectoryInfo(Config.GetPath());
            DirectoryInfo backDictory = new DirectoryInfo(Config.GetBackUpPath());
            if (backDictory.Exists == false)
            {
                backDictory.Create();
            }
            if (
                (File.Exists(Config.GetPath() + "OPT10001_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10014_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10015_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10059_1_1_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10059_1_2_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10059_2_1_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10059_2_2_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10080_FTP.dat") == true)
                && (File.Exists(Config.GetPath() + "OPT10081_FTP.dat") == true)
            )
            {
                //FileInfo[] fileList = directorySelected.GetFiles(startsWithZip + "*" + endsWithZip + ".zip");
                FileInfo[] fileList = directorySelected.GetFiles("*.zip");  //모든 zip파일을 옮기자
                foreach (FileInfo file in fileList)
                {
                    bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/data", file.FullName);
                    FileLog.PrintF("UploadEosFiles ftpUtil.Upload=>" + file.Name + "_" + ret1.ToString());
                    if (ret1 == true)
                    {
                        FileLog.PrintF("UploadEosFiles path\\BACKUP=>" + Config.GetBackUpPath());
                        FileLog.PrintF("UploadEosFiles file.FullName=>" + file.FullName);
                        FileLog.PrintF("UploadEosFiles file.Name=>" + file.Name);
                        File.Move(file.FullName, Config.GetBackUpPath() + file.Name);
                    }
                }
                /*
                bool ret2 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/data", checkFile.FullName);
                FileLog.PrintF("UploadEosFiles ftpUtil.Upload=>" + checkFile.Name + "_" + ret2.ToString());
                if (ret2 == true)
                {
                    File.Delete(checkFile.FullName);
                }
                */
            }
        }


        public static void PushFtpZipFiles()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(Config.GetPath());
            FileInfo[] fileList = directorySelected.GetFiles("*.zip");
            FtpUtil ftpUtil = new FtpUtil("192.168.0.30", Config.GetFtpUser(), Config.GetFtpPwd(), "21");
            /*FTP 서버에 있는 파일들 읽어오기
            String[] test = ftpUtil.GetFileList("/home/jijs/rubytest/mysqloader/BACKUP/");
            for (int i = 0; i < test.Length; i++)
            {
                Console.WriteLine(test[i]);
            }
            */
            /*
            //파일 다운로드 테스트.
            String dirPath = System.IO.Path.GetDirectoryName(path);
            String tmp = dirPath + "\\OPT10081_20160208.zip";
            Console.WriteLine("dirPath =" + dirPath);
            Console.WriteLine("tmp =" + tmp);
            bool ret = ftpUtil.Download(tmp, "/home/jijs/rubytest/mysqloader/data/OPT10081_20160208.zip");
            Console.WriteLine("ftpUtil.Download ret =" + ret);
            */

            /*
            //파일 업로드 테스트.
            String dirPath1 = System.IO.Path.GetDirectoryName(path);
            String tmp1 = dirPath + "\\OPT10081_000020_20160209.log";
            Console.WriteLine("dirPath1 =" + dirPath1);
            Console.WriteLine("tmp1 =" + tmp1);
            //  String fileName= Path.GetFileName(tmp1);
            //Console.WriteLine("ftpUtil.Upload fileName =" + fileName);
            bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/data", tmp1);
            Console.WriteLine("ftpUtil.Upload ret1 =" + ret1);
            */

            foreach (FileInfo file in fileList)
            {
                bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/data", file.FullName);
                FileLog.PrintF("PushFtpZipFiles ftpUtil.Upload file.Name_ret1=" + file.Name + "_" + ret1.ToString());
            }


        }

        private static void CompressZip(string zipPath, string startsWith, string endsWith)
        {
            DirectoryInfo directorySelected = new DirectoryInfo(Config.GetPath());
            //Directory.GetFiles(path,"*.txt",SearchOption.TopDirectoryOnly).Where(s=>s.StartsWith(startsWith));
            FileInfo[] fileList = directorySelected.GetFiles(startsWith + "*" + endsWith + ".txt");


            if (fileList.Count() > 0 && AppLib.getClass1Instance().getStockCodeList().Count() <= fileList.Count())
            {
                //카운트가 맞을때만 압축하는것도 좋은 예지만 이것만으로는 TEXT파일에 끝까지데이터가 있다는걸 보장을 할수 없다.

                FileLog.PrintF("CompressZip zipPath=" + zipPath);
                FileLog.PrintF("CompressZip startsWith=" + startsWith);
                FileLog.PrintF("CompressZip endsWith=" + endsWith);
                using (FileStream zipToCreate = new FileStream(zipPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                    {
                        foreach (FileInfo fileToCompress in fileList)
                        {
                            archive.CreateEntryFromFile(fileToCompress.FullName, fileToCompress.Name);
                        }
                    }
                }

            }

        }

        private void DecompressZip(string zipPath, string extractPath, string startsWith)
        {
            //*이 메소드는 쓸일이 없긴한데..*/
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase)) //startsWith에 해당하는것만 압축이 풀리도록
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    }
                }
            }
        }
    }
}

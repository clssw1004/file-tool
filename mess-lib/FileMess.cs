using System;
using System.IO;
using System.Text;

namespace mess_lib
{
   public delegate void AfterMess(String fileName);
    public class FileMess
    {
        private const int buffer_size = 9737;
        private const int file_head = 4096;
        public static void MessDir(String dirName, MessOption option, bool delAfterMess = false, AfterMess am = null)
        {
            DirectoryInfo di = new DirectoryInfo(dirName);
            if (!di.Exists)
            {
                Console.WriteLine("dir not found");
                return;
            }
            foreach (var f in di.GetFileSystemInfos())
            {
                try
                {
                    if (f is FileInfo)
                    {
                        String name = Mess(f.FullName, option, delAfterMess);
                        if (am != null)
                            am(name);
                    }
                    else if (f is DirectoryInfo)
                    {
                        MessDir(f.FullName, option, delAfterMess);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    continue;
                }
            }
        }

        /// <summary>
        /// 打乱或恢复文件
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static String Mess(String fullName, MessOption option, bool delAfterMess = false)
        {
            String newFullName = null;
            try
            {
                newFullName = ReverseFileWithBinary(fullName, option, delAfterMess);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newFullName;
        }

        private static String ReverseFileWithBinary(String src, MessOption option, bool delAfterReverse = false)
        {
            FileStream fsReader = null;
            BinaryReader br = null;
            FileStream fsWriter = null;
            BinaryWriter bw = null;
            String destFileName = null;
            FileInfo file = new FileInfo(src);
            if (!file.Exists)
                throw new Exception("File Not Found !");
            else if (option != MessOption.MESS && option != MessOption.UNMESS)
                throw new Exception("Mess option error!!");
            try
            {
                fsReader = new FileStream(src, FileMode.Open);
                br = new BinaryReader(fsReader, Encoding.UTF8);
                if (option == MessOption.MESS)
                {
                    destFileName = String.Format(@"{0}\{1}", file.Directory, Guid.NewGuid().ToString("N"));
                    fsWriter = new FileStream(destFileName, FileMode.CreateNew);
                    bw = new BinaryWriter(fsWriter, Encoding.UTF8);
                    WriteFileHead(bw, file.Name);
                }
                else
                {
                    destFileName = String.Format(@"{0}\{1}", file.Directory, ReadFileHead(br));
                    fsWriter = new FileStream(destFileName, FileMode.CreateNew);
                    bw = new BinaryWriter(fsWriter, Encoding.UTF8);
                }
                byte[] buffer = new byte[buffer_size];
                int index = 2;
                int len = 0;
                while ((len = br.Read(buffer, 0, index++)) != 0)
                {
                    Array.Reverse(buffer,0,len);
                    bw.Write(buffer, 0, len);
                    if (index >= buffer.Length)
                        index = 2;
                }
                bw.Close();
                br.Close();
                fsReader.Close();
                fsWriter.Close();
                if (delAfterReverse)
                    file.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (null != bw)
                    bw.Close();
                if (null != br)
                    br.Close();
                if (null != fsReader)
                    fsReader.Close();
                if (null != fsWriter)
                    fsWriter.Close();
            }
            return destFileName;
        }
        /// <summary>
        /// 写入文件头（写入文件名）
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="fileName"></param>
        public static void WriteFileHead(BinaryWriter bw, String fileName)
        {
            byte[] fileHead = new byte[file_head];
            byte[] bytes = Encoding.UTF8.GetBytes(fileName);
            int len = bytes.Length;
            Array.Copy(bytes, fileHead, len);
            bw.Write(Int2Bytes(len));
            bw.Write(fileHead);
        }
        /// <summary>
        /// 读取文件名
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static String ReadFileHead(BinaryReader br)
        {
            byte[] bytes = new byte[4];
            br.Read(bytes, 0, 4);
            int len = Bytes2Int(bytes, 0);
            byte[] fileHead = new byte[file_head];
            br.Read(fileHead, 0, fileHead.Length);
            return Encoding.UTF8.GetString(fileHead, 0, len);

        }
        /**  
        * byte数组中取int数值，本方法适用于(低位在前，高位在后)的顺序，和和intToBytes（）配套使用 
        *   
        * @param src  
        *            byte数组  
        * @param offset  
        *            从数组的第offset位开始  
        * @return int数值  
        */
        public static int Bytes2Int(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        /**  
        * 将int数值转换为占四个字节的byte数组，本方法适用于(低位在前，高位在后)的顺序。 和bytesToInt（）配套使用 
        * @param value  
        *            要转换的int值 
        * @return byte数组 
        */
        public static byte[] Int2Bytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }
    }
}

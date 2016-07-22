using mess_lib;
using System;

namespace file_mess
{
    class Program
    {
        static void Main(string[] args)
        {
            //要打乱文件字节顺序的路径
            String dirName = @"";
            //恢复该路径下所有文件
            FileMess.MessDir(dirName, MessOption.UNMESS, true);
            //打乱该路径下所有文件字节顺序
            FileMess.MessDir(dirName, MessOption.MESS, true);
            Console.ReadLine();
        }





    }
}

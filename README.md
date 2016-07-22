# mess-tool
我们在上传文件到网盘时（百度某盘），有时会因为文件内容违规，不让上传，或者是可以上传，但下载时显示违规不让下载了；
本工具作用是打乱文件字节顺序，与文件名，让网盘无法识别文件，以实现正常的上传下载；
- 本代码由C#实现
- 打乱或恢复文件顺序
    ```C#
    FileMess.Mess(fileName, MessOption,delFile=false);
    ```
    fileName为文件全路径；
    MessOption有两个值：
    ```C#
    MessOption.MESS  //打乱顺序
    MessOption.UNMESS //恢复
    ```
    delFile是一个布尔值，true为处理后删除源文件，false为保留源文件

- 整个文件夹操作
    ```C#
     FileMess.MessDir(dirName, MessOption.MESS, delFile);
    ```
    dirName为文件夹全路径，后面两个参数同上






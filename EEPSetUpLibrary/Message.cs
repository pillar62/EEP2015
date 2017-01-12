using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// 消息类,用以传输文件,包含消息的类别,文件的标识号,文件内容的起始位置,文件内容的长度,及文件的内容
    /// </summary>
    public class MessageBuffer//测试OK
    {
        /// <summary>
        /// 创建一个消息类
        /// </summary>
        /// <param name="header">消息的类别</param>
        /// <param name="fileid">文件的标识号</param>
        /// <param name="fileoffset">内容的起始位置</param>
        /// <param name="filelength">内容的长度</param>
        public MessageBuffer(MessageType header, int fileid, int fileoffset, int filelength)
        {
            _Header = header;
            _FileID = fileid;
            _FileOffSet = fileoffset;
            _FileLength = filelength;
            FileBytes = new byte[FileLength];
        }

        /// <summary>
        /// 从接收到的字节数组中得到消息类
        /// </summary>
        /// <param name="btmessage">接收到的数组</param>
        public MessageBuffer(byte[] btmessage)
        {
            if (btmessage.Length < 13)
            {
                _Completed = false;    //等下一个数据包接上
            }
            else
            {
                _Header = (MessageType)Buffer.GetByte(btmessage, 0);
                _FileID = BitConverter.ToInt32(btmessage, 1);
                _FileOffSet = BitConverter.ToInt32(btmessage, 5);
                _FileLength = BitConverter.ToInt32(btmessage, 9);
                FileBytes = new byte[FileLength];

                if (btmessage.Length < (_FileLength + 13))
                {
                    _Completed = false;
                    //等下一个数据包接上
                }
                else
                {
                    _Completed = true;
                    Buffer.BlockCopy(btmessage, 13, FileBytes, 0, FileLength);
                }
            }
        }

        private bool _Completed;
        /// <summary>
        /// 信息的数据是否完整,如果不完整就与下一次接收到的数据合并
        /// </summary>
        public bool Completed
        {
            get { return _Completed; }
        }


        /// <summary>
        /// 服务端发送完成的消息
        /// </summary>
        public static MessageBuffer SendFinished = new MessageBuffer(MessageType.SendFinished, 0, 0, 0);

        /// <summary>
        /// 客户端接收完成的消息
        /// </summary>
        public static MessageBuffer ReceiveFinished = new MessageBuffer(MessageType.ReceiveFinished, 0, 0, 0);

        public static MessageBuffer RequerySolution = new MessageBuffer(MessageType.RequerySolution, 0, 0, 0);

        private MessageType _Header;
        /// <summary>
        /// 获取消息的类别
        /// </summary>
        public MessageType Header
        {
            get { return _Header; }
        }

        private int _FileID;
        /// <summary>
        /// 文件的标识号
        /// </summary>
        public int FileID
        {
            get { return _FileID; }
        }

        private int _FileOffSet;
        /// <summary>
        /// 文件内容的起始位置
        /// </summary>
        public int FileOffSet
        {
            get { return _FileOffSet; }
        }

        private int _FileLength;
        /// <summary>
        /// 文件内容的长度
        /// </summary>
        public int FileLength
        {
            get { return _FileLength; }
        }

        private byte[] FileBytes = null;

        /// <summary>
        /// 按照起始位置及长度从流中读出内容
        /// </summary>
        /// <param name="filestream">要读出的文件流</param>
        public void ReadFileBytes(Stream filestream)
        {
            filestream.Position = FileOffSet;
            _FileLength = filestream.Read(FileBytes, 0, FileLength);
        }

        /// <summary>
        /// 返回消息类中的文件内容部分
        /// </summary>
        /// <returns>文件内容部分</returns>
        public byte[] GetFileBytes()//use for xml
        {
            return FileBytes;
        }

        /// <summary>
        /// 按照起始位置及长度将内容写回到文件流中
        /// </summary>
        /// <param name="filestream">要写入的文件流</param>
        public void WriteFileBytes(Stream filestream)
        {
            filestream.Position = FileOffSet;
            filestream.Write(FileBytes, 0, FileLength);
        }

        /// <summary>
        /// 设定消息类中的文件内容部分
        /// </summary>
        /// <param name="btfile">新的文件内容</param>
        public void SetFileBytes(byte[] btfile)//use for xml
        {
            FileBytes = btfile;
        }

        /// <summary>
        /// 获取整个消息的字节数组表达方式
        /// </summary>
        /// <returns>存放消息的数组</returns>
        public byte[] GetBytes()
        {
            byte[] btmessage = new byte[13 + FileLength];
            Buffer.SetByte(btmessage, 0, (byte)Header);                             //插入信息类型
            Buffer.BlockCopy(BitConverter.GetBytes(FileID), 0, btmessage, 1, 4);    //插入文件ID
            Buffer.BlockCopy(BitConverter.GetBytes(FileOffSet), 0, btmessage, 5, 4);//插入文件偏移
            Buffer.BlockCopy(BitConverter.GetBytes(FileLength), 0, btmessage, 9, 4);//插入文件长度
            if (FileLength > 0)
            {
                Buffer.BlockCopy(FileBytes, 0, btmessage, 13, FileLength);//文件内容
            }
            return btmessage;
        }
    }

    /// <summary>
    /// 消息的类别
    /// </summary>
    public enum MessageType
    { 


       /// <summary>
        /// 客户端请求文件
        /// </summary>
        RequeryFile = 0x01,//C->S
        /// <summary>
        /// 服务端返回更新列表
        /// </summary>
        SendFileList = 0x02,//S->C
        /// <summary>
        /// 服务端返回更新文件的内容
        /// </summary>
        SendFile = 0x03,//S->C
        /// <summary>
        /// 服务端发送完成
        /// </summary>
        SendFinished = 0x04,//S->C
        //UserCancel//C->S ioexception
        /// <summary>
        /// 客户端请求部分文件
        /// </summary>
        RequeryBlock = 0x05,//C->S
        /// <summary>
        /// 服务端响应请求错误
        /// </summary>
        RequeryBlockError = 0x06,
        /// <summary>
        /// 客户端接收完成
        /// </summary>
        ReceiveFinished =0x07,//C->S

        RequerySolution = 0x08,

        SendSolutionList = 0x09
    }
}

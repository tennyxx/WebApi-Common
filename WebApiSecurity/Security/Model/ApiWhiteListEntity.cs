using System;
using System.Runtime.Serialization;

namespace WebApi.Common.Security
{
    /// <summary>
    ///ApiWhiteList数据实体
    /// </summary>
    [Serializable]
    [DataContract]
    public class ApiWhiteListEntity
    {
        #region 变量定义
        ///<summary>
        ///白名单主键ID
        ///</summary>
        private int _whiteID;
        ///<summary>
        ///授权的Key
        ///</summary>
        private string _appKey = String.Empty;
        ///<summary>
        ///IP地址开始
        ///</summary>
        private long _ipAddrStart;
        ///<summary>
        ///IP地址结束
        ///</summary>
        private long _ipAddrEnd;
        ///<summary>
        ///备注
        ///</summary>
        private string _remark = String.Empty;
        ///<summary>
        ///创建时间
        ///</summary>
        private DateTime _createTime;
        ///<summary>
        ///是否有效
        ///</summary>
        private short _isActive;
        #endregion

        #region 构造函数

        ///<summary>
        ///允许访问接口的IP地址
        ///</summary>
        public ApiWhiteListEntity()
        {
        }
        ///<summary>
        ///允许访问接口的IP地址
        ///</summary>
        public ApiWhiteListEntity
        (
            int whiteID,
            string appKey,
            long ipAddrStart,
            long ipAddrEnd,
            string remark,
            DateTime createTime,
            short isActive
        )
        {
            _whiteID = whiteID;
            _appKey = appKey;
            _ipAddrStart = ipAddrStart;
            _ipAddrEnd = ipAddrEnd;
            _remark = remark;
            _createTime = createTime;
            _isActive = isActive;

        }
        #endregion

        #region 公共属性


        ///<summary>
        ///白名单主键ID
        ///</summary>
        [DataMember]
        public int WhiteID
        {
            get { return _whiteID; }
            set { _whiteID = value; }
        }

        ///<summary>
        ///授权的Key
        ///</summary>
        [DataMember]
        public string AppKey
        {
            get { return _appKey; }
            set { _appKey = value; }
        }

        ///<summary>
        ///IP地址开始
        ///</summary>
        [DataMember]
        public long IpAddrStart
        {
            get { return _ipAddrStart; }
            set { _ipAddrStart = value; }
        }

        ///<summary>
        ///IP地址结束
        ///</summary>
        [DataMember]
        public long IpAddrEnd
        {
            get { return _ipAddrEnd; }
            set { _ipAddrEnd = value; }
        }

        ///<summary>
        ///备注
        ///</summary>
        [DataMember]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        ///<summary>
        ///创建时间
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        ///<summary>
        ///是否有效
        ///</summary>
        [DataMember]
        public short IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #endregion

    }
}

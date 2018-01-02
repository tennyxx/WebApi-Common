using System;
using System.Runtime.Serialization;
namespace WebApi.Common.Security
{
    /// <summary>
    ///ApiRsaSecret数据实体
    /// </summary>
    [Serializable]
    public class ApiRsaSecretEntity
    {
        #region 变量定义
        ///<summary>
        ///
        ///</summary>
        private int _rsaID;
        ///<summary>
        ///授权的Key
        ///</summary>
        private string _appKey = String.Empty;
        ///<summary>
        ///私钥
        ///</summary>
        private string _privateKey = String.Empty;
        ///<summary>
        ///公钥
        ///</summary>
        private string _publicKey = String.Empty;
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
        ///
        ///</summary>
        public ApiRsaSecretEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public ApiRsaSecretEntity
        (
            int rsaID,
            string appKey,
            string privateKey,
            string publicKey,
            DateTime createTime,
            short isActive
        )
        {
            _rsaID = rsaID;
            _appKey = appKey;
            _privateKey = privateKey;
            _publicKey = publicKey;
            _createTime = createTime;
            _isActive = isActive;

        }
        #endregion

        #region 公共属性


        ///<summary>
        ///
        ///</summary>
        public int RsaID
        {
            get { return _rsaID; }
            set { _rsaID = value; }
        }

        ///<summary>
        ///授权的Key
        ///</summary>
        public string AppKey
        {
            get { return _appKey; }
            set { _appKey = value; }
        }

        ///<summary>
        ///私钥
        ///</summary>
        public string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        ///<summary>
        ///公钥
        ///</summary>
        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        ///<summary>
        ///是否有效
        ///</summary>
        public short IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #endregion

    }
}

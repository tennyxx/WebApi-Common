
using System;
using System.Runtime.Serialization;
namespace WebApi.Common.Security
{
    /// <summary>
    ///ApiAuthorize数据实体
    /// </summary>
    [Serializable]
    public class ApiAuthorizeEntity
    {
        #region 变量定义
        ///<summary>
        ///授权ID
        ///</summary>
        private int _authorizeID;
        ///<summary>
        ///授权的Key
        ///</summary>
        private string _appKey = String.Empty;
        ///<summary>
        ///1、MD5，2、RSA，3、SHA1
        ///</summary>
        private short _algorithm;
        ///<summary>
        ///授权签名码
        ///</summary>
        private string _signature = String.Empty;
        ///<summary>
        ///0、不验证，1、验证
        ///</summary>
        private short _checkIPAddress;
        ///<summary>
        ///授权的项目
        ///</summary>
        private string _projectName = String.Empty;
        ///<summary>
        ///项目负责人
        ///</summary>
        private string _projectPerson = String.Empty;
        ///<summary>
        ///联系电话
        ///</summary>
        private string _mobile = String.Empty;
        ///<summary>
        ///联系邮箱
        ///</summary>
        private string _email = String.Empty;
        ///<summary>
        ///项目所在地址
        ///</summary>
        private string _apiProjectPath = String.Empty;
        ///<summary>
        ///API的研发负责人
        ///</summary>
        private string _devPerson = String.Empty;
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
        public ApiAuthorizeEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public ApiAuthorizeEntity
        (
            int authorizeID,
            string appKey,
            short algorithm,
            string signature,
            short checkIPAddress,
            string projectName,
            string projectPerson,
            string mobile,
            string email,
            string apiProjectPath,
            string devPerson,
            DateTime createTime,
            short isActive
        )
        {
            _authorizeID = authorizeID;
            _appKey = appKey;
            _algorithm = algorithm;
            _signature = signature;
            _checkIPAddress = checkIPAddress;
            _projectName = projectName;
            _projectPerson = projectPerson;
            _mobile = mobile;
            _email = email;
            _apiProjectPath = apiProjectPath;
            _devPerson = devPerson;
            _createTime = createTime;
            _isActive = isActive;

        }
        #endregion

        #region 公共属性


        ///<summary>
        ///授权ID
        ///</summary>
        public int AuthorizeID
        {
            get { return _authorizeID; }
            set { _authorizeID = value; }
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
        ///1、MD5，2、RSA，3、SHA1
        ///</summary>
        public short Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }

        ///<summary>
        ///授权签名码
        ///</summary>
        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        ///<summary>
        ///0、不验证，1、验证
        ///</summary>
        public short CheckIPAddress
        {
            get { return _checkIPAddress; }
            set { _checkIPAddress = value; }
        }

        ///<summary>
        ///授权的项目
        ///</summary>
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        ///<summary>
        ///项目负责人
        ///</summary>
        public string ProjectPerson
        {
            get { return _projectPerson; }
            set { _projectPerson = value; }
        }

        ///<summary>
        ///联系电话
        ///</summary>
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        ///<summary>
        ///联系邮箱
        ///</summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        ///<summary>
        ///项目所在地址
        ///</summary>
        public string ApiProjectPath
        {
            get { return _apiProjectPath; }
            set { _apiProjectPath = value; }
        }

        ///<summary>
        ///API的研发负责人
        ///</summary>
        public string DevPerson
        {
            get { return _devPerson; }
            set { _devPerson = value; }
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

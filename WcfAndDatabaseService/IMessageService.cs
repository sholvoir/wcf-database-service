using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfAndDatabaseService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWriteMessageToDatabaseCallback))]
    public interface IMessageService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 在此添加您的服务操作
        [OperationContract(IsInitiating =true, IsTerminating = false)]
        void WriteMessageToDatabase(string logMsg);

        [OperationContract(IsInitiating =true, IsTerminating = false)]
        void RegisterListener();

        [OperationContract(IsInitiating =false, IsTerminating = false)]
        void UnregisterListener();
    }

    [ServiceContract]
    public interface IWriteMessageToDatabaseCallback 
    {
        [OperationContract(IsOneWay = true)]
        void OnWriteMessageToDatabase(string logMsg);  
    } 

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}

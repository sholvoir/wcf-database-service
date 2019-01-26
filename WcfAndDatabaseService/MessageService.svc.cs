using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Diagnostics;

namespace WcfAndDatabaseService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode =ConcurrencyMode.Multiple)]
    public class MessageService : IMessageService
    {
        public MessageService() 
        { 
            Trace.WriteLine("Create LogService Instance."); 
        }

        Dictionary<string, OperationContext> listeners = new Dictionary<string, OperationContext>();

        private void BroadCast(string logMsg)
        {
            List<string> errorClints = new List<string>(); 
            foreach (KeyValuePair<string, OperationContext>listener in listeners)
            {
                try
                {
                    listener.Value.GetCallbackChannel<IWriteMessageToDatabaseCallback>().OnWriteMessageToDatabase(logMsg);
                }
                catch (System.Exception e) 
                { 
                    errorClints.Add(listener.Key); 
                    Trace.WriteLine("BROAD EVENT ERROR:" + e.Message); 
                }
            } 
            foreach (string id in errorClints) 
            { 
                listeners.Remove(id); 
            }
        }

        #region IMessageService 成员
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void WriteMessageToDatabase(string logMsg)  
        {  
            Trace.WriteLine("Write LOG:"+logMsg);
            BroadCast(logMsg);
        }

        public void RegisterListener()
        {
            listeners.Add(OperationContext.Current.SessionId, OperationContext.Current);
            Trace.WriteLine("SessionID:" + OperationContext.Current.SessionId);
            Trace.WriteLine("Register listener.Client Count:" + listeners.Count.ToString());
        }
        
        public void UnregisterListener()  
        {  
            listeners.Remove(OperationContext.Current.SessionId); 
            Trace.WriteLine("SessionID:" + OperationContext.Current.SessionId);  
            Trace.WriteLine("Unregister listener. Client Count:" + listeners.Count.ToString());  
        }  
        #endregion
    }
}

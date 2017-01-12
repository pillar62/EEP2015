using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace EFWCFModule.Silverlight
{
    public class SilverlightFaultBehavior : IErrorHandler, IServiceBehavior
    {
        #region IErrorHandler Members

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var realException = GetRealException(error);
            FaultException fe = new FaultException(realException.Message);
            MessageFault messageFault = fe.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, "http://microsoft.wcf.documentation/default");
            HttpResponseMessageProperty property = new HttpResponseMessageProperty();
            property.StatusCode = System.Net.HttpStatusCode.OK;
            fault.Properties[HttpResponseMessageProperty.Name] = property;
        }

        #endregion

        private static Exception GetRealException(Exception exception)
        {
            while (exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message))
            {
                exception = exception.InnerException;
            }
            return exception;
        }

        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)
            {
                chanDisp.ErrorHandlers.Add(this);
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            
        }

        #endregion
    }
}

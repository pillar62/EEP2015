using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace Srvtools
{

    public class RemotingDelegates : MarshalByRefObject
    {
        public static ManualResetEvent e;

        public CliUtils.CallBack fjustdo;

        public delegate object[] RemoteAsyncDelegate(object[] ClientInfo, string ModuleName, string MethodName, object[] objParams);

        // This is the call that the AsyncCallBack delegate references.
        [OneWayAttribute]
        public void OurRemoteAsyncCallBack(IAsyncResult ar)
        {
            RemoteAsyncDelegate del = (RemoteAsyncDelegate)((AsyncResult)ar).AsyncDelegate;
            object[] oRet = del.EndInvoke(ar);
            // Signal the thread.
            e.Set();

            fjustdo(oRet);
            return;
        }

        public void Run(object[] ClientInfo, string ModuleName, string MethodName, object[]objParams, CliUtils.CallBack justdo)
        {
            // Enable this and the e.WaitOne call at the bottom if you 
            // are going to make more than one asynchronous call.
            fjustdo = justdo;

            e = new ManualResetEvent(false);

            // This is the only thing you must do in a remoting scenario
            // for either synchronous or asynchronous programming 
            // configuration.

            // The remaining steps are identical to single-
            // AppDomain programming.
            /*Rich marked...
            ServiceClass obj = new ServiceClass();
             * */

            // This delegate is an asynchronous delegate. Two delegates must 
            // be created. The first is the system-defined AsyncCallback 
            // delegate, which references the method that the remote type calls 
            // back when the remote method is done.

            AsyncCallback RemoteCallback = new AsyncCallback(this.OurRemoteAsyncCallBack);

            // Create the delegate to the remote method you want to use 
            // asynchronously.
            RemoteAsyncDelegate RemoteDel = new RemoteAsyncDelegate(CliUtils.RemoteObject.CallMethod);

            // Start the method call. Note that execution on this 
            // thread continues immediately without waiting for the return of 
            // the method call. 
            IAsyncResult RemAr = RemoteDel.BeginInvoke(ClientInfo, ModuleName, MethodName, objParams, RemoteCallback, null);
            //已经调用方法了...

            // If you want to stop execution on this thread to 
            // wait for the return from this specific call, retrieve the 
            // IAsyncResult returned from the BeginIvoke call, obtain its 
            // WaitHandle, and pause the thread, such as the next line:
            // RemAr.AsyncWaitHandle.WaitOne();

            // To wait in general, if, for example, many asynchronous calls 
            // have been made and you want notification of any of them, or, 
            // like this example, because the application domain can be 
            // recycled before the callback can print the result to the 
            // console.
            //e.WaitOne();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface.Helper;
using DCRF.Interface.Primitive;
using DCRF.Interface.Collection;

namespace DCRF.Interface.Core
{
    public delegate void SiteEventDelegate(ISite sender, IComponent subject);
    public delegate void SiteActionEventDelegate(ISite sender, CID subject,IComponent comp, ref bool cancelOperation);
    public delegate void RequestCallbackDelegate(ISite sender, IComponent subject, object tag, object result);
    public delegate void PreComponentCallEventDelegate(ISite sender,IComponent subject,string serviceName, object[] args,ref object response,ref bool cancelCall);
    public delegate void PostComponentCallEventDelegate(ISite sender, IComponent subject, string serviceName, object[] args, ref object response);
    public delegate void SiteExceptionDelegate(ISite sender, Exception exc,ref bool throwException);

    /// <summary>
    /// Order of a class items:
    /// private/public events;
    /// private properties and fields;
    /// public properties and fields;
    /// private methods
    /// public methods
    /// </summary>
    public interface ISite
    {
        event SiteEventDelegate ComponentLoaded;
        event SiteActionEventDelegate AddingComponent;
        event SiteEventDelegate ComponentAdded;
        event SiteActionEventDelegate DeletingComponent;
        event SiteEventDelegate ComponentDeleted;
        event PreComponentCallEventDelegate PreComponentCall;
        event PostComponentCallEventDelegate PostComponentCall;
        event SiteExceptionDelegate ExceptionOccured;
        
        PropertyCollection GlobalProperties
        {
            get;
        }

        EventCollection GlobalEvents
        {
            get;
        }

        IComponentBroker ComponentBroker
        {
            get;
        }

        CID[] ComponentIDs
        {
            get;
        }

        int Count
        {
            get;
        }


        /// <summary>
        /// This should be set using constructor.
        /// Shows parent site, used to make site hierarchy
        /// </summary>
        ISite Parent
        {
            get;
        }

        PlatformType Platform
        {
            get;
        }

        CID AddComponent(CID handle,params object[] constructorArgs);
        void DeleteComponent(CID handle); 

        /// <summary>
        /// Synchronous component call
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="serviceName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object ProcessComponentRequest(CID handle,string serviceName, params object[] args);

        /// <summary>
        /// This method calls component service asynchronously and when done, calls callBack delegate to inform caller
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="serviceName"></param>
        /// <param name="callBack"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object ProcessComponentRequest(CID handle, string serviceName, RequestCallbackDelegate callBack, object tag, params object[] args);

        /// <summary>
        /// This is used to call a component which is found using key.
        /// If we have no component with this key then we need to load it 
        /// If key is new, CID is used to load the component
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="serviceName"></param>
        /// <param name="callBack"></param>
        /// <param name="tag"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object ProcessComponentRequest(string key, CID handle, string serviceName, params object[] args);

        /// <summary>
        /// As we do not like to pass IComponent object to requesters, we have a wrapper for IComponent methods
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        PropertyCollection GetComponentProperties(CID handle);
        EventCollection GetComponentEvents(CID handle);
        ComponentInfo GetComponentInfo(CID handle);

        /// <summary>
        /// used by siteManager to manipulate lastOperationTime of components
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        SitedComponentInfo GetSitedComponentInfo(IComponent comp);
        SitedComponentInfo GetSitedComponentInfo(CID comp);
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DCRF.Interface.Primitive;
using DCRF.Interface.Collection;

namespace DCRF.Interface.Core
{
    public delegate void ComponentEventHandler(params object[] args);


	/// <summary>
	/// describes the common behavior of a component
    /// Components must support multiple calls to dispose method. This is because the failover mechanism in component brokers
    /// may call dispose method multiple times
	/// </summary>
	public interface IComponent: IDisposable
	{
        /// <summary>
        /// information about component some of which is used by DCRF and some are just for information
        /// </summary>
        ComponentInfo ComponentInfo
        {
            get;
        }        

        /// <summary>
        /// List of services which this component supports. this list only includes the service name
        /// which will be used in a call to processRequest method
        /// If component is inherited from ComponentBase this will be names of the mehtods which imlement
        /// component services (have ComponentService attribute)
        /// </summary>
        string[] Services
        {
            get;
        }

        /// <summary>
        /// points to the site in which the component is hosted
        /// </summary>
        ISite Site
        {
            get;
            set;
        }

        /// <summary>
        /// a list of properties of this component. They can be configuration data or commonly used
        /// information or anything else
        /// </summary>
        PropertyCollection Properties
        {
            get;
        }

        /// <summary>
        /// collection of events supported by this component
        /// </summary>
        EventCollection Events
        {
            get;
        }

        /// <summary>
        /// Type of this component
        /// </summary>
        ComponentType Type 
        {
            get;
        }

        PlatformType Platform
        {
            get;
        }


        /// <summary>
        /// A list of components which this component calls and needs their service for its operation
        /// This is not currently used explicitly
        /// </summary>
        CID[] ReferencedComponents
        {
            get;
        }

        /// <summary>
        /// Performs a service of the component and returns the result (if any)
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        /// <param name="args">Parameters of the service</param>
        /// <returns></returns>
        object ProcessRequest(string serviceName, params object[] args);

        /// <summary>
        /// this method is called after componen is being sited.
        /// Initializes the supported properties of this component - this is seperated from initcomponent
        /// for more readability of the code
        /// </summary>
        void InitProperties();

        /// <summary>
        /// this method is called after calling initProperties
        /// Initializes the supported events of this component - this is seperated from initcomponent
        /// for more readability of the code
        /// </summary>
        void InitEvents();

        /// <summary>
        /// Initializes component - this method is called after calling initProperties and initEvents methods
        /// </summary>
        void InitComponent();

        /// <summary>
        /// This methods is called only once so its able to simulate a constructor
        /// The user can pass a series of arguments to this method
        /// This method can be used for resource allocation for example opening a stream in a logger component
        /// We need this to be performed only once.
        /// This method is called when addin the component to the site. It's called when component is loaded and
        /// all init methods and events (InitEvents, InitProperties, ...) are called
        /// </summary>
        /// <param name="args"></param>
        void StartupConstructor(params object[] args);
    }

}

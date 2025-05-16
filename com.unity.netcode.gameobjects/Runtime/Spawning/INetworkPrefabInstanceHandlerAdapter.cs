using UnityEngine;

namespace Unity.Netcode
{

    //This interface is used to allow the PrefabHandler to use the INetworkPrefabInstanceHandlerSource, so in this way we avoid having to add a different implementation
    // of AddHandler for each of the posible INetworkPrefabInstanceHandlerSource implementations
    // POSIBLY MAKE THIS PUBLIC, SO USERS CAN CREATE THEIR OWN ADAPTERS, READ THE NOTE IN THE OTHER INTERFACE
    internal interface INetworkPrefabInstanceHandlerAdapter
    {
        NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation, FastBufferReader instantiationDataReader = default);
        bool HandlesDataOfType<T>();
        void Destroy(NetworkObject networkObject);
    }

    public interface INetworkPrefabInstanceHandlerSource
    {
        //This interface requiring an implementation of CreateHandlerAdapter is also a way to disallow users to use INetworkPrefabInstanceHandlerSource
        // Outside the assembly, as it is internal and will give compilation errors.
        // BUT!!
        // I think we can make this also public along with the Adapter Interface, in that way users could create their own wrappers giving even more flexibility
        // By default, the current internal interfaces uses a default implementation of CreateHandlerAdapterm so users dont need to add this
        internal INetworkPrefabInstanceHandlerAdapter CreateHandlerAdapter();
    }
}
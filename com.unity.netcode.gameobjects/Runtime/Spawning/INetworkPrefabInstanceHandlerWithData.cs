using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Unity.Netcode
{
    /// <summary>
    /// Specialized version of <see cref="INetworkPrefabInstanceHandler"/> that receives
    /// custom instantiation data injected by the server before spawning.
    /// </summary>
    public interface INetworkPrefabInstanceHandlerWithData<T> : INetworkPrefabInstanceHandlerSource where T : struct, INetworkSerializable
    {
        NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation, T instantiationData);
        void Destroy(NetworkObject networkObject);
        INetworkPrefabInstanceHandlerAdapter INetworkPrefabInstanceHandlerSource.CreateHandlerAdapter()
        {
            return new PrefabInstanceHandlerWithDataAdapter<T>(this);
        }
    }

    internal class PrefabInstanceHandlerWithDataAdapter<T> : INetworkPrefabInstanceHandlerAdapter where T : struct, INetworkSerializable
    {
        private readonly INetworkPrefabInstanceHandlerWithData<T> _impl;
        
        public PrefabInstanceHandlerWithDataAdapter(INetworkPrefabInstanceHandlerWithData<T> impl) => _impl = impl;

        public bool HandlesDataOfType<U>() => typeof(T) == typeof(U);

        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation, FastBufferReader reader = default)
        {
            if(!reader.IsInitialized)
                return _impl.Instantiate(ownerClientId, position, rotation, default);

            reader.ReadValueSafe(out T _payload);
            return _impl.Instantiate(ownerClientId, position, rotation, _payload);
        }
        public void Destroy(NetworkObject networkObject) => _impl.Destroy(networkObject);
    }
}
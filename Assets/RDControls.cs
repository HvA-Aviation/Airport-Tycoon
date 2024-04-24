//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/RDControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @RDControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @RDControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RDControls"",
    ""maps"": [
        {
            ""name"": ""R&DControlls"",
            ""id"": ""437a06d1-ea47-4412-97fc-88e5ce50d4be"",
            ""actions"": [
                {
                    ""name"": ""ShowTree"",
                    ""type"": ""Button"",
                    ""id"": ""ade0d289-6427-44ec-9224-f3bfe9797f10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""83726313-6bc4-48a6-9b2b-dd344f795667"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowTree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // R&DControlls
        m_RDControlls = asset.FindActionMap("R&DControlls", throwIfNotFound: true);
        m_RDControlls_ShowTree = m_RDControlls.FindAction("ShowTree", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // R&DControlls
    private readonly InputActionMap m_RDControlls;
    private List<IRDControllsActions> m_RDControllsActionsCallbackInterfaces = new List<IRDControllsActions>();
    private readonly InputAction m_RDControlls_ShowTree;
    public struct RDControllsActions
    {
        private @RDControls m_Wrapper;
        public RDControllsActions(@RDControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ShowTree => m_Wrapper.m_RDControlls_ShowTree;
        public InputActionMap Get() { return m_Wrapper.m_RDControlls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RDControllsActions set) { return set.Get(); }
        public void AddCallbacks(IRDControllsActions instance)
        {
            if (instance == null || m_Wrapper.m_RDControllsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RDControllsActionsCallbackInterfaces.Add(instance);
            @ShowTree.started += instance.OnShowTree;
            @ShowTree.performed += instance.OnShowTree;
            @ShowTree.canceled += instance.OnShowTree;
        }

        private void UnregisterCallbacks(IRDControllsActions instance)
        {
            @ShowTree.started -= instance.OnShowTree;
            @ShowTree.performed -= instance.OnShowTree;
            @ShowTree.canceled -= instance.OnShowTree;
        }

        public void RemoveCallbacks(IRDControllsActions instance)
        {
            if (m_Wrapper.m_RDControllsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRDControllsActions instance)
        {
            foreach (var item in m_Wrapper.m_RDControllsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RDControllsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RDControllsActions @RDControlls => new RDControllsActions(this);
    public interface IRDControllsActions
    {
        void OnShowTree(InputAction.CallbackContext context);
    }
}

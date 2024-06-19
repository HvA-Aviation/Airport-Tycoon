using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    public Action<float> SetMultiplierSecurity;
    public Action<float> SetMultiplierGeneralWorker;
    public Action<float> SetMultiplierBuilder;

    /// <summary>
    /// Call this function when an upgrade for the security workload is done
    /// </summary>
    /// <param name="multiplier">The multiplier that the workload will be multiplied</param>
    public void SetSecurityWorkLoad(float multiplier) => SetMultiplierSecurity?.Invoke(multiplier);

    /// <summary>
    /// Call this function when an upgrade for the General staff workload is done
    /// </summary>
    /// <param name="multiplier">The multiplier that the workload will be multiplied</param>
    public void SetGeneralStaffWorkLoad(float multiplier) => SetMultiplierGeneralWorker?.Invoke(multiplier);

    /// <summary>
    /// Call this function when an upgrade for the Builder workload is done
    /// </summary>
    /// <param name="multiplier">The multiplier that the workload will be multiplied</param>
    public void SetBuilderWorkLoad(float multiplier) => SetMultiplierBuilder?.Invoke(multiplier);
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionUpdater
{

    private static MonoBehaviourHookFunctionUpdater monoGameObject;

    /// <summary>
    ///     Creates a timer that runs an action a certain amount of times with the given interval.
    /// </summary>
    /// <param name="action"> Action that will be performed </param>
    /// <param name="timer"> Amount of time before the action will be performed </param>
    /// <param name="amountOfRepeats"> Amount of times the loop will repeat </param>
    /// <returns> Returns the timer instance </returns>
    public static FunctionUpdater Create(Func<bool> func)
    {
        InitIfNeeded();

        FunctionUpdater functionTimer = new FunctionUpdater(func);
        monoGameObject.AddTimer(functionTimer);

        return functionTimer;
    }



    /// <summary>
    ///     Object made to make use of the mono behaviour
    /// </summary>
    private class MonoBehaviourHookFunctionUpdater : MonoBehaviour
    {
        private List<FunctionUpdater> activeTimerList;
        private List<FunctionUpdater> pausedTimerList;

        public MonoBehaviourHookFunctionUpdater()
        {
            activeTimerList = new List<FunctionUpdater>();
            pausedTimerList = new List<FunctionUpdater>();
        }

        private void Update()
        {
            for (var i = activeTimerList.Count - 1; i >= 0; i--)
                activeTimerList[i].Update();
        }

        /// <summary>
        ///     Adds a timer to the 'active' list
        /// </summary>
        /// <param name="timer"> Timer to add </param>
        public void AddTimer(FunctionUpdater timer)
            => activeTimerList.Add(timer);

        /// <summary>
        ///     Removes a timer from the 'active' list
        /// </summary>
        /// <param name="timer"> timer to remove </param>
        public void RemoveTimer(FunctionUpdater timer)
            => activeTimerList.Remove(timer);

        /// <summary>
        ///     Pauses a timer from the 'active' list
        /// </summary>
        /// <param name="timer"> Timer to pause </param>
        public void PauseTimer(FunctionUpdater timer)
        {
            if (activeTimerList.Contains(timer))
            {
                activeTimerList.Remove(timer);
                pausedTimerList.Add(timer);
            }
        }

        /// <summary>
        ///     Moves a timer from the 'paused' list to the 'active' list
        /// </summary>
        /// <param name="timer"> Timer to unpause </param>
        public void UnpauseTimer(FunctionUpdater timer)
        {
            if (pausedTimerList.Contains(timer))
            {
                pausedTimerList.Remove(timer);
                activeTimerList.Add(timer);
            }
        }
    }

    /// <summary>
    ///     Checks if the MonoBehaviourHook exists and instantiates it if needed.
    /// </summary>
    private static void InitIfNeeded()
    {
        if (monoGameObject == null)
        {
            GameObject gameObject = new GameObject("FunctionUpdaterObject");
            monoGameObject = gameObject.AddComponent<MonoBehaviourHookFunctionUpdater>();
        }
    }






    private Func<bool> func;
    private bool isPaused;

    private FunctionUpdater(Func<bool> func)
    {
        this.func = func;
    }

    public void Update()
    {
        if (isPaused) return;
        if(func()) Destroy();
    }

    /// <summary>
    ///     Destroys the timer
    /// </summary>
    public void Destroy() => monoGameObject.RemoveTimer(this);
    /// <summary>
    ///     Pauses the timer
    /// </summary>
    public void Pause() => isPaused = true;
    /// <summary>
    ///     Unpauses the timer
    /// </summary>
    public void Unpause() => isPaused = false;
}



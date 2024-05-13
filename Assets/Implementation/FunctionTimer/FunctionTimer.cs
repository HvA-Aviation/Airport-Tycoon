using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementation.FunctionTimer
{
    public class FunctionTimer
    {

        private static MonoBehaviourHookFunctionTimer monoGameObject;

        /// <summary>
        ///     Creates a timer that runs an action once after a set amount of time or runs an unlimited amount of times with the given interval.
        /// </summary>
        /// <param name="action"> Action that will be performed </param>
        /// <param name="timer"> Amount of time before the action will be performed </param>
        /// <param name="infiniteRepeat"> Whether or not the function should run an unlimited amount of times </param>
        /// <returns> Returns the timer instance </returns>
        public static FunctionTimer Create(Action action, float timer, bool infiniteRepeat = false)
        {
            if (infiniteRepeat) return Create(action, timer, -1);
            return Create(action, timer, 0);
        }

        /// <summary>
        ///     Creates a timer that runs an action a certain amount of times with the given interval.
        /// </summary>
        /// <param name="action"> Action that will be performed </param>
        /// <param name="timer"> Amount of time before the action will be performed </param>
        /// <param name="amountOfRepeats"> Amount of times the loop will repeat </param>
        /// <returns> Returns the timer instance </returns>
        public static FunctionTimer Create(Action action, float timer, float amountOfRepeats)
        {
            InitIfNeeded();

            FunctionTimer functionTimer = new FunctionTimer(action, timer, amountOfRepeats);
            monoGameObject.AddTimer(functionTimer);

            return functionTimer;
        }



        /// <summary>
        ///     Object made to make use of the mono behaviour
        /// </summary>
        private class MonoBehaviourHookFunctionTimer : MonoBehaviour
        {
            private List<FunctionTimer> activeTimerList;

            public MonoBehaviourHookFunctionTimer()
            {
                activeTimerList = new List<FunctionTimer>();  
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
            public void AddTimer(FunctionTimer timer) 
                => activeTimerList.Add(timer);

            /// <summary>
            ///     Removes a timer from the 'active' list
            /// </summary>
            /// <param name="timer"> timer to remove </param>
            public void RemoveTimer(FunctionTimer timer) 
                => activeTimerList.Remove(timer);
        }

        /// <summary>
        ///     Checks if the MonoBehaviourHook exists and instantiates it if needed.
        /// </summary>
        private static void InitIfNeeded()
        {
            if (monoGameObject == null)
            {
                GameObject gameObject = new GameObject("FunctionTimerObject");
                monoGameObject = gameObject.AddComponent<MonoBehaviourHookFunctionTimer>();
            }
        }






        private Action action;
        private float timer, timerMax;
        private float amountOfRepeats;
        private bool isPaused;

        private FunctionTimer(Action action, float timer, float amountOfRepeats)
        {
            this.action = action;
            this.timerMax = timer;
            this.timer = timer;
            this.amountOfRepeats = amountOfRepeats;
        }

        public void Update()
        {
            if (isPaused) return;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                timer += timerMax;
                if (amountOfRepeats > 0) amountOfRepeats--;
                else if (amountOfRepeats < 0) return;
                else Destroy();
            }
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
}

using System;
using UnityEngine;

namespace CSoft
{
    public class AwaitExecution<T> : CustomYieldInstruction
    {
        public override bool keepWaiting => true;

        public AwaitExecution(Func<T> coroutine)
        {
            
        }
    }
}

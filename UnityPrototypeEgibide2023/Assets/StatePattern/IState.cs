using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}

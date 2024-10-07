using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Active,
        Carrying,
        TryingToCarry
    }

    public State currentState;

    public void SetState(State newState)
    {
        currentState = newState;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // 根据状态更改角色的外观
        switch (currentState)
        {
            case State.Idle:
                // 更改为空闲状态视觉
                break;
            case State.Active:
                // 更改为选中状态视觉
                break;
            case State.Carrying:
                // 更改为携带状态视觉
                break;
            case State.TryingToCarry:
                // 更改为尝试携带状态视觉
                break;
        }
    }
}
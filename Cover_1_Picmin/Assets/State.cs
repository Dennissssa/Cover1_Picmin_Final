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
        // ����״̬���Ľ�ɫ�����
        switch (currentState)
        {
            case State.Idle:
                // ����Ϊ����״̬�Ӿ�
                break;
            case State.Active:
                // ����Ϊѡ��״̬�Ӿ�
                break;
            case State.Carrying:
                // ����ΪЯ��״̬�Ӿ�
                break;
            case State.TryingToCarry:
                // ����Ϊ����Я��״̬�Ӿ�
                break;
        }
    }
}
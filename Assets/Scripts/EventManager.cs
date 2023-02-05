using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager main;

    private void Awake()
    {
        main = this;
    }

    public delegate void GlobalDelegate();
    public event GlobalDelegate onGameOver;
    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public delegate void TurnEventDelegate(int turn);
    public event TurnEventDelegate onTurnStart;
    public void StartTurn(int turn)
    {
        onTurnStart?.Invoke(turn);
    }

    public delegate void SelectCellDelegate(HexCell cell, bool on);
    public event SelectCellDelegate onCellSelect;
    public void SelectCell(HexCell cell, bool on)
    {
        onCellSelect?.Invoke(cell, on);
    }
}

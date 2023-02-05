using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NextTurnButton : MonoBehaviour
{
    public TMP_Text buttonText, stacktext;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        bool isOmenTurn = GameManager.main.Card? GameManager.main.Card.type == Card.Type.Omen:false;
        button.interactable = GameManager.playerAgency && HexCell.Omens.Count >= GameManager.main.OmenCount;
        buttonText.text = GameManager.playerAgency ? (isOmenTurn ? "Tithe (" + HexCell.Omens.Count + "/" + GameManager.main.OmenCount + ")" : "Skip") : "...";
        stacktext.text = Mathf.Max(0, GameManager.main.stack.Count - 2).ToString();
    }

    public void Press()
    {
        GameManager.main.NextTurn();
    }
}

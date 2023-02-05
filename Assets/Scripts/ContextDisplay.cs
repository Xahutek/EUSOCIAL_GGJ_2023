using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextDisplay : MonoBehaviour
{
    public static ContextDisplay main;
    public GameObject
        Display,
        speechMenu,
        TermitnatorPortrait, QueenPortrait;
    public OrderMenu orderMenu;
    public TMP_Text speaker, speechText;

    private void Awake()
    {
        main = this;
    }

    public IEnumerator ExecuteSpeech(Speaker identity, string speech)
    {
        OpenSpeech(identity, speech);

        while (Display.activeSelf)
        {
            yield return null;
        }
    }
    public void OpenSpeech(Speaker identity, string speech)
    {
        Display.SetActive(true);
        orderMenu.Close(false);
        speechMenu.SetActive(true);

        speaker.text= identity.ToString();
        TermitnatorPortrait.SetActive(identity == Speaker.Termitnator);
        QueenPortrait.SetActive(identity == Speaker.TermiteQueen);
        speechText.text = speech;
    }
    public void OpenOrder(HexCell cell, HexCell selected)
    {
        speaker.text = "ROYAL DECREE";

        TermitnatorPortrait.SetActive(false);
        QueenPortrait.SetActive(true);

        Display.SetActive(true);
        orderMenu.Open(cell, selected);
        speechMenu.SetActive(false);
    }
    public void Close()
    {
        Display.SetActive(false);
        orderMenu.Close(false);
        speechMenu.SetActive(false);
    }
}

public enum Speaker
{
    TermiteQueen, Termitnator
}
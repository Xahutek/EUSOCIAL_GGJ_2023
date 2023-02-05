using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public int turn;
    public static bool playerAgency, gameOver;

    public int OmenCount;
    public Card OmenCard;
    public List<Card> 
        commonDeck= new List<Card>(),
        rareDeck= new List<Card>(),
        legendaryDeck= new List<Card>(),
        stack = new List<Card>();
    public Card Card
    {
        get
        {
            return stack.Count ==0 ? null : stack[0];
        }
    }
    public Card NextCard
    {
        get
        {
            return stack.Count < 2 ? null : stack[1];
        }
    }

    [Range(0, 1)] public float
        rareProbability,
        LegendaryProbability;

    public GameObject GameOverMenu;

    private void Awake()
    {
        main = this;

        turn = 0;
        OmenCount = 1;
        playerAgency = true;
        gameOver = false;

        HexCell.Selected = null;
        HexCell.Omens = new List<HexCell>();
        GameOverMenu.SetActive(false);

        stack = new List<Card>();
        stack.Add(commonDeck[0]);
        AddToStack(4);
    }
    private void Start()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        if(playerAgency&&!gameOver)
        StartCoroutine(ExecuteNextTurn());
    }
    IEnumerator ExecuteNextTurn()
    {
        playerAgency = false;

        turn++;

        if (turn == 1)
        {
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "Make pace my children! The faster we expand our colony, the sooner this Savannah will be mine!"));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "Every day, I give birth to new servants that differ in range."));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "First, I must pick a mount from which to expand. Options for expansions will then be indicated."));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "But I must take heed, as the path to some might be blocked by rocks, structures or previous paths."));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "Then I must give a production order to my many children."));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "They will do my bidding anyhow, but I will only breed new ones every day if their needs are met."));
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.TermiteQueen, "I must increase our numbers, as the day no eggs will be layed, <i>he</i> will return..."));
        }

        if (Card && Card.type == Card.Type.Omen)
        {
            HexCell[] omens = HexCell.Omens.ToArray();
            foreach (HexCell cell in omens)
            {
                cell.Destroy();
                cell.SetBiome(Biome.Rock);
                cell.Refresh();
            }
            AudioEffectManager.main.TriggerOmen();

            if (gameOver) yield break;
            OmenCount *= 2;
            AddToStack(OmenCount >= 4 ? (OmenCount >= 16 ? 3 : 4) : 5);
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.Termitnator,GetOmenText(false)));
        }
        if (stack.Count != 0) stack.RemoveAt(0);

        EventManager.main.StartTurn(turn);

        yield return new WaitForSeconds(0.5f);

        if (Card == null)
        {
            AudioEffectManager.main.Termitnator();
            yield return StartCoroutine(ContextDisplay.main.ExecuteSpeech(Speaker.Termitnator, GetOmenText(true)));
            stack.Add(OmenCard);
        }

        Debug.Log("-----{ TURN "+turn+" }-----");

        playerAgency = true;
    }

    public void AddToStack(int times=1)
    {
        for (int i = 0; i < times; i++)
        {
            Card card = null;
            float roll = Random.Range(0f, 1f);
            Debug.Log(roll);

            if (legendaryDeck.Count != 0 && roll <= LegendaryProbability)
                card = legendaryDeck[Random.Range(0, legendaryDeck.Count)];
            else roll -= LegendaryProbability;

            if (card == null && rareDeck.Count != 0 && roll <= rareProbability)
                card = rareDeck[Random.Range(0, rareDeck.Count)];
            else roll -= rareProbability;

            if (card == null) card = commonDeck[Random.Range(0, commonDeck.Count)];

            stack.Add(card);
        }
    }
    public void GameOver()
    {
        playerAgency=false;
        gameOver = true;
        Debug.Log("-----{ GAME OVER! }-----");
        EventManager.main.GameOver();
        ContextDisplay.main.OpenSpeech(Speaker.Termitnator, "I am inevitable, I am death.");
        GameOverMenu.SetActive(true);
    }

    public string GetOmenText(bool start)
    {            
        string text;

        if (start)
        {
            text = "Once again, I demand the tithe.";
            switch (OmenCount)
            {
                case 1: text = "I see you have been buzy in my absence... Remember that your hybris is only surpassed by my appetite."; break;

                case 2: text = "Hello again, little queen.\n Will you honor our contract?"; break;

                case 4: text = "How many of your children must die before you will embrace me?"; break;

                case 8: text = "All your foremothers have fallen before me, what makes you think you will be any different?"; break;

                case 16: text = "Do you understand the truth of my inevitability now, little queen?"; break;

                case 32: text = "The harvest moon is rising...\n My hunger never ending."; break;

                default: break;
            }
        }
        else
        {
            text = "I am inevitable.";
            switch (OmenCount)
            {
                case 2: text = "Remember that next time,\n the tithe will be doubled..."; break;

                case 4: text = "Until next time, little queen..."; break;

                case 8: text = "Like ants to the slaughter...\n busy, busy, until the end."; break;

                case 16: text = "Get stronger.\nThink and hope, think and hope..."; break;

                case 32: text = "I am judgement.\nI am death."; break;

                case 64: text = "Defiance tastes like life itself..."; break;

                default: break;
            }
        }

        return "<i>" + text + "</i>";
    }
}

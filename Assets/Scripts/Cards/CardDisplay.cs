using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TMP_Text rangeText;
    private CanvasGroup group;

    public Card card;

    public Image cardImage,typeImage;

    public Sprite
        straight,
        corner,
        omen;

    public enum Type
    {
        None, CurrentCard, NextCard
    }
    public Type type;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        Display(null);
    }
    private void Start()
    {
        EventManager e = EventManager.main;
        e.onTurnStart += OnNextTurn;
    }

    public void FixedUpdate()
    {
        Card c = null;
        switch (type)
        {
            case Type.CurrentCard:
                c = GameManager.main.Card;
                break;
            case Type.NextCard:
                c = GameManager.main.NextCard;
                break;
            default: return;
        }
        Display(c);
    }
    public void Display(Card card)
    {
        this.card = card;

        group.alpha = card ? 1 : 0;

        if (card)
        {
            bool isOmen = card.type == Card.Type.Omen;
            rangeText.text =isOmen? GameManager.main.OmenCount.ToString() :card.range.ToString();

            Sprite typeSprite = straight;
            switch (card.type)
            {
                case Card.Type.Corner: typeSprite = corner;
                    break;
                case Card.Type.Omen:typeSprite = omen;
                    break;
                default: //Straight
                    break;
            }
            typeImage.sprite = typeSprite;
            cardImage.sprite = card.image;
            cardImage.gameObject.SetActive(card.image != null);
        }
    }

    public void OnNextTurn(int turn)
    {
  
    }
}

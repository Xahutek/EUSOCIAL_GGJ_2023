using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class HexCell : MonoBehaviour
{
    public ResourceLibrary library;
    public static HexCell Selected;
    public static List<HexCell>Omens= new List<HexCell>();
    public static bool ThroneIsOmen;
    public bool isSelected
    {
        get
        {
            return Selected == this;
        }
        set
        {
            if (value)
            {
                if (Selected != null) Selected.isSelected = false;
                Selected = this;
                EventManager.main.SelectCell(this, true);
            }
            else if (Selected == this)
            {
                Selected = null;
                EventManager.main.SelectCell(this, false);
            }
            Refresh();
        }
    }
    public bool isOmen
    {
        get
        {
            return Omens.Contains(this);
        }
        set
        {
            if (value)
            {
                if (!isOmen)
                {
                    Omens.Add(this);
                    if (Omens.Count > GameManager.main.OmenCount)
                        Omens[0].isOmen = false;
                }
            }
            else if (isOmen)
            {
                Omens.Remove(this);
            }
            if (isThrone) ThroneIsOmen = value;
            Refresh();
        }
    }
    [HideInInspector] public bool isEnvisioned;

    public bool isClaimed,isBlocked, isThrone;

    public HexDirection conA, conB;
    public int order;
    public Recipe myRecipe
    {
        get
        {
            if (!isClaimed) return null;
            if (isThrone) return library.ThroneRecipe;
            return library.GetRecipe(biome, order);
        }
    }
    public bool recipeMet(out bool in1met, out bool in2met)
    {
        in1met = false;
        in2met = false;
        Recipe recipe = myRecipe;
        if (myRecipe == null || myRecipe.name == "") return false;
        List<Resource> input = new List<Resource>();
        input.AddRange(recipe.Inputs);
        foreach (HexCell con in connectedClaimed)
        {
            Recipe ConRecipe = con.myRecipe;
            if (ConRecipe == null || ConRecipe.name == "") continue;

            foreach (Resource r in con.myRecipe.Outputs)
            {
                if (input.Contains(r))
                {
                    input.Remove(r);
                }
            }
            if (input.Count == 0)
            {
                in1met = true;
                in2met = true;
                return true;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            if (i>=recipe.Inputs.Length||!input.Contains(recipe.Inputs[i]))
            {
                if (i == 0) in1met = true;
                else in2met = true;
            }
        }
        return false;
    }
    public bool recipeWasMet;

    public List<HexCell> 
        connectedClaimed= new List<HexCell>(),
        connectedBlocked=new List<HexCell>();

    public HexCoordinates coordinates;
    public Biome biome;

    public TMP_Text text;
    [SerializeField]private SpriteRenderer mainSprite, topSprite;
    [SerializeField] private GameObject 
        SelectionBorder,EnvisionedBorder, OmenBorder,
        RecipeDisplay, ThroneRecipeDisplay;
    [SerializeField]
    private GameObject[]
        connectionMarkers;
    [SerializeField]
    private SpriteRenderer
        InputA,
        InputB,
        OutputA,
        OutputB,
        Arrow;
    public Transform RewardPop;

    Color arrowCol;

    public Tween rewardMoveTween,rewardSpinTween;

    [Header("Sprites")]
    public Sprite 
        ThroneMount;
    public Sprite
        PlainsMount,
        AquiferMount,
        ForestMount;
    private Sprite
        PlainsTop,
        AquiferTop,
        ForestTop,
        RockTop;
    public Sprite
        PlainsHex,
        AquiferHex,
        ForestHex,
        RockHex;
    public Sprite[] PlainSprites, AquiferSprites, ForestSprites, RockSprites;

    private void Awake()
    {
        arrowCol = Arrow.color;
    }
    public void SetBiome(Biome biome)
    {
        this.biome = biome;

        Sprite baseSprite;
        Sprite[] sprites = PlainSprites;
        switch (biome)
        {
            case Biome.Rock:
                sprites = RockSprites;
                baseSprite = RockHex;
                RockTop= sprites.Length == 0 ? null : sprites[Random.Range(0, sprites.Length)];
                break;
            case Biome.Forest:
                sprites = ForestSprites;
                baseSprite = ForestHex;
                ForestTop = sprites.Length == 0 ? null : sprites[Random.Range(0, sprites.Length)];
                break;
            case Biome.Aquifer:
                sprites = AquiferSprites;
                baseSprite = AquiferHex;
                AquiferTop = sprites.Length == 0 ? null : sprites[Random.Range(0, sprites.Length)];
                break;
            default: //Plains
                sprites = PlainSprites;
                baseSprite = PlainsHex;
                PlainsTop = sprites.Length == 0 ? null : sprites[Random.Range(0, sprites.Length)];
                break;
        }

        mainSprite.sprite = baseSprite;

        Refresh();
    }

    private void Start()
    {
        EventManager e = EventManager.main;
        e.onCellSelect += OnSelectCell;
        e.onTurnStart += OnNextTurn;
        e.onGameOver += Destroy;
        Refresh();
    }

    public void OnNextTurn(int turn)
    {
        isSelected = false;
        isOmen = false;
        Refresh();
    }
    public void OnSelectCell(HexCell cell, bool on)
    {
        if (on && cell != this && biome != Biome.Rock)
        {
            Card card = GameManager.main.Card;
            List<Vector3Int>
                paths, envisionedCoordinates = card.GetCoordinates(cell.coordinates.AsVector3Int(), out paths);
            isEnvisioned = envisionedCoordinates.Contains(coordinates.AsVector3Int());

            if (isEnvisioned)
            {
                int initialIndex = envisionedCoordinates.IndexOf(this.coordinates.AsVector3Int());
                for (int i = 1; i < card.range; i++)//excluding this
                {
                    HexCell c = HexMap.main.cellDict[paths[initialIndex + (i - 1) * 6]];
                    if (c.isBlocked || c.isClaimed || c.biome == Biome.Rock)
                    {
                        isEnvisioned = false;
                        break;
                    }
                }
            }
        }
        else isEnvisioned = false;

        Refresh();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSelected)
            isSelected = false;
    }

    public void Refresh()
    {
        text.text = coordinates.ToString();

        SelectionBorder.SetActive(isSelected&&!isOmen);
        EnvisionedBorder.SetActive(isEnvisioned);
        OmenBorder.SetActive(isOmen||(ThroneIsOmen&&isClaimed));

        Sprite top = null;
        if (isClaimed)
        {
            if (!isThrone) switch (biome)
            {
                case Biome.Forest: top = ForestMount;
                    break;
                case Biome.Aquifer: top = AquiferMount;
                    break;
                default: top = PlainsMount;
                    break;
            }
        }
        else
        {
            switch (biome)
            {
                case Biome.Rock:top = RockTop;
                    break;
                case Biome.Forest:top = ForestTop;
                    break;
                case Biome.Aquifer:top = AquiferTop;
                    break;
                default: top = PlainsTop;
                    break;
            }
        }
        topSprite.gameObject.SetActive(top!=null);
        topSprite.sprite = top;
        topSprite.sortingOrder=biome==Biome.Aquifer&&!isClaimed ? -4 : 2;

        for (int i = 0; i < 6; i++)
        {
            connectionMarkers[i].SetActive(((int)conA == i || (int)conB == i) && isBlocked);
        }

        Recipe recipe = myRecipe;
        if (isThrone) recipe = null;
        ThroneRecipeDisplay.SetActive(isThrone);
        RecipeDisplay.SetActive(recipe != null);
        if (recipe != null && recipe.name != "")
        {
            Resource
                inA = recipe.Inputs.Length < 1 ? Resource.None : recipe.Inputs[0],
                inB = recipe.Inputs.Length < 2 ? Resource.None : recipe.Inputs[1],
                outA = recipe.Outputs.Length < 1 ? Resource.None : recipe.Outputs[0],
                outB = recipe.Outputs.Length < 2 ? Resource.None : recipe.Outputs[1];

            InputA.sprite = library.GetIcon(inA);
            InputB.sprite = library.GetIcon(inB);
            OutputA.sprite = library.GetIcon(outA);
            OutputB.sprite = library.GetIcon(outB);

            bool
                in1Met, in2Met,
                metRecipe = recipeMet(out in1Met, out in2Met);
            if (metRecipe && !recipeWasMet &&!isThrone) Reward();
            recipeWasMet = metRecipe;

            InputA.color = in1Met ? Color.white : Color.Lerp(Color.white, Color.black, 0.25f);
            InputB.color = in2Met ? Color.white : Color.Lerp(Color.white, Color.black, 0.25f);

            Arrow.color = metRecipe ? arrowCol : Color.white;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!GameManager.playerAgency|| EventSystem.current.IsPointerOverGameObject()) return;

        if (GameManager.main.Card.type==Card.Type.Omen && isClaimed)
        {
            isOmen = !isOmen;
            if (isOmen)
                AudioEffectManager.main.PlaceOmen();
            isSelected = true;
            Refresh();
            return;
        }
        if (!ContextDisplay.main.orderMenu.open && Selected && !isSelected &&Selected.isClaimed && isEnvisioned && !isClaimed)
        {
            if(Claim(Selected)) 
                AudioEffectManager.main.PlaceTile();
        }
        else if (!ContextDisplay.main.orderMenu.open && Selected && !isSelected &&Selected.isClaimed && isEnvisioned && isClaimed)
        {
            if (Connect(Selected))
            {
                GameManager.main.NextTurn();
                AudioEffectManager.main.PlaceTile();
            }
        }
        else if (isClaimed)
            isSelected = !isSelected;
        else if(Selected)Selected.isSelected = false;

        Refresh();
    }

    public bool Claim(HexCell from)
    {
        bool valid = Connect(from);
        if(valid)
        {
            isClaimed = true;
            order = -1;
            ContextDisplay.main.OpenOrder(this, from);
            Refresh();
        }

        return valid;
    }
    public bool ConnectAdjacent(HexCell cell)
    {
        if (!isClaimed || !cell.isClaimed || cell.isBlocked||cell.biome==Biome.Rock) return false;

        connectedClaimed.Add(cell);
        cell.connectedClaimed.Add(this);

        return true;
    }
    public bool Connect(HexCell from)
    {
        if (isBlocked) return false;

        List<HexCell> conCells = new List<HexCell>();

        Card card = GameManager.main.Card;

        List<Vector3Int>
            allPaths,
            coordinates = card.GetCoordinates(from.coordinates.AsVector3Int(), out allPaths);
        int initialIndex = coordinates.IndexOf(this.coordinates.AsVector3Int());
        for (int i = 1; i < card.range; i++)//excluding this
        {
            int index = initialIndex + (i - 1) * 6;
            Vector3Int vec = allPaths[index];
            HexCell cell = HexMap.main.cellDict[vec];
            if (cell.isBlocked || cell.isClaimed || cell.biome == Biome.Rock) return false;
            conCells.Add(cell);
        }

        HexCell last = from, next;
        for (int i = 0; i < conCells.Count; i++)//excluding this
        {
            next = i < conCells.Count - 1 ? conCells[i + 1] : this;
            HexCell cell=conCells[i];
            cell.isBlocked = true;
            GetConDirection(last, cell, next, out cell.conA, out cell.conB);
            cell.Refresh();
            last = cell;
        }

        connectedBlocked.AddRange(conCells);
        connectedClaimed.Add(from);
        from.connectedBlocked.AddRange(conCells);
        from.connectedClaimed.Add(this);

        return true;
    }
    public void Destroy()
    {
        foreach (HexCell conClaimed in connectedClaimed)
        {
            conClaimed.connectedClaimed.Remove(this);
            List<HexCell> cutBlocked = new List<HexCell>();
            foreach (HexCell c in connectedBlocked)
            {
                if (conClaimed.connectedBlocked.Contains(c)) cutBlocked.Add(c);
            }
            foreach (HexCell c in cutBlocked)
            {
                conClaimed.connectedBlocked.Remove(c);
            }
            conClaimed.Refresh();
        }
        connectedClaimed = new List<HexCell>();

        foreach (HexCell conPath in connectedBlocked)
        {
            conPath.isBlocked = false;
            conPath.Refresh();
        }
        connectedBlocked = new List<HexCell>();
        isClaimed = false;
        Refresh();

        if (isThrone)
        {
            isThrone = false;
            GameManager.main.GameOver();
        }
    }

    public void Reward()
    {
        AudioEffectManager.main.UnlockCard();
        GameManager.main.AddToStack();
        RewardPop.gameObject.SetActive(true);
        RewardPop.localPosition = Vector3.zero;
        rewardMoveTween = RewardPop.DOLocalMoveY(0.75f, 1f).SetEase(Ease.OutBack).OnComplete(() => { RewardPop.gameObject.SetActive(false); });
        rewardSpinTween = RewardPop.DOShakeRotation(0.75f,180,0,0);
        //Check Neighbours?
    }

    public HexCell GetNeighbor(HexDirection dir)
    {
        return HexMap.main.cellDict[coordinates.AsVector3Int()+HexMetrics.HexCoordinateDir[(int)dir]];
    }
    public void GetConDirection(HexCell previous, HexCell current, HexCell next, out HexDirection conA, out HexDirection conB)
    {
        conA = HexDirection.NE;
        conB = HexDirection.NE;

        Vector3Int vec;

        vec = previous.coordinates.AsVector3Int() - current.coordinates.AsVector3Int();
        for (int i = 0; i < 6; i++)
        {
            if (vec == HexMetrics.HexCoordinateDir[i])
            {
                conA = (HexDirection)i;
                break;
            }
        }
        vec = next.coordinates.AsVector3Int() - current.coordinates.AsVector3Int();
        for (int i = 0; i < 6; i++)
        {
            if (vec == HexMetrics.HexCoordinateDir[i])
            {
                conB = (HexDirection)i;
                break;
            }
        }
    }
}
public enum HexDirection
{
    NE, E, SE, SW, W, NW
}
public enum Biome
{
    Plains, Rock, Forest, Aquifer
}
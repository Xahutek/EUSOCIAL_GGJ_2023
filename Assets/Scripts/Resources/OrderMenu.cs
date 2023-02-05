using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    public bool open = false;
    HexCell cell, selected;

    public RecipeDisplay[] displays;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || HexCell.Selected != selected || HexCell.Selected == null)
            Close(true);
    }
    public void Open(HexCell cell, HexCell selected)
    {
        open = true;
        gameObject.SetActive(true);

        this.cell = cell;
        this.selected = selected;

        Recipe[] recipes = cell.library.GetRecipes(cell.biome);
        for (int i = 0; i < 3; i++)
        {
            RecipeDisplay display = displays[i];
            Recipe recipe = i<recipes.Length?recipes[i]:null;
            display.Display(recipe);
        }
    }
    public void Close(bool destroy)
    {
        if (destroy&&cell) cell.Destroy();
        cell = null;

        if (!open) return; //To avoid infinite loop

        open = false;
        gameObject.SetActive(false);
        ContextDisplay.main.Close();
    }

    public void Pick(int order)
    {
        if (!open) Close(false);

        cell.isClaimed = true;
        cell.order = order;
        HexMap.main.RevealFromCoordinates(cell.coordinates.AsVector3Int());
        for (int i = 0; i < 6; i++)
        {
            HexCell n = cell.GetNeighbor((HexDirection)i);
            if (n.isClaimed)
                cell.ConnectAdjacent(n);
        }
        cell.Refresh();
        Close(false);
        GameManager.main.NextTurn();
    }
}

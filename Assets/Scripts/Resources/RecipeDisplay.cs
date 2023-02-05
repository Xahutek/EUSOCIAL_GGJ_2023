using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDisplay : MonoBehaviour
{
    public ResourceLibrary library;
    public Recipe myRecipe;

    public TMP_Text title;

    [SerializeField]private Image
        InputA,
        InputB,
        OutputA,
        OutputB;

    public void Display(Recipe recipe)
    {
        myRecipe = recipe;

        bool active = recipe.name != "";

        name = !active? "Empty Order":recipe.name;
        gameObject.SetActive(active);
        if (active)
        {
            title.text = recipe.name;

            Resource
                    inA = recipe.Inputs.Length < 1 ? Resource.None : recipe.Inputs[0],
                    inB = recipe.Inputs.Length < 2 ? Resource.None : recipe.Inputs[1],
                    outA = recipe.Outputs.Length < 1 ? Resource.None : recipe.Outputs[0],
                    outB = recipe.Outputs.Length < 2 ? Resource.None : recipe.Outputs[1];

            InputA. sprite = library.GetIcon(inA);
            InputB. sprite = library.GetIcon(inB);
            OutputA.sprite = library.GetIcon(outA);
            OutputB.sprite = library.GetIcon(outB);

            InputA.gameObject.SetActive(inA != Resource.None);
            InputB.gameObject.SetActive(inB != Resource.None);
            OutputA.gameObject.SetActive(outA != Resource.None);
            OutputB.gameObject.SetActive(outB != Resource.None);
        }

    }


}

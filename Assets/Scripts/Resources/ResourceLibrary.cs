using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resource Library")]
public class ResourceLibrary : ScriptableObject
{
    [System.Serializable] public class Profile
    {
        public Resource resource;
        public Color color;
        public Sprite icon;
    }
    public Profile[] profiles;

    [Header("Recipes")]
    public Recipe ThroneRecipe;
    public Recipe[]
        PlainRecipes,
        ForestRecipes,
        AquiferRecipes;

    public Color GetColor(Resource resource)
    {
        Profile p = GetProfile(resource);
        if (p == null) return Color.clear;
        else return p.color;
    }
    public Sprite GetIcon(Resource resource)
    {
        Profile p = GetProfile(resource);
        if (p == null) return null;
        else return p.icon;
    }
    public Profile GetProfile(Resource resource)
    {
        foreach (Profile p in profiles)
        {
            if (p.resource == resource)
                return p;
        }
        return null;
    }

    public Recipe GetRecipe(Biome biome, int order)
    {
        if (order < 0) return null;
        Recipe[] recipes = GetRecipes(biome);

        if (recipes == null || order >= recipes.Length) return null;

        return recipes[order];
    }
    public Recipe[] GetRecipes(Biome biome)
    {
        switch (biome)
        {
            case Biome.Plains:
                return PlainRecipes;
            case Biome.Forest:
                return  ForestRecipes;
            case Biome.Aquifer:
                return AquiferRecipes;
            default: //Rock
                return null;
        }
    }
}

[System.Serializable] public class Recipe
{
    public string name;
    public Resource[] Inputs;
    public Resource[] Outputs;
}

public enum Resource
{
    None, Queen, Nymphs, Workers, Soldiers, Leaves, Wood, Water, Clay
}
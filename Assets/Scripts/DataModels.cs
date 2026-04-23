[System.Serializable]
public class PokemonData
{
    public string name;
    public Sprites sprites;
    public TypeWrapper[] types;
}

[System.Serializable]
public class Sprites
{
    public string front_default;
}

[System.Serializable]
public class TypeWrapper
{
    public TypeInfo type;
}

[System.Serializable]
public class TypeInfo
{
    public string name;
}

[System.Serializable]
public class SpeciesData
{
    public FlavorText[] flavor_text_entries;
}

[System.Serializable]
public class FlavorText
{
    public string flavor_text;
}

public class PokemonResult
{
    public string name;
    public string image;
    public string type;
}

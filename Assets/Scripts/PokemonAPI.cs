using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonAPI : MonoBehaviour
{
    public IEnumerator GetPokemon(int id, System.Action<PokemonResult> callback)
    {
        string url = "https://pokeapi.co/api/v2/pokemon/" + id;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        PokemonData data = JsonUtility.FromJson<PokemonData>(request.downloadHandler.text);

        PokemonResult result = new PokemonResult();
        result.name = data.name;
        result.image = data.sprites.front_default;
        result.type = data.types[0].type.name;

        callback(result);
    }

    public IEnumerator GetSpecies(int id, System.Action<string> callback)
    {
        string url = "https://pokeapi.co/api/v2/pokemon-species/" + id;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        SpeciesData data = JsonUtility.FromJson<SpeciesData>(request.downloadHandler.text);

        callback(data.flavor_text_entries[0].flavor_text);
    }
}
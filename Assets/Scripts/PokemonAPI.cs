using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonAPI : MonoBehaviour
{
    // 🔹 Obtener datos básicos
    public IEnumerator GetPokemon(int id, System.Action<PokemonResult> callback)
    {
        string url = "https://pokeapi.co/api/v2/pokemon/" + id;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonData data = JsonUtility.FromJson<PokemonData>(request.downloadHandler.text);

            PokemonResult result = new PokemonResult();
            result.name = data.name;
            result.imageUrl = data.sprites.front_default;

            // 👉 tipos (uno o dos)
            string types = "";
            foreach (var t in data.types)
            {
                types += t.type.name + " ";
            }
            result.type = types.Trim();

            callback(result);
        }
        else
        {
            Debug.LogError("Error API Pokemon");
        }
    }

    // 🔹 Obtener descripción (solo en inglés)
    public IEnumerator GetSpecies(int id, System.Action<string> callback)
    {
        string url = "https://pokeapi.co/api/v2/pokemon-species/" + id;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            SpeciesData data = JsonUtility.FromJson<SpeciesData>(request.downloadHandler.text);

            foreach (var entry in data.flavor_text_entries)
            {
                if (entry.language.name == "en")
                {
                    // Limpia saltos raros
                    string cleanText = entry.flavor_text
                        .Replace("\n", " ")
                        .Replace("\f", " ");

                    callback(cleanText);
                    yield break;
                }
            }

            callback("No description available");
        }
        else
        {
            Debug.LogError("Error API Species");
        }
    }
}
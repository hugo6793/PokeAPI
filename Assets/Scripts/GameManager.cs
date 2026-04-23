using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PokemonAPI api;
    public UIManager ui;

    int score = 0;
    string correctAnswer;

    int[] pokemonIds = new int[]
    {
        1,4,7,25,39,52,54,66,95,81,92,104,145,133,150
    };

    void Start()
    {
        StartCoroutine(NewQuestion());
    }

    IEnumerator NewQuestion()
    {
        int id = pokemonIds[Random.Range(0, pokemonIds.Length)];
        int type = Random.Range(0, 3);

        List<string> options = new List<string>();

        PokemonResult pokemon = null;

        yield return StartCoroutine(api.GetPokemon(id, (res) =>
        {
            pokemon = res;
        }));

        yield return StartCoroutine(ui.LoadImage(pokemon.image));

        if (type == 0)
        {
            correctAnswer = pokemon.name;
        }
        else if (type == 1)
        {
            correctAnswer = pokemon.type;
        }
        else
        {
            yield return StartCoroutine(api.GetSpecies(id, (desc) =>
            {
                ui.SetDescription(desc);
            }));

            correctAnswer = pokemon.name;
        }

        options.Add(correctAnswer);

        while (options.Count < 4)
        {
            int rand = pokemonIds[Random.Range(0, pokemonIds.Length)];

            yield return StartCoroutine(api.GetPokemon(rand, (r) =>
            {
                if (!options.Contains(r.name))
                    options.Add(r.name);
            }));
        }

        Shuffle(options);

        ui.SetOptions(options.ToArray());
    }

    public void Answer(string selected)
    {
        if (selected == correctAnswer)
            score++;

        ui.SetScore(score);

        StartCoroutine(NewQuestion());
    }

    void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}

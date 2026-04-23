using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PokemonAPI api;
    public UIManager ui;

    private int score = 0;
    private int questionCount = 0;
    private int maxQuestions = 20;

    private string correctAnswer;

    void Start()
    {
        StartCoroutine(NewQuestion());
    }

    IEnumerator NewQuestion()
    {
        if (questionCount >= maxQuestions)
        {
            ui.ShowFinal(score);
            yield break;
        }

        questionCount++;

        int id = Random.Range(1, 152);
        int type = Random.Range(0, 3);

        PokemonResult pokemon = null;

        yield return StartCoroutine(api.GetPokemon(id, (res) =>
        {
            pokemon = res;
        }));

        ui.ClearUI();

        // 🖼️ IMAXE
        if (type != 2)
        {
            yield return StartCoroutine(ui.LoadImage(pokemon.imageUrl));
        }

        // 🧠 TIPOS DE PREGUNTA
        if (type == 0) // WHO'S THAT POKEMON
        {
            correctAnswer = pokemon.name;
            ui.SetQuestion("Who's that Pokémon?");
            ui.EnableInput(true);
        }
        else if (type == 1) // TYPE
        {
            correctAnswer = pokemon.type;
            ui.SetQuestion("What type is this Pokémon?");
            ui.EnableInput(true);
        }
        else // DESCRIPTION
        {
            yield return StartCoroutine(api.GetSpecies(id, (desc) =>
            {
                ui.SetQuestion(desc);
            }));

            correctAnswer = pokemon.name;

            // opcións só con nomes
            List<string> options = new List<string>();
            options.Add(correctAnswer);

            while (options.Count < 4)
            {
                int rand = Random.Range(1, 152);

                yield return StartCoroutine(api.GetPokemon(rand, (r) =>
                {
                    if (!options.Contains(r.name))
                        options.Add(r.name);
                }));
            }

            Shuffle(options);

            ui.SetOptions(options.ToArray());
            ui.EnableButtons(true);
        }
    }

    public void SubmitAnswer(string answer)
    {
        if (answer.ToLower().Trim() == correctAnswer.ToLower())
            score++;

        ui.SetScore(score);
        StartCoroutine(NewQuestion());
    }

    public void SelectOption(string option)
    {
        if (option == correctAnswer)
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

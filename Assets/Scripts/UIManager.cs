using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public Image pokemonImage;
    public Text descriptionText;
    public Text scoreText;
    public Button[] buttons;

    public IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        Texture2D tex = DownloadHandlerTexture.GetContent(request);

        pokemonImage.sprite = Sprite.Create(tex,
            new Rect(0,0,tex.width,tex.height),
            new Vector2(0.5f,0.5f));
    }

    public void SetOptions(string[] options)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            string opt = options[i];

            buttons[i].GetComponentInChildren<Text>().text = opt;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() =>
            {
                FindObjectOfType<GameManager>().Answer(opt);
            });
        }
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void SetDescription(string text)
    {
        descriptionText.text = text;
    }
}

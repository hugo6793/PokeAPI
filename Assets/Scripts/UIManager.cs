using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public Image pokemonImage;
    public TMP_Text questionText;
    public TMP_Text scoreText;

    public TMP_InputField inputField;

    public GameObject buttonPanel;
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

    public void SetQuestion(string text)
    {
        questionText.text = text;
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void EnableInput(bool active)
    {
        inputField.gameObject.SetActive(active);
        buttonPanel.SetActive(!active);
    }

    public void EnableButtons(bool active)
    {
        buttonPanel.SetActive(active);
        inputField.gameObject.SetActive(!active);
    }

    public void SetOptions(string[] options)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            string opt = options[i];

            buttons[i].GetComponentInChildren<TMP_Text>().text = opt;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() =>
            {
                FindObjectOfType<GameManager>().SelectOption(opt);
            });
        }
    }

    public void ClearUI()
    {
        pokemonImage.sprite = null;
        inputField.text = "";
    }

    public void OnSubmitInput()
    {
        FindObjectOfType<GameManager>().SubmitAnswer(inputField.text);
    }

    public void ShowFinal(int score)
    {
        questionText.text = "Final Score: " + score;
        inputField.gameObject.SetActive(false);
        buttonPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Gameplay : MonoBehaviour
{
    [SerializeField] Image currentHangman;
    [SerializeField] TextMeshProUGUI guessText;
    [SerializeField] SceneLoader sceneloader;

    Sprite[] hangmanStages = new Sprite[7];
    string answer = "WANKER";
    List<char> guessList = new List<char>();
    List<char> answerList = new List<char>();
    int hangmanStage = 0;
    IEnumerator coroutine;
    const int lastHangman = 6;
    const int WINSCENE = 2;
    const int LOSESCENE = 3;

    string[] swearwords = { "TWATWAFFLE", "THUNDERCUNT", "FUCKTRUMPET", "DICKSNEEZE", "CHUCKLEFUCK", "COCKWEASEL", "CUNTNUGGET", "PISSFLAPS"};

    // Start is called before the first frame update
    void Start()
    {

        LoadHangmanImages();
        currentHangman.sprite = hangmanStages[0];
        SetupGuess();
    }

    // Update is called once per frame
    void Update()
    {
        for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
        {
            if (Input.GetKeyDown(key))
            {
                UpdateGuess(key);
                break;
            }
        }
    }

    void LoadHangmanImages()
    {
        string path = "Hangman Stages/";
        for (int i = 0; i < hangmanStages.Length; i++)
        {
            path = "Hangman Stages/" + (i + 1);
            hangmanStages[i] = Resources.Load<Sprite>(path);
        }
    }

    void SetupGuess()
    {
        int rand = Random.Range(0, swearwords.Length);
        answer = swearwords[rand];
        answer = answer.ToLower();
        answerList.AddRange(answer);
        
        for (int i = 0; i < answer.Length; i++)
        {
            guessList.Add('_');
        }
        RenderGuess();
    }

    void RenderGuess()
    {
        string currentGuess = "";
        foreach (char letter in guessList)
        {
            currentGuess += letter;
        }
        guessText.text = currentGuess;
    }

    void UpdateGuess(KeyCode keycode)
    {
        bool wrongGuess = true;
        char charKey = (char)(keycode);

        foreach (var letter in answerList)
        {
            if(letter == charKey)
            {
                // Correct guess
                wrongGuess = false;
            }
        }

        if (!wrongGuess)
        {
            for (int i = 0; i < answerList.Count; i++)
            {
                if (charKey == answerList[i])
                {
                    guessList[i] = charKey;
                    answerList[i] = '_';
                }
            }
            
            string checkAnswer = new string(guessList.ToArray());
            if (checkAnswer.Equals(answer))
            {
                coroutine = WaitForEndScene(1.5f, WINSCENE);
                StartCoroutine(coroutine);
            }
        }
        else
        {
            foreach(var letter in guessList)
            {
                if(letter == charKey)
                {
                    // Already guessed that letter
                    return;
                }
            }
            // Wrong guess
            hangmanStage++;
            if (hangmanStage == lastHangman)
            {
                coroutine = WaitForEndScene(1.5f, LOSESCENE);
                StartCoroutine(coroutine);
            }
           
            currentHangman.sprite = hangmanStages[hangmanStage];
        }
        
        RenderGuess();
    }

    IEnumerator WaitForEndScene(float waitTime, int scene)
    {
        yield return new WaitForSeconds(waitTime);
        sceneloader.LoadScene(scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{

    public CameraMove camerMove;

    public Transform studyScreen;

    public Transform quizScreen;

    public Transform failScreen;

    public Transform passScreen;

    public Transform confirmScreen;

    public Transform thankYouScreen;

    public Transform mainMenuScreen;

    public void StudyScreen()
    {
        camerMove.setAnchor(studyScreen);
    }

    public void TakeQuiz()
    {
        camerMove.setAnchor(quizScreen);
    }

    public void QuitApp()
    {
        camerMove.setAnchor(mainMenuScreen);

        Debug.Log("Quit Application");

        Application.Quit();
    }



    public VideoPlayer videoPlayer;

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    public void RewindVideo()
    {
        videoPlayer.Pause();

        videoPlayer.frame = 0;
    }

    public void ReturnFromVideo()
    {
        videoPlayer.Pause();

        videoPlayer.frame = 0;

        camerMove.setAnchor(mainMenuScreen);
    }



    string allQuestions;

    public List<List<string>> questionsList = new List<List<string>>();

    int currentQuestion = 0;

    public Text titleText, questionText, answer1Text, answer2Text, answer3Text, answer4Text;

    public string correctAnswerText;

    public int totalNumberOfQuestions;

    public System.Random rnd = new System.Random();

    void Start()
    {
        TextAsset readingQuestionsFile = Resources.Load("questions") as TextAsset;

        string allQuestions = readingQuestionsFile.text;

        string[] questions = allQuestions.Split(";");

        totalNumberOfQuestions = questions.Length;

        foreach (string question in questions)
        {
            string[] questionParts = question.Split(",");

            List<string> tempList = new List<string>();

            tempList.Add(questionParts[0].Replace("\n", ""));

            tempList.Add(questionParts[1].Replace("\n", ""));

            tempList.Add(questionParts[2].Replace("\n", ""));

            tempList.Add(questionParts[3].Replace("\n", ""));

            tempList.Add(questionParts[4].Replace("\n", ""));

            tempList.Add(questionParts[5].Replace("\n", ""));

            questionsList.Add(tempList);
        }

        questionsList = ShuffleQuestoins(questionsList);

        titleText.text = "Question " + (currentQuestion + 1) + " of " + totalNumberOfQuestions;

        questionText.text = questionsList[currentQuestion][0] + "?";

        List<string> tempAnswersList = new List<string>();

        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        tempAnswersList = ShuffleAnswers(tempAnswersList);

        answer1Text.text = tempAnswersList[0];

        answer2Text.text = tempAnswersList[1];

        answer3Text.text = tempAnswersList[2];

        answer4Text.text = tempAnswersList[3];

        correctAnswerText = questionsList[currentQuestion][5];
    }

    public List<List<string>> ShuffleQuestoins(List<List<string>> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;

            int k = rnd.Next(n + 1);

            List<string> value = list[k];

            list[k] = list[n];

            list[n] = value;
        }
        return list;
    }

    public List<string> ShuffleAnswers(List<string> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }



    public int totalAnswersRight = 0;

    public GameObject checkAnswerButton;

    public string studentAnswer;

    public GameObject correctAnswerPanel, incorrectAnswePannel;

    public void ToggleChecked()
    {
        int togglesChecked = 0;

        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        foreach (GameObject toggle in toggles)
        {
            if (toggle.GetComponent<Toggle>().isOn)
            {
                togglesChecked++;

                checkAnswerButton.SetActive(true);

                studentAnswer = toggle.transform.Find("Label").GetComponent<Text>().text;
            }
        }
        if(togglesChecked == 0)
        {
            checkAnswerButton.SetActive(false);

            studentAnswer = "";
        }
    }

    public void CheckAnswer()
    {
        checkAnswerButton.SetActive(false );

        if (string.Equals(studentAnswer, correctAnswerText))
        {
            totalAnswersRight++;

            correctAnswerPanel.SetActive(true);
        }
        else
        {
            incorrectAnswePannel.SetActive(true);
        }
    }

    public void CloseAnswerPannel()
    {
        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        foreach(GameObject toggle in toggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }

        incorrectAnswePannel.SetActive(false);
        correctAnswerPanel.SetActive(false) ;

        NextQuestion();
    }



    public int passingGrade;

    public Text failText;

    public Text passText;

    public void NextQuestion()
    {

        currentQuestion++;

        if(currentQuestion > (questionsList.Count - 1))
        {
            float studentScore = ((float)totalAnswersRight / (float)questionsList.Count) * 100;

            if(studentScore > passingGrade)
            {
                passText.text = totalAnswersRight + " out of " + totalNumberOfQuestions + " questions is a grade of " + studentScore + " . "
                    + "\n" + passingGrade + " was the minimum passing grade. ";

                camerMove.setAnchor(passScreen);

                return;
            }
            else
            {
                failText.text = totalAnswersRight + " out of " + totalNumberOfQuestions + " questions is a grade of " + studentScore + " . "
                    + "\n" + passingGrade + " was the minimum passing grade. ";

                camerMove.setAnchor(failScreen);

                return;
            }
        }

        titleText.text = "Question " + (currentQuestion + 1) + " of " + totalNumberOfQuestions;

        questionText.text = questionsList[currentQuestion][0] + "?";

        List<string> tempAnswersList = new List<string>();

        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        tempAnswersList = ShuffleAnswers(tempAnswersList);

        answer1Text.text = tempAnswersList[0];

        answer2Text.text = tempAnswersList[1];

        answer3Text.text = tempAnswersList[2];

        answer4Text.text = tempAnswersList[3];

        correctAnswerText = questionsList[currentQuestion][5];
    }


    public InputField userSignature;

    public Text certificateSignature;

    public Text certificateDate;

    public RectTransform certificate;

    public void EnterSignature()
    {
        certificateSignature.text = userSignature.text;

        certificateDate.text = System.DateTime.Now.ToString();

        camerMove.setAnchor(confirmScreen);
    }


    public void RedoSignature()
    {
        userSignature.text = "";

        certificateDate.text = "";

        camerMove.setAnchor(passScreen);
    }

    public void ConfirmSignature()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        float widthFactor = Screen.width / 1920f;
        float heightFactor = Screen.height / 1080f;
        int width = Mathf.FloorToInt(certificate.rect.width * widthFactor);
        int height = Mathf.FloorToInt((certificate.rect.height * heightFactor));

        Vector2 certPos = certificate.anchoredPosition;

        var ss = new Texture2D(width, height, TextureFormat.RGB24, false);

        ss.ReadPixels(new Rect(certPos.x, certPos.y, width, height), 0, 0);

        ss.Apply();

#if UNITY_EDITOR
        System.IO.File.WriteAllBytes(Application.dataPath + ".png", ss.EncodeToPNG());
        Debug.Log("Unity Editor");
#endif
#if UNITY_ANDROID
        NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));
#endif
    }





    public void MainMenu()
    {
        Restart();

        camerMove.setAnchor(mainMenuScreen);
    }

    public void Restart()
    {
        allQuestions = "";

        questionsList = new List<List<string>>();

        currentQuestion = 0;

        totalAnswersRight = 0;

        Start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

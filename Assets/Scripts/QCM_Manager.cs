using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QCM_Manager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public XML_Reader xML_Reader;
    public Question[] listQuestions;
    public int id = 0;
    public Transform answersPanel;
    public GameObject answerPrefab;
    public int nbAnswerCorrect = 0;
    public GameObject result;
    public GameObject btnValider;
    public bool isBtnValider = true;
    
    void Start()
    {
        // parser le fichier xml
        xML_Reader.InitListQuestion();
        listQuestions = xML_Reader.getListQuestion;
        // afficher la première question
        updateTitle();
        ShowQuestion();
    }



    void ShowQuestion()
    {
        // effacer les anciens toggles de réponse
        foreach (Transform child in answersPanel)
        {
            Destroy(child.gameObject);
        }

        // créer des toggles pour chaque réponse
        float i = 200;
        foreach (var answer in listQuestions[id].answer)
        {
            // instancier le prefab Answer
            GameObject answerToggle = Instantiate(answerPrefab, answersPanel);
            
            // placer le prefab
            RectTransform rectTransform = answerToggle.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(-100f, i, 0f);

            answerToggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = answer.text;
            i-=100;
        }
    }

    // modifier le titre de la question
    public void updateTitle()
    {        
        title.text = "Question " + (id+1) + ":\n" + listQuestions[id].title;
    }

    // changer l'indice de la question
    private void indSuiv()
    {
        if (listQuestions.Length-1 > id) id++;
    }

    // vérifier si on a eu bon à la question
    private bool testAnswerCorrect()
    {
        bool test = true;

        // récupérer l'object Answers qui contient les toggles de réponse
        GameObject answerToggles = GameObject.Find("Answers");

        // récupérer les toggles enfant de Answers
        for (int i = 0; i < answerToggles.transform.childCount; i++)
        {
            Toggle toggleComponent = answerToggles.transform.GetChild(i).GetComponent<Toggle>();

            // vérifie si les cases coché son correct et si on en a pas oublié 
            bool isToggleOn = toggleComponent.isOn;
            if ((isToggleOn && !listQuestions[id].answer[i].correct) || (!isToggleOn && listQuestions[id].answer[i].correct))
            {
                test = false;
            }
        }
        
        return test;
    }

    // afficher la phenetre de résultat
    private void windowEnd()
    {
        // effacer les anciens toggles de réponse
        foreach (Transform child in answersPanel)
        {
            Destroy(child.gameObject);
        }
        // désactiver le bouton valider
        btnValider.SetActive(false);
        // modifier le titre en résultat
        title.text = "Résultat";
        // activer et modifier le nombre de réponse correct
        result.SetActive(true);
        if (nbAnswerCorrect > 1) result.GetComponent<TextMeshProUGUI>().text = "Vous avez répondu juste à " + nbAnswerCorrect + " questions sur " + listQuestions.Length + "."; 
        else result.GetComponent<TextMeshProUGUI>().text = "Vous avez répondu juste à " + nbAnswerCorrect + " question sur " + listQuestions.Length + ".";
    }

    // met en vert les réponses correct
    private void showAnswersCorrect()
    {
        GameObject answerToggles = GameObject.Find("Answers");
        for (int i = 0; i < answerToggles.transform.childCount; i++)
        {
            TextMeshProUGUI text = answerToggles.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();

            if (listQuestions[id].answer[i].correct)
            {
                text.color = Color.green;
            }

            // désactive l'interaction des toggles
            Toggle toggleComponent = answerToggles.transform.GetChild(i).GetComponent<Toggle>();
            toggleComponent.interactable = false;
        }

    }

    // commande pour le bouton valider
    public void Valider()
    {
        if (listQuestions.Length-1 <= id && !isBtnValider) windowEnd();
        else 
        {
            if (!isBtnValider) 
            {
                bool test = testAnswerCorrect();
                if (test) nbAnswerCorrect++;
                indSuiv();
                updateTitle();
                ShowQuestion();
                isBtnValider = true;
            }
            else
            {
                showAnswersCorrect();
                isBtnValider = false;
            }
        }
    }

    // commande pour le bouton quitter
    public void Quitter()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;


public class XML_Reader : MonoBehaviour
{
    // tableau qui contient les questions
    public Question[] listQuestions;

    // fonction qui permet de parser le fichier XML et de remplir le tableau
    public void InitListQuestion()
    {
        // Chemin du fichier XML
        string cheminFichier = "Assets/StreamingAssets/qcmFile.xml";
        
        if (File.Exists(cheminFichier))
        {
            TextAsset textAsset = new TextAsset(System.IO.File.ReadAllText(cheminFichier));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList questions = xmlDoc.GetElementsByTagName("question");

            listQuestions = new Question[questions.Count];

            int i = 0;
            foreach (XmlNode questionNode in questions)
            {
                Question q = new Question();
                q.title = questionNode.SelectSingleNode("title").InnerText;

                XmlNodeList answers = questionNode.SelectNodes("answer");
                q.answer = new Answer[answers.Count];

                int j = 0;
                foreach (XmlNode answerNode in answers)
                {
                    Answer a = new Answer();
                    a.text = answerNode.InnerText;
                    a.correct = (answerNode.Attributes["correct"] != null && bool.Parse(answerNode.Attributes["correct"].Value));
                    q.answer[j] = a;
                    j++;
                }

                listQuestions[i] = q;
                i++;
            }
        }
        else
        {
            Debug.LogError("Le fichier XML n'a pas pu être trouvé à l'emplacement : " + cheminFichier);
        }

    }

    // getteur du tableau
    public virtual Question[] getListQuestion
    {
        get {return listQuestions;}
    }

}

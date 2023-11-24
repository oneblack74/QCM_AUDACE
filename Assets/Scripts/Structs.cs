using UnityEngine;

// structure qui contient le text de chaque choix de réponse et si elle est valide
public struct Answer
{
    public string text;
    public bool correct;
}

// structure qui contient l'ennoncé de la question et la liste des réponses
public struct Question
{
    public string title;
    public Answer[] answer;
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using MimeKit;
using System.Net.Mail;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;

public class Database : MonoBehaviour
{
    private const string supabaseUrl = "https://edptwbaxbtriutkovqzy.supabase.co";
    private const string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVkcHR3YmF4YnRyaXV0a292cXp5Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc0MjIyOTczMywiZXhwIjoyMDU3ODA1NzMzfQ.5wQWO896Sswuq0EsO4PT74zcyY6e5U59_fRc82AFvxY";
    private const string tableName = "contestant";

    private int GetCurrentMonthId()
    {
        DateTime currentDate = DateTime.Now;
        int currentMonthId = currentDate.Year * 100 + currentDate.Month;
        return currentMonthId;
    }
    public IEnumerator PlayerCanParticipatedInCurrentContest(string email,string name,string lastname,TextMeshProUGUI textError, System.Action<Contestant> callback)
    {
        //StartCoroutine(GetParticipants());
        int currentMonthId = GetCurrentMonthId();
        email = Regex.Replace(email, @"\p{C}+", "");
        email = email.Trim();
        string encodedEmail = Uri.EscapeDataString(email);
        string url = $"{supabaseUrl}/rest/v1/{tableName}?select=*&email=eq.{encodedEmail}&idmonth=eq.{currentMonthId}";
        Debug.Log("URL encodée: " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apiKey", supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();
        Debug.Log(request);
        if (request.result != UnityWebRequest.Result.Success)
        {
            textError.text = "Please refresh the site and start again";
            Debug.LogError("Erreur lors de la récupération des données : " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            if (string.IsNullOrEmpty(request.downloadHandler.text) || jsonResponse == "[]")
            {
                textError.text = "YOU CAN PLAY";
                Debug.Log("Le joueur n'existe pas pour ce mois.");
                StartCoroutine(CreateContestant(name, lastname, email,callback));
            }
            else
            {
                ProcessParticipantsData(jsonResponse);
                Contestant player = ProcessParticipantsData(jsonResponse)[0];
                if (player.GetLastDayPlayed().Date==DateTime.Today) {
                    textError.text = "Come back tomorrow for another attempt !";
                }
                else
                {
                    Debug.Log("Le joueur n'a pas jouer ajd.");
                    textError.text = "YOU CAN PLAY";
                    string jsonBody = "{\"lastdayplayed\": \"" + DateTime.Today.ToString("yyyy-MM-dd") + "\"}";
                    StartCoroutine(PutContestant(player,jsonBody) );
                    /*jsonBody = "{\"score\": \"" +10 + "\"}";
                    StartCoroutine(PutContestant(player,jsonBody) );*/
                    callback?.Invoke(player);
                }
                
            }
        }
    }

    IEnumerator GetParticipants()
    {
        string url = $"{supabaseUrl}/rest/v1/{tableName}?select=*";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apiKey", supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur lors de la récupération des données : " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Réponse du serveur : " + jsonResponse);
            ProcessParticipantsData(jsonResponse);
        }
    }

    List<Contestant> ProcessParticipantsData(string jsonResponse)
    {
        string wrappedJson = "{\"participants\":" + jsonResponse + "}";
        ParticipantList participantList = JsonUtility.FromJson<ParticipantList>(wrappedJson);
        foreach (Contestant participant in participantList.participants)
        {
            Debug.Log($"Nom: {participant.name} {participant.lastname}, Email: {participant.email}, Score: {participant.score}, Time : {participant.lastdayplayed}");
        }
        return participantList.participants;
    }

    [System.Serializable]
    public class Contestant
    {
        public string email;
        public int idmonth;
        public string name;
        public string lastname;
        public int score;
        public string lastdayplayed;
        public DateTime GetLastDayPlayed()
        {
            if (DateTime.TryParse(lastdayplayed, out DateTime parsedDate))
            {
                return parsedDate;
            }
            return DateTime.MinValue;
        }
        public Contestant(string email, int idMonth, string name, string lastname, int score, DateTime lastdayplayed)
        {
            this.email = email;
            this.idmonth = idMonth;
            this.name = name;
            this.lastname = lastname;
            this.score = score;
            this.lastdayplayed = lastdayplayed.ToString("yyyy-MM-dd");
        }
    }

    [System.Serializable]
    public class ParticipantList
    {
        public List<Contestant> participants;
    }

    private IEnumerator CreateContestant(string name,string lastname,string email, System.Action<Contestant> callback)
    {
        Contestant newContestant = new Contestant(email, GetCurrentMonthId(), name, lastname, 0, DateTime.Now);
        string jsonData = JsonUtility.ToJson(newContestant);

        string url = $"{supabaseUrl}/rest/v1/{tableName}";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Contestant ajouté avec succès : " + request.downloadHandler.text);
            callback?.Invoke(newContestant);
        }
        else
        {
            Debug.LogError("Erreur lors de l'ajout du Contestant : " + request.error);
        }
    }
    public IEnumerator PutContestant(Contestant updatedContestant,string jsonBody)
    {
        //string jsonData = JsonUtility.ToJson(updatedContestant);
        updatedContestant.email = Regex.Replace(updatedContestant.email, @"\p{C}+", "");
        updatedContestant.email = updatedContestant.email.Trim();
        string encodedEmail = Uri.EscapeDataString(updatedContestant.email);
        string url = $"{supabaseUrl}/rest/v1/{tableName}?select=*&email=eq.{encodedEmail}&idmonth=eq.{updatedContestant.idmonth}";
        Debug.Log(updatedContestant.lastdayplayed);

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        // Attendre la réponse
        yield return request.SendWebRequest();

        // Vérifier les erreurs
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Date mise à jour avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de la mise à jour de la date : " + request.error);
        }
    }
}

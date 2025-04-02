using TMPro;
using UnityEngine;
using System.Collections;
using System.Net.Mail;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class StartPlay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI emailAddressTxt;
    [SerializeField] TextMeshProUGUI lastNameTxt;
    [SerializeField] TextMeshProUGUI firstNameTxt;
    [SerializeField] TextMeshProUGUI errorTxt;
    [SerializeField] Database db;
    public static Database.Contestant player;
   public void CheckMail()
   {
        string emailAddress=emailAddressTxt.text;
        string lastname= lastNameTxt.text;
        string firstname= firstNameTxt.text;
        if(emailAddress.Trim().Length ==1 || !IsValidEmail(emailAddress))
        {
            errorTxt.text = "Please enter a valid email.";
            return;
        }
        if (lastname.Trim().Length == 1) {
            errorTxt.text = "Please enter a valid last name.";
            return;
        }
        if (firstNameTxt.text.Trim().Length == 1) {
            errorTxt.text = "Please enter a valid first name.";
            return;
        }
        StartCoroutine(StartToPlay(emailAddress,firstname,lastname));
   }
    private IEnumerator StartToPlay(string emailAddress,string firstname,string lastname)
    {
        yield return StartCoroutine(db.PlayerCanParticipatedInCurrentContest(emailAddress, firstname, lastname, errorTxt,(player) =>
        {
            StartPlay.player = player;
            var parameters = new LoadSceneParameters(LoadSceneMode.Single);
            SceneManager.LoadScene("MainGame");
            /*SceneManager.LoadSceneAsync("MainGame", parameters).completed += (operation) =>
            {
                SceneManager.GetActiveScene().GetRootGameObjects()[0].GetComponent<Player>().ReceiveData(player);
            };*/
        }));
        
    }
    public bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }
}

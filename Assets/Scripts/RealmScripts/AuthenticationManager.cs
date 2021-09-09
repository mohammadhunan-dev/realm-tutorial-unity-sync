using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AuthenticationManager : MonoBehaviour
{

    public static VisualElement root;
    public static Label subtitle;
    public static Button startButton;
    public static bool isShowingRegisterUI = false;
    //public static string loggedInUser;
    public static TextField userInput;

    // sync variables:
    public static Realms.Sync.User syncUser;
    public static TextField passInput;
    public static Button toggleLoginOrRegisterUIButton;
    public static Player currentPlayer;



    // LOCAL START:
    //// Start is called before the first frame update
    //void Start()
    //{
    //    root = GetComponent<UIDocument>().rootVisualElement;

    //    loginSubtitle = root.Q<Label>("login-subtitle");
    //    startButton = root.Q<Button>("start-button");

    //    userInput = root.Q<TextField>("username-input");


    //    startButton.clicked += () =>
    //    {
    //        onPressLogin();
    //    };
    //}

    // SYNC START
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        subtitle = root.Q<Label>("subtitle");
        startButton = root.Q<Button>("start-button");
        userInput = root.Q<TextField>("username-input");
        passInput = root.Q<TextField>("password-input"); // sync line
        passInput.isPasswordField = true; // sync line
        toggleLoginOrRegisterUIButton = root.Q<Button>("toggle-login-or-register-ui-button"); // sync line


        //startButton.clicked += () =>
        //{
        //    onPressLogin();
        //};

        // sync toggle:

        toggleLoginOrRegisterUIButton.clicked += () =>
        {
            // if the registerUI is already visible, switch to the loginUI and set isShowingRegisterUI to false	
            if (isShowingRegisterUI == true)
            {
                switchToLoginUI();
                isShowingRegisterUI = false;
            }
            else
            {
                switchToRegisterUI();
                isShowingRegisterUI = true;
            }
        };

        // sync start click:
        startButton.clicked += async () =>
        {
            if (isShowingRegisterUI == true)
            {
                onPressRegister();
            }
            else
            {
                onPressLogin();
            }
        };
    }
    // sync method:
    public static void switchToLoginUI()
    {
        subtitle.text = "Login";
        startButton.text = "Login & Start Game";
        toggleLoginOrRegisterUIButton.text = "Don't have an account yet? Register";
    }
    // sync method:
    public static void switchToRegisterUI()
    {
        subtitle.text = "Register";
        startButton.text = "Signup & Start Game";
        toggleLoginOrRegisterUIButton.text = "Have an account already? Login";
    }


    // LOCAL onPressLogin:
    //public static void onPressLogin()
    //{
    //    try
    //    {
    //        Debug.Log("login pressed!");
    //        root.AddToClassList("hide");
    //        loggedInUser = userInput.value;
    //        RealmController.setLoggedInUser(loggedInUser);
    //        ScoreCardManager.setLoggedInUser(loggedInUser);
    //        LeaderboardManager.Instance.setLoggedInUser(loggedInUser);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log("an exception was thrown:" + ex.Message);
    //    }
    //}


    // Sync onPressLogin:
    public static async void onPressLogin()
    {
        try
        {
            currentPlayer = await RealmController.setLoggedInUser(userInput.value, passInput.value);
            if (currentPlayer != null)
            {
                root.AddToClassList("hide");
            }
            ScoreCardManager.setLoggedInUser(currentPlayer.Name);
            LeaderboardManager.Instance.setLoggedInUser(currentPlayer.Name);
        }
        catch (Exception ex)
        {
            Debug.Log("an exception was thrown:" + ex.Message);
        }
    }

    public static async void onPressRegister()
    {
        try
        {
            currentPlayer = await RealmController.OnPressRegister(userInput.value, passInput.value);

            if (currentPlayer != null)
            {
                root.AddToClassList("hide");
            }
            ScoreCardManager.setLoggedInUser(currentPlayer.Name);
            LeaderboardManager.Instance.setLoggedInUser(currentPlayer.Name);

        }
        catch (Exception ex)
        {
            Debug.Log("an exception was thrown:" + ex.Message);
        }
    }

}


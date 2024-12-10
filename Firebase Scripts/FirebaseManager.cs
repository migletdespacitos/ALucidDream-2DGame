using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

//Referenced from Tornadoally and xzippyzachx
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference DBreference;
    [Space(5f)]

    [Header("Login References")]
    [SerializeField]
    private TMP_InputField loginEmail;
    [SerializeField]
    private TMP_InputField loginPassword;
    [SerializeField]
    private TMP_Text loginOutputText;
    [Space(5f)]

    [Header("Register References")]
    [SerializeField]
    private TMP_InputField registerUsername;
    [SerializeField]
    private TMP_InputField registerEmail;
    [SerializeField]
    private TMP_InputField registerPassword;
    [SerializeField]
    private TMP_InputField registerConfirmPassword;
    [SerializeField]
    private TMP_Text registerOutputText;

    public bool IsSavingGame {get; private set;}

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) 
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependencies());
        
        loginPassword.contentType = TMP_InputField.ContentType.Password;
        registerPassword.contentType = TMP_InputField.ContentType.Password;
        registerConfirmPassword.contentType = TMP_InputField.ContentType.Password;
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();
        
        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependenciesTask.Result;

        if (dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyResult}");
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Reload information of the last used account.
    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();

        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    // If no modifications were made to account details between the previous and current game session,
    // user will be logged into the game,
    // user will be directed to the Login Screen to login otherwise.
    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                GameManager.instance.ChangeScene(1);
                // TODO - Edit this part to incorporate Save System
                //StartCoroutine(LoadUserData());
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
            }
            
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (signedIn && user != null)
            {
                Debug.Log("Signed Out");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"Signed In: {user.DisplayName}");
            }
        }
    }

    public void ClearTextFields()
    {
        loginEmail.text = "";
        loginPassword.text = "";
        loginOutputText.text = "";

        registerUsername.text = "";
        registerEmail.text = "";
        registerPassword.text = "";
        registerConfirmPassword.text = "";
        registerOutputText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    public void RegisterAccountButton()
    {
        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }

    public void ResetPasswordButton()
    {
        StartCoroutine(ResetPasswordLogic(loginEmail.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError) firebaseException.ErrorCode;
            string output = "Unknown Error, Please Try Again";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Please Enter Your Email";
                    break;
                case AuthError.MissingPassword:
                    output = "Please Enter Your Password";
                    break;
                case AuthError.InvalidEmail:
                    output = "Invalid Email";
                    break;
                case AuthError.WrongPassword:
                    output = "Incorrect Password";
                    break;
                case AuthError.UserNotFound:
                    output = "Account Does Not Exist";
                    break;
            }
            loginOutputText.text = output;
        }
        else
        {
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1f);

                // Change to Lobby Scene
                Debug.Log("Logged in");
                GameManager.instance.ChangeScene(1);
                // TODO - Edit to incorporate Save System
                // StartCoroutine(LoadUserData());
            }
            else
            {
                Debug.Log("Sending verification email");
                StartCoroutine(SendVerificationEmail());
            }
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if (_username == "")
        {
            registerOutputText.text = "Please Enter A Username";
        }
        else if (_password != _confirmPassword)
        {
            registerOutputText.text = "Passwords Do Not Match!";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError) firebaseException.ErrorCode;
                string output = "Unknown Error, Please Try Again";

                switch (error)
                {
                    case AuthError.InvalidEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        output = "Email Already In Use";
                        break;
                    case AuthError.WeakPassword:
                        output = "Weak Password";
                        break;
                    case AuthError.MissingEmail:
                        output = "Please Enter Your Email";
                        break;
                    case AuthError.MissingPassword:
                        output = "Please Enter Your Password";
                        break;
                }
                registerOutputText.text = output;
            }
            else
            {
                // Create a user profile and set the username
                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                if (defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError) firebaseException.ErrorCode;
                    string output = "Unknown Error, Please Try Again";

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "Update User Cancelled";
                            break;
                        case AuthError.SessionExpired:
                            output = "Session Expired";
                            break;
                    }
                    registerOutputText.text = output;
                }
                else
                {
                    Debug.Log($"Firebase User Created Successfully: {user.DisplayName} ({user.UserId})");

                    StartCoroutine(SendVerificationEmail());
                }
            }
        }
    }

    private IEnumerator SendVerificationEmail()
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if (emailTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "Unknown Error, Try Again!";

                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Verification Task was Cancelled";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Too Many Requests";
                        break;
                }

                AuthUIManager.instance.AwaitVerification(false, user.Email, output);
            }
            else
            {
                AuthUIManager.instance.AwaitVerification(true, user.Email, null);
                Debug.Log("Email Sent Successfully");
            }
        }
    }

    private IEnumerator ResetPasswordLogic(string _email)
    {
        if (_email == "")
        {
            loginOutputText.text = "Please Enter An Email";
        }
        else
        {
            var resetPasswordTask = auth.SendPasswordResetEmailAsync(_email);

            yield return new WaitUntil(predicate: () => resetPasswordTask.IsCompleted);

            if (resetPasswordTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)resetPasswordTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "Unknown Error, Try Again!";

                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Reset Password Task was Cancelled";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Too Many Requests";
                        break;
                    case AuthError.UserNotFound:
                        output = "Account Does Not Exist";
                        break;
                    case AuthError.InvalidEmail:
                        output = "Invalid Email";
                        break;
                }
                loginOutputText.text = output;
            }
            else
            {
                loginOutputText.text = $"Sent Reset Password Email!\nPlease Check {_email}";
                Debug.Log("Reset Password Email Sent Successfully!");
            }
        }
    }

    public void SignOut()
    {
        auth.SignOut();

        // Return to Login Screen
        Debug.Log("Signed Out!");
        GameManager.instance.ChangeScene(0);
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBreference.Child("users").Child(user.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Successfully updated database username");
        }
    }

    public IEnumerator UpdateGameData(string selectedSaveId, GameData data)
    {
        IsSavingGame = true;

        // serialize the C# game data object into Json
        string dataToStore = JsonUtility.ToJson(data, true);

        var DBTask = DBreference.Child("users").Child(user.UserId).Child(selectedSaveId).SetRawJsonValueAsync(dataToStore);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            // Game Data has been updated
        }

        IsSavingGame = false;
    }

    public IEnumerator LoadGameData(string selectedSaveId, System.Action<GameData> callback)
    {
        var DBTask = DBreference.Child("users").Child(user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        // load serialized data from the file
        string dataToLoad = "";

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("No data exists yet");
            StartCoroutine(UpdateUsernameDatabase(user.DisplayName));
        }
        else if (DBTask.Result.Child(selectedSaveId).Value == null)
        {
            Debug.Log("Data for save slot (" + selectedSaveId + ") does not exist yet");
        }
        else
        {
            Debug.Log("Data for save slot (" + selectedSaveId + ") has been retrieved");
            DataSnapshot snapshot = DBTask.Result;

            dataToLoad = snapshot.Child(selectedSaveId).GetRawJsonValue();
        }

        // deserialize the data from Json back into the C# object
        callback(JsonUtility.FromJson<GameData>(dataToLoad));
    }

    public IEnumerator LoadAllSaves(System.Action<Dictionary<string, GameData>> callback)
    {
        List<string> saveNodes = new List<string> { "save1", "save2", "save3" };
        
        Dictionary<string, GameData> saveDictionary = new Dictionary<string, GameData>();

        // Loop over all possible save nodes in the Firebase database
        foreach (string saveId in saveNodes)
        {
            // Load the game data for this saveId and put it in the dictionary
            yield return StartCoroutine(LoadGameData(saveId, (loadedGameData) =>
            {
                // defensive programming - ensure the saveId data isn't null
                if (loadedGameData != null)
                {
                    saveDictionary.Add(saveId, loadedGameData);
                }
                else
                {
                    Debug.Log("Executing LoadAllSaves, current saveId: " + saveId + " does not exist");
                }
            }));
        }

        callback(saveDictionary);
    }
}

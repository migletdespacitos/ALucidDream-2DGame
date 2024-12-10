using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Referenced from Coco Code, Tornadoally and xzippyzachx

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager instance;

    private EventSystem system;

    [Header("References")]
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    private GameObject verifyEmailUI;
    [SerializeField]
    private TMP_Text verifyEmailText;
    [SerializeField]
    private TMP_InputField loginEmail;
    [SerializeField]
    private TMP_InputField registerUsername;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        system = EventSystem.current;
        loginEmail.Select();

        if (DataPersistenceManager.instance != null)
        {
            Debug.LogError("Deleted DataPersistenceManager when transitioning back to Login UI");
            Destroy(DataPersistenceManager.instance.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }

    private void ClearUI()
    {
        FirebaseManager.instance.ClearTextFields();

        loginUI.SetActive(false);
        registerUI.SetActive(false);
        verifyEmailUI.SetActive(false);
    }

    public void LoginScreen() // Invokes when clicking on Back Button
    {
        ClearUI();
        
        loginUI.SetActive(true);
        loginEmail.Select();
    }

    public void RegisterScreen() // Invokes when clicking on Register Account Button
    {
        ClearUI();
        
        registerUI.SetActive(true);
        registerUsername.Select();
    }

    public void ExitButton()
    {
        Debug.Log("User has quit the game");
        Application.Quit();
    }

    public void AwaitVerification(bool _emailSent, string _email, string _output)
    {
        ClearUI();

        verifyEmailUI.SetActive(true);

        if (_emailSent)
        {
            verifyEmailText.text = $"Sent Email!\nPlease Verify {_email}";
        }
        else
        {
            verifyEmailText.text = $"Email Not Sent: {_output}\nPlease Verify {_email}";
        }
    }
}

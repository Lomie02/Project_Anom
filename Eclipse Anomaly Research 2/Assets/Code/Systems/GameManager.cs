using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    public bool mUseEscapeQuit;
    public UnityEvent mOnConfigure;

    void Start()
    {
        mOnConfigure.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mUseEscapeQuit)
        {
            Application.Quit();
        }
    }

    public void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}

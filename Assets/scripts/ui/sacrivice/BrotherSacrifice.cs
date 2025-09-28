using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BrotherSacrifice : MonoBehaviour
{
    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();

        m_button.onClick.AddListener(() =>
        {
            print("BrotherSacrifice");
            SceneManager.LoadScene("Bad ending", LoadSceneMode.Single);
        });
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KingFrogSacrifice : MonoBehaviour
{
    private Button m_button;

    private void Awake()
    {
        m_button = GetComponent<Button>();

        m_button.onClick.AddListener(() =>
        {
            print("KingFrogSacrifice");
            SceneManager.LoadScene("Good ending", LoadSceneMode.Single);
        });
    }
}

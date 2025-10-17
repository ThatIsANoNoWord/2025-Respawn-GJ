using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnterWin : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    TextMeshProUGUI timeText;
    TextMeshProUGUI hitText;
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var hitPlayer))
        {
            Time.timeScale = 0.0f;
            timeText.text = "Time: " + (int)Mathf.Floor(Mathf.Floor(time) / 60) + ":" + (int)(Mathf.Floor(time) % 60) + "." + (int)Mathf.Floor((time % 1) * 1000);
            hitText.text = "Hits taken: " + hitPlayer.HitCount;
            GetComponent<Animator>().Play("WinWon");
        }
    }

    public void DisablePlayer()
    {
        player.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

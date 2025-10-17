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
    [SerializeField]
    TextMeshProUGUI hitText;
    float time;
    int hits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0;
        hits = 69;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var hitPlayer))
        {
            Time.timeScale = 0.0f;
            hits = hitPlayer.HitCount;
            GetComponent<Animator>().Play("WinWinning");
        }
    }

    public void UpdateData()
    {
        string timeOf = "Time: " + (int)Mathf.Floor(Mathf.Floor(time) / 60) + ":" + (int)(Mathf.Floor(time) % 60) + ".";
        int millis = (int)Mathf.Floor((time % 1) * 1000);
        switch (millis)
        {
            case >= 100:
                timeOf += millis;
                break;
            case >= 10:
                timeOf += "0" + millis;
                break;
            default:
                timeOf += "00" + millis;
                break;
        }
        timeText.text = timeOf;
        hitText.text = "Hits taken: " + hits;
    }

    public void DisablePlayer()
    {
        player.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
}

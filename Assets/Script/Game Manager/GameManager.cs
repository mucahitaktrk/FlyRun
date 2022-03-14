using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject = null;
    [SerializeField] private List<GameObject> fly = null;
    [SerializeField] private GameObject flyObject = null;


    private float _move = -1.2f;
    private float _lastFrameFingerPostionX;
    private float _moveFactorX;
    private int length = 0;

    [SerializeField] private float distance;
    [SerializeField] private float boundrey = 0;

    private Rigidbody playerRigidbody;
    private PlayerColliderScript playerColliderScript;
    [SerializeField] private TextMeshPro flyText = null;
    [SerializeField] private GameObject finishSlamp = null;
    [SerializeField] private GameObject[] levels = null;
    [SerializeField] private GameObject[] finishPanel = null;

    private int level;

    private float timer = 0;
    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");
        if (level > levels.Length)
        {
            level = 0;
        }
        levels[level].SetActive(true);
    }
    private void Awake()
    {
        playerColliderScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerColliderScript>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
        length = fly.Count;
        timer = Time.time;
    }

    private void FixedUpdate()
    {

        int count = fly.Count;
        Time.timeScale = 1f;
        flyText.text = count.ToString();
        if (fly.Count >= 1)
        {

            if (playerColliderScript.flyPlus)
            {
                int q = int.Parse(playerColliderScript.value);
                length += q;
                for (int i = 1; i <= q; i++)
                {
                    FlyInstantiate();
                    playerColliderScript.flyPlus = false;
                }
                return;
            }
            else if (playerColliderScript.flyMinus)
            {
                int q = int.Parse(playerColliderScript.value);
                length -= q;
                for (int i = 1; i <= q; i++)
                {
                    Destroy(fly[i]);
                    playerColliderScript.flyMinus = false;
                }
                return;
            }
            else if (playerColliderScript.flyMultiple)
            {
                int q = int.Parse(playerColliderScript.value);
                length *= q;
                int carpim = q * count;
                int socun = carpim - count;
                for (int i = 1; i <= socun; i++)
                {
                    FlyInstantiate();
                    playerColliderScript.flyMultiple = false;
                }
                return;
            }
            else if (playerColliderScript.col)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Destroy(fly[i]);
                    fly.RemoveAt(i);
                    playerColliderScript.col = false;
                }
                return;
            }
            else if (playerColliderScript.finish)
            {
                Finish();
            }
            Speed();
            InputSystem();
            MoverSystem();
        }
        if (fly.Count <= 1 && Time.time > timer + 1f)
        {
            timer = Time.time;
            finishPanel[1].SetActive(true);
            Time.timeScale = 0f;
        }
        for (int i = 0; i < fly.Count; i++)
        {
            if (!fly[i])
            {
                fly.RemoveAt(i);
            }
        }
        if (level > 3)
        {
            level = 0;
        }
    }

    private void InputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPostionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            _moveFactorX = Input.mousePosition.x - _lastFrameFingerPostionX;
            _lastFrameFingerPostionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0;
        }

        float xBondrey = Mathf.Clamp(value: playerObject.transform.position.x, min: -boundrey, max: boundrey);
        playerObject.transform.position = new Vector3(xBondrey, playerObject.transform.position.y, playerObject.transform.position.z);
    }

    private void MoverSystem()
    {
        float swaerSystem = Time.fixedDeltaTime * _move * 0.8f * _moveFactorX;
        playerObject.transform.Translate(swaerSystem, 0, 0);
    }

    private void Speed()
    {
        playerRigidbody.velocity = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, 17);
    }

    public void FlyInstantiate()
    {
        fly.Add(Instantiate(flyObject, playerObject.transform.position, playerObject.transform.rotation, playerObject.transform.GetChild(0)));
        CircleSystem();
    }

    private void Finish()
    {
        for (int i = 0; i < fly.Count; i++)
        {
            timer = Time.time;
            Instantiate(finishSlamp, fly[i].transform.position, fly[i].transform.rotation);
            finishPanel[0].SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void NextButton()
    {
        level++;
        PlayerPrefs.SetInt("Level", level);
        levels[level--].SetActive(false);
        levels[level].SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        timer = Time.time;
    }
    public void AddNextButton()
    {
        level++;
        PlayerPrefs.SetInt("Level", level);
        levels[level--].SetActive(false);
        levels[level].SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        timer = Time.time;
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;

    }

    private void CircleSystem()
    {
        float angle = 1f;
        float flyCount = fly.Count;
        angle = 360 / flyCount;
        for (int i = 0; i < flyCount; i++)
        {
            MoveObject(fly[i].transform, i * angle);
        }
    }

    private void MoveObject(Transform objectTransform, float degree)
    {
        Vector3 vec = Vector3.zero;
        vec.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        vec.y = Mathf.Sin(degree * Mathf.Deg2Rad);
        objectTransform.localPosition = vec * distance;
    }
}

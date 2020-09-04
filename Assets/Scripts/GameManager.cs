using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    static GameManager instance;

    public GameObject platformPrefab;

    GameObject[] _platforms;
    float _currentOffsetY = 0;
    float _remainOffsetY = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _platforms = new GameObject[20];
        for(int i = 0; i < 20; i++)
        {
            _platforms[i] = Instantiate(platformPrefab);
        }
        Vector2 firstPos = PlayerController.Instance.transform.position;
        firstPos.y -= 0.7f;
        _currentOffsetY = firstPos.y;
        _platforms[0].transform.position = firstPos;
        for(int i = 1; i < 20; i++)
        {
            Vector2 pos = _platforms[i - 1].transform.position;
            pos.x = Random.Range(-2.3f, 2.3f);
            pos.y += Random.Range(1f, 2f);
            _platforms[i].transform.position = pos;
        }
    }

    void Update()
    {
        for(int i = 0; i < 20; i++)
        {
            if(_platforms[i].transform.position.y <= -6f)
            {
                Vector2 newPos = _platforms[(i + 19) % 20].transform.position;
                newPos.x = Random.Range(-2.3f, 2.3f);
                newPos.y += Random.Range(1f, 2f);
                _platforms[i].transform.position = newPos;
            }
        }
    }

    // 카메라를 1초에 걸쳐 움직임 (평범하게 발판을 밟아 올라갈 때)
    public void setCameraPosSlowly(float y)
    {
        if (y > _currentOffsetY)
        {
            _remainOffsetY += y - _currentOffsetY;
            StopAllCoroutines();
            StartCoroutine(MovePlatformsSlowly());
        }
    }

    // 카메라를 즉시즉시 움직임 (스프링 등 급발진할 때)
    public void setCameraPosFastly(float y)
    {
        _remainOffsetY += y - _currentOffsetY;
        StopAllCoroutines();
        MovePlatformFastly();
    }

    IEnumerator MovePlatformsSlowly()
    {
        float remainOffsetY = _remainOffsetY;
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 20; j++)
            {
                Vector2 newPlatformPos = _platforms[j].transform.position;
                newPlatformPos.y -= 0.05f * remainOffsetY;
                _platforms[j].transform.position = newPlatformPos;
            }

            Vector2 newPlayerPos = PlayerController.Instance.transform.position;
            newPlayerPos.y -= 0.05f * remainOffsetY;
            PlayerController.Instance.transform.position = newPlayerPos;

            _remainOffsetY -= 0.05f * remainOffsetY;
            yield return new WaitForSeconds(0.01f);
        }
        _remainOffsetY = 0;
    }

    void MovePlatformFastly()
    {
        for (int j = 0; j < 20; j++)
        {
            Vector2 newPlatformPos = _platforms[j].transform.position;
            newPlatformPos.y -= _remainOffsetY;
            _platforms[j].transform.position = newPlatformPos;
        }

        Vector2 newPlayerPos = PlayerController.Instance.transform.position;
        newPlayerPos.y -= _remainOffsetY;
        PlayerController.Instance.transform.position = newPlayerPos;

        _remainOffsetY = 0;
    }

}

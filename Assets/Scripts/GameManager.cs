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
    int _currLastIndex = 29;
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
        _platforms = new GameObject[30];
        for(int i = 0; i < 30; i++)
        {
            _platforms[i] = Instantiate(platformPrefab);
        }
        Vector2 firstPos = PlayerController.Instance.transform.position;
        firstPos.y -= 0.7f;
        _currentOffsetY = firstPos.y;
        _platforms[0].transform.position = firstPos;
        for(int i = 1; i < 30; i++)
        {
            Vector2 pos = _platforms[i - 1].transform.position;
            pos.x = Random.Range(-2.3f, 2.3f);
            pos.y += Random.Range(1f, 2f);
            _platforms[i].transform.position = pos;
        }
    }

    void Update()
    {
    }

    public void setCameraPos(float y)
    {
        _remainOffsetY += y - _currentOffsetY;
        StopAllCoroutines();
        StartCoroutine(movePlatforms());
    }

    IEnumerator movePlatforms()
    {
        float remainOffsetY = _remainOffsetY;
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 30; j++)
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

}

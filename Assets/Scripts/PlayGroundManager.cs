using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGroundManager : MonoBehaviour
{
    static public PlayGroundManager Instance;
    [SerializeField] PlayerController _playerController;
    [SerializeField] GameObject _spawnArea;

    [SerializeField] List<BallData> _ballDatas;
    [SerializeField] GameObject _ballPrefab;
    private List<Ball> _balls;
    private Ball _activeBall;
    public Ball ActiveBall { get { return _activeBall; } }

    [SerializeField] private float _timeBetweenSpawn = 5f;
    private float _spawnTimer;
    private bool _isSpawnRequired;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _balls = new List<Ball>();
        //TODO : eventuellement trier la liste balls data selon le Lvl 
        PlayerController.OnBallShooted += RequireSpawn;
    }

    private void Update()
    {
        if (_isSpawnRequired)
        {
            _spawnTimer -= Time.deltaTime;
        }

        if(_spawnTimer <= 0)
        {
            SpawnBall();
            _isSpawnRequired = false;
            _spawnTimer = _timeBetweenSpawn;
        }
    }

    private void RequireSpawn()
    {
        _isSpawnRequired = true;
    }


    private void SpawnBall()
    {
        var sphere = Instantiate(_ballPrefab);
        sphere.transform.position = _spawnArea.transform.position;
        var ball = sphere.GetComponent<Ball>();
        ball.Setup(_ballDatas[0]); //TODO : add random choice
        _activeBall = ball;
        _balls.Add(ball);
        _playerController.EnablePlayerInputs(true);
    }

    private void RemoveBall(int ballID)
    {
        //add ID to Ball class and use that to retrieve chosen ball and remove ot from list
    }

    private void OnDisable()
    {
        PlayerController.OnBallShooted -= RequireSpawn;
    }


}

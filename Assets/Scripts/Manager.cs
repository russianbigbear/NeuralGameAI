using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NeuralNetworkPlugin;

public class Manager : MonoBehaviour
{
    #region Variables

    // Объекты Prefabs
    public GameObject Player;
    public GameObject ObstacleSpikes;
    public GameObject ObstacleRock;
    public GameObject ObstacleCrate;
    public GameObject ObstacleSkulls;

    // Переременные счетчики 
    public int CountPlayers;
    public int DeadCount;
    public float ProbabilityChange;
    public float MaxDistance;
    public int Generation;
    public int IsSave;

    // Списки игроков
    public List<Player> LastPlayerList;
    public List<Player> PlayerList;

    #endregion

    #region Unity functions

    void Start()
    {
        LoadPlayerPrefs(); 
        InstantiatePlayers();
        StartCoroutine(GenerateObstacle());
    }

    public void FixedUpdate()
    {
        PlayerList.Sort(SortByPosition);
        PlayerList.Sort(SortByDistance);
        MaxDistance = Math.Max(PlayerList[0].Distance, MaxDistance);
        DisplayStats();

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    #endregion

    #region Generation management

    /// <summary>
    /// Игрок врезался.
    /// </summary>
    public void PlayerCrashed()
	{
		DeadCount++;

		if (DeadCount == CountPlayers)
			StartCoroutine(EndGeneration());     
	}

    /// <summary>
    /// Конец генерации.
    /// </summary>
    /// <returns></returns>
	IEnumerator EndGeneration()
	{
		yield return new WaitForSeconds(1.2f);
		StopAllCoroutines();
		CancelInvoke();
		DestroyObstacles();  
		DestroyAllPlayers();
        StartGeneration();
    }

    /// <summary>
    /// Старт генерации.
    /// </summary>
	void StartGeneration()
	{
		Generation++;
		CountPlayers = PlayerPrefs.GetInt("PlayersSize");
		DeadCount = 0;
		LastPlayerList = PlayerList;
		PlayerList = new List<Player>();
		InstantiatePlayers();
		StartCoroutine(GenerateObstacle());
	}

    /// <summary>
    /// Удаление всех препятствий.
    /// </summary>
	void DestroyObstacles()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            Destroy(obj);
    }

    /// <summary>
    /// Удаление всех игроков.
    /// </summary>
    void DestroyAllPlayers()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            Destroy(obj);
    }

    /// <summary>
    /// Соритровка по позиции.
    /// </summary>
    /// <param name="_FirstPlayer">Первый игрок</param>
    /// <param name="_SecondPlayer">Второй игрок</param>
    /// <returns></returns>
    public int SortByPosition(Player _FirstPlayer, Player _SecondPlayer)
    {
        return -(_FirstPlayer.transform.position.x.CompareTo(_SecondPlayer.transform.position.x));
    }

    /// <summary>
    /// Сортировка по дистанциию
    /// </summary>
    /// <param name="_FirstPlayer">Первый игрок</param>
    /// <param name="_SecondPlayer">Второй игрок</param>
    /// <returns></returns>
    public int SortByDistance(Player _FirstPlayer, Player _SecondPlayer)
    {
        return -(_FirstPlayer.Distance.CompareTo(_SecondPlayer.Distance));
    }

    /// <summary>
    /// Инизиализация игроков.
    /// </summary>
    public void InstantiatePlayers()
    {
        PlayerList = new List<Player>();

        for (int i = 0; i < CountPlayers; i++)
        {
            Player obj = Instantiate(Player, new Vector2(0, 0), Quaternion.identity).GetComponent<Player>();
            obj.transform.SetParent(GameObject.Find("InstantiatedPlayers").transform, false);
            PlayerList.Add(obj);

            if (Generation == 0)
                if(IsSave == 1)
                    PlayerList[i].SetBrain(new MultilayerPerceptron().LoadNeuralNetwork());
                else
                    PlayerList[i].SetBrain(new MultilayerPerceptron(new int[] { 6, 10, 10, 3 }));
            else
                ChangePopulation(i);
        }

        foreach(GameObject obj1 in GameObject.FindGameObjectsWithTag("Player"))
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
                Physics2D.IgnoreCollision(obj1.GetComponent<BoxCollider2D>(), obj2.GetComponent<BoxCollider2D>());          
    }

    /// <summary>
    /// Генерация нового игроков с мутациями.
    /// </summary>
    /// <param name="NumberPlayer">Номер в списке поколения</param>
    public void ChangePopulation(int NumberPlayer)
    {
        if (NumberPlayer == 0)
            PlayerList[NumberPlayer].SetBrain(LastPlayerList[0].PlayerBrain.UseTrainingSystem(0));
        else if (NumberPlayer < CountPlayers * 0.25f)
            PlayerList[NumberPlayer].SetBrain(LastPlayerList[NumberPlayer].PlayerBrain.UseTrainingSystem(ProbabilityChange));
        else if (NumberPlayer < CountPlayers * 0.50f)
            PlayerList[NumberPlayer].SetBrain(LastPlayerList[0].PlayerBrain.UseTrainingSystem(ProbabilityChange));
        else if (NumberPlayer < CountPlayers * 0.75f)
            PlayerList[NumberPlayer].SetBrain(LastPlayerList[0].PlayerBrain.UseTrainingSystem(ProbabilityChange * 0.5f));
        else
            PlayerList[NumberPlayer].SetBrain(LastPlayerList[0].PlayerBrain.UseTrainingSystem(ProbabilityChange * 1.5f));
    }

    #endregion

    #region Obstacles

    /// <summary>
    /// Интерфейс, который поддерживает простой перебор по универсальной коллекции. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateObstacle()
	{
		while (true)
		{
			InstantiateObstacle();
			yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
		}
	}

    /// <summary>
    /// 'Спавн' препятствия различной высоты.
    /// </summary>
	public void InstantiateObstacle()
	{
		float randomFloat = UnityEngine.Random.Range(0.05f, 0.45f);
		float bottomHeight = (1 - randomFloat) * Screen.height;
        GameObject Obstacle;

        switch (UnityEngine.Random.Range(1, 5))
        {
            case 1: {
                    Obstacle = Instantiate(ObstacleSpikes, new Vector2(0, (bottomHeight / 2)), Quaternion.identity);
                    Obstacle.GetComponent<RectTransform>().sizeDelta = new Vector2(88, bottomHeight);
                    Obstacle.GetComponent<BoxCollider2D>().size = Obstacle.GetComponent<RectTransform>().sizeDelta;
                    break;
                }
            case 2: { 
                    Obstacle = Instantiate(ObstacleRock, new Vector2(0, (bottomHeight / 2)), Quaternion.identity);
                    Obstacle.GetComponent<RectTransform>().sizeDelta = new Vector2(88, bottomHeight);
                    Obstacle.GetComponent<BoxCollider2D>().size = Obstacle.GetComponent<RectTransform>().sizeDelta;
                    break; 
                }
            case 3: { 
                    Obstacle = Instantiate(ObstacleCrate, new Vector2(0, (bottomHeight / 2)), Quaternion.identity);
                    Obstacle.GetComponent<RectTransform>().sizeDelta = new Vector2(88, bottomHeight);
                    Obstacle.GetComponent<BoxCollider2D>().size = Obstacle.GetComponent<RectTransform>().sizeDelta;
                    break; 
                }
            case 4: { 
                    Obstacle = Instantiate(ObstacleSkulls, new Vector2(0, (bottomHeight / 2)), Quaternion.identity); 
                    Obstacle.GetComponent<RectTransform>().sizeDelta = new Vector2(88, bottomHeight);
                    Obstacle.GetComponent<BoxCollider2D>().size = Obstacle.GetComponent<RectTransform>().sizeDelta;
                    break;
                }
            default: { Obstacle = Instantiate(ObstacleSpikes, new Vector2(0, (bottomHeight / 2)), Quaternion.identity); break; }
        }    

        Obstacle.transform.SetParent(GameObject.Find("InstantiatedObstacles").transform, false);
		Obstacle.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 150000000, ForceMode2D.Force);
	}

    #endregion


    #region Player saves

        /// <summary>
        /// Загрузка предыдущих значений слайдеров.
        /// </summary>
        void LoadPlayerPrefs()
    {
        CountPlayers = PlayerPrefs.GetInt("PlayersSize", 50);
        ProbabilityChange = PlayerPrefs.GetFloat("ProbabilityChange", 0.025f);
        IsSave = PlayerPrefs.GetInt("IsSave", 0);

        GameObject.Find("PlayersSlider").GetComponent<Slider>().value = CountPlayers;
        GameObject.Find("MutationSlider").GetComponent<Slider>().value = ProbabilityChange;
    }

    #endregion

    #region UI

    /// <summary>
    /// Вывод статистики по приложению.
    /// </summary>
    void DisplayStats()
    {
        GameObject.Find("Generation").GetComponent<Text>().text = "Поколение: " + Generation.ToString();
        GameObject.Find("Population").GetComponent<Text>().text = "Осталось игроков: " + (CountPlayers - DeadCount).ToString();
        GameObject.Find("MaxDistance").GetComponent<Text>().text = "Лучшее расстояние: " + Math.Round(MaxDistance, 1).ToString();
        GameObject.Find("CurrentBestDistance").GetComponent<Text>().text = "Текущее расстояние: " + Math.Round(PlayerList[0].Distance, 1).ToString();
    }

    public void ToggleTimeScale()
    {
        Time.timeScale = Math.Abs(Time.timeScale - 1) < 0.01f ? 10 : 1;
    }

    /// <summary>
    /// Рестарт приложения.
    /// </summary>
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Смена значения слайдера.
    /// </summary>
    /// <param name="value"></param>
    void OnPopulationSliderChanged(float value)
    {
        PlayerPrefs.SetInt("PlayersSize", (int)value);
        GameObject.Find("PlayersSlider").GetComponentInChildren<Text>().text = "Игроков: " + (int)value;

    }

    /// <summary>
    /// Смена значения слайдера.
    /// </summary>
    /// <param name="value"></param>
    void OnMutationSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("ProbabilityChange", value);
        GameObject.Find("MutationSlider").GetComponentInChildren<Text>().text = "Мутация: " + Math.Round(value * 100, 1) + "%";

    }

    /// <summary>
    /// Сохранение лучшего результата действия ИНС.
    /// </summary>
    public void Save()
    {
        if(PlayerList.Count > 0)
            PlayerList[0].PlayerBrain.SaveNeuralNetwork();
    }

    /// <summary>
    /// Загрузка сохранённого состояния. 
    /// </summary>
    public void Load()
    {
        PlayerPrefs.SetInt("IsSave", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Запуск работы с самого начала.
    /// </summary>
    public void New()
    {
        PlayerPrefs.SetInt("IsSave", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NeuralNetworkPlugin;

public class Player : MonoBehaviour
{
    #region Variables

    // Спрайты игрока
    public Sprite PlayerSprite;
    public Sprite PlayerFall;
    public Sprite[] PlayerJump;
    public Sprite[] PlayerRun;

    // Нейронная сеть персонажа (мозг)
    public MultilayerPerceptron PlayerBrain { get; set; }

    // Анимация персонажа
    private bool IsJumpAnimation { get; set; }
    private bool IsRunAnimation { get; set; }

    // Флаги персонажа
    private bool IsAlive { get; set; }
    private bool IsInitilized { get; set; }
    private bool IsGroundCheck { get; set; }
    private bool IsDebug { get; set; }

    // Дистанции лучей до препятствия
    List<float> RaysDistance;

    // Пройденная дистанция
    public float Distance;

    #endregion

    #region Unity functions

    void Start()
    {
        IsAlive = true;
    }

    void FixedUpdate()
    {
        if (IsAlive)
        {
            Distance += 0.01f;
            if (IsInitilized)
            {
                CalculateDistances();
                BrainThinking();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.gameObject.name != "Road")
        {
            IsAlive = false;
            FallDown();
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name == "Road")
            IsGroundCheck = true;
    }

    #endregion

    #region Player brains

    /// <summary>
    /// Присваивание нейронной сети пресонажу.
    /// </summary>
    /// <param name="_NeuralNetwork">Искусственная нейронная сеть</param>
    /// <param name="_IsDebug">Вывод отладки</param>
    public void SetBrain(MultilayerPerceptron _NeuralNetwork, bool _IsDebug = false)
    {
        PlayerBrain = _NeuralNetwork;
        Distance = 0f;
        IsDebug = _IsDebug;
        IsInitilized = true;
        IsGroundCheck = false;
    }

    /// <summary>
    /// Работа нейронной сети персонажа в игровом окружении.
    /// </summary>
    public void BrainThinking()
    {
        Layer output = PlayerBrain.WorkNeurons(RaysDistance, "Tanh");

        if(IsGroundCheck)
        {
            Run();
            if (output.Neurons[0].Value > 0)
            {
                Jump(output.Neurons[1].Value * 10);

                if (output.Neurons[2].Value > 0)
                    GetComponent<Rigidbody2D>().AddRelativeForce((Vector2.up + Vector2.right) * (output.Neurons[2].Value * 90), ForceMode2D.Impulse);
            }
        }    
    }

    /// <summary>
    /// Подсчет дистанций до препятствий. 
    /// </summary>
    void CalculateDistances()
    {
        // определяем направление относительно игрока
        List<Vector2> Vectors = new List<Vector2>
        {
            (transform.up + transform.right * 2) * 500,
            transform.right * 1000,
            (-transform.up + transform.right * 4) * 250,
            (-transform.up + transform.right * 2) * 500,
            (-transform.up) * 800,
            (-transform.up + -transform.right * 2) * 500
        };

        // отправляем лучи в заданые направления
        List<RaycastHit2D> Raycasts = new List<RaycastHit2D>
        {
            Physics2D.Raycast(transform.position, Vectors[0]),
            Physics2D.Raycast(transform.position, Vectors[1]),
            Physics2D.Raycast(transform.position, Vectors[2]),
            Physics2D.Raycast(transform.position, Vectors[3]),
            Physics2D.Raycast(transform.position, Vectors[4]),
            Physics2D.Raycast(transform.position, Vectors[5])
        };

        // Считаем расстояние между игроком и ближайшей точкой попадания луча
        // Нормализуем расстоняние от 0 до 1
        // Устанавливаем 1 если RaycastHit2D равен null
        RaysDistance = new List<float>
        {
            Raycasts[0] ? (Raycasts[0].distance / 1000) : 1,
            Raycasts[1] ? (Raycasts[1].distance / 1000) : 1,
            Raycasts[2] ? (Raycasts[2].distance / 1000) : 1,
            Raycasts[3] ? (Raycasts[3].distance / 1000) : 1,
            Raycasts[4] ? (Raycasts[4].distance / 785) : 1,
            Raycasts[5] ? (Raycasts[5].distance / 1000) : 1
        };

        // отрисовка лучей при отладке
        if (IsDebug)
            foreach (Vector2 vector in Vectors)
                Debug.DrawRay(transform.position, vector);    
    }

    #endregion

    #region Player jump

    /// <summary>
    /// Прыжок персонажа.
    /// </summary>
    /// <param name="_Push">Сила прыжка</param>
    public void Jump(float _Push)
    {
        IsGroundCheck = false;

        if (!IsJumpAnimation)
            StartCoroutine(JumpAnimation());

        if (_Push < 0)
            _Push = 1;
        else if (_Push > 1.25f)
            _Push = 1.25f;

        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 500 * _Push, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Анимация прыжка.
    /// </summary>
    /// <returns></returns>
    public IEnumerator JumpAnimation()
    {
        IsJumpAnimation = true;
        for (int i = 0; i < PlayerJump.Length; i++)
        {
            GetComponent<Image>().overrideSprite = PlayerJump[i];
            yield return new WaitForSeconds(0.09f);
        }
        GetComponent<Image>().overrideSprite = PlayerSprite;
        IsJumpAnimation = false;
    }

    #endregion

    #region Player run

    /// <summary>
    /// Бег персонажа.
    /// </summary>
    void Run()
    {
        if (!IsRunAnimation)
            StartCoroutine(RunAnimation());
    }

    /// <summary>
    /// Анимация бега.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunAnimation()
    {
        IsRunAnimation = true;
        for (int i = 0; i < PlayerRun.Length; i++)
        {
            GetComponent<Image>().overrideSprite = PlayerRun[i];
            yield return new WaitForSeconds(0.09f);
        }
        GetComponent<Image>().overrideSprite = PlayerSprite;
        IsRunAnimation = false;
    }

    #endregion

    #region Player fall

    /// <summary>
    /// Падение персонажа вниз.
    /// </summary>
    void FallDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().overrideSprite = PlayerFall;
        GetComponent<Rigidbody2D>().gravityScale = 200;
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * 500, ForceMode2D.Impulse);
        StopAllCoroutines();
        GameObject.Find("Canvas").GetComponent<Manager>().PlayerCrashed();
    }

    #endregion
}
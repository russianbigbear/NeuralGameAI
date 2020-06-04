using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NeuralNetworkPlugin;

public class Player : MonoBehaviour
{
    #region Variables

    // Анимация игрока
    private bool IsAlive { get; set; }
    private bool IsJumpAnimation { get; set; }
    private bool IsRunAnimation { get; set; }

    public Sprite PlayerSprite;
    public Sprite PlayerFall;
    public Sprite[] PlayerJump;
    public Sprite[] PlayerRun;

    // Нейронная сеть игрока (мозг)
    public MultilayerPerceptron PlayerBrain { get; set; }
    public float Distance;
    private bool IsInitilized;
    private bool IsGroundCheck { get; set; }
    private bool IsDebug { get; set; }

    // Дистанции лучей до препятствия
    float DistanceRayUpRight;
    float DistanceRayRight;
    float DistanceRayRightRightDown;
    float DistanceRayRightDown;
    float DistanceRayDown;
    float DistanceRayLeftDown;

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
    #endregion

    #region Neural network (brains player)

    public void SetBrain(MultilayerPerceptron _NeuralNetwork, bool _IsDebug = false)
    {
        PlayerBrain = _NeuralNetwork;
        IsDebug = _IsDebug;
        IsInitilized = true;
        IsGroundCheck = false;
        Distance = 0f;
    }

    public void BrainThinking()
    {
        // создаем входной массив
        float[] NeuralInputs = new float[6];

        // Устанавливаем дистанцию лучей видимости препятствия
        NeuralInputs[0] = DistanceRayUpRight;
        NeuralInputs[1] = DistanceRayRight;
        NeuralInputs[2] = DistanceRayRightRightDown;
        NeuralInputs[3] = DistanceRayRightDown;
        NeuralInputs[4] = DistanceRayDown;
        NeuralInputs[5] = DistanceRayLeftDown;

        // считаем выходные значения
        Layer output = PlayerBrain.WorkNeurons(NeuralInputs, "Tanh");

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

    void CalculateDistances()
    {
        // определяем направление относительно игрока
        Vector2 VectorUpRight = (transform.up + transform.right * 2) * 500;
        Vector2 VectorRight = transform.right * 1000;
        Vector2 VectorRightRightDown = (-transform.up + transform.right * 4) * 250;
        Vector2 VectorRightDown = (-transform.up + transform.right * 2) * 500;
        Vector2 VectorDown = (-transform.up) * 800;
        Vector2 VectorLeftDown = (-transform.up + -transform.right * 2) * 500;

        // отправляем лучи в заданые направления
        RaycastHit2D UpRight = Physics2D.Raycast(transform.position, VectorUpRight);
        RaycastHit2D Right = Physics2D.Raycast(transform.position, VectorRight);
        RaycastHit2D RightRightDown = Physics2D.Raycast(transform.position, VectorRightRightDown);
        RaycastHit2D RightDown = Physics2D.Raycast(transform.position, VectorRightDown);
        RaycastHit2D Down = Physics2D.Raycast(transform.position, VectorDown);
        RaycastHit2D LeftDown = Physics2D.Raycast(transform.position, VectorLeftDown);

        // Считаем расстояние между игроком и ближайшей точкой попадания луча
        // Нормализуем расстоняние от 0 до 1
        // Устанавливаем 1 если RaycastHit2D равен null
        DistanceRayUpRight = UpRight ? (UpRight.distance / 1000) : 1;
        DistanceRayRight = Right ? (Right.distance / 1000) : 1;
        DistanceRayRightRightDown = RightRightDown ? (RightRightDown.distance / 1000) : 1;
        DistanceRayRightDown = RightDown ? (RightDown.distance / 1000) : 1;
        DistanceRayDown = Down ? (Down.distance / 785) : 1;
        DistanceRayLeftDown = LeftDown ? (LeftDown.distance / 1000) : 1;

        if (IsDebug)
        {
            Debug.DrawRay(transform.position, VectorUpRight);
            Debug.DrawRay(transform.position, VectorRight);
            Debug.DrawRay(transform.position, VectorRightRightDown);
            Debug.DrawRay(transform.position, VectorRightDown);
            Debug.DrawRay(transform.position, VectorDown);
            Debug.DrawRay(transform.position, VectorLeftDown);
        }
    }

    #endregion

    #region Player jump

    public void Jump(float _Push)
    {
        IsGroundCheck = false;

        if (!IsJumpAnimation)
            StartCoroutine(JumpAnimation());

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        if (_Push < 0) _Push = 1;
        else if (_Push > 1.25f) _Push = 1.25f;

        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 500 * _Push, ForceMode2D.Impulse);
    }

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

    #region Player fall

    void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.gameObject.name != "Road")
        {
            IsAlive = false;
            FallDown();
            GameObject.Find("Canvas").GetComponent<Manager>().PlayerCrashed();
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {

        if (Collision.gameObject.name == "Road")
            IsGroundCheck = true;
    }

    void FallDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StopAllCoroutines();
        GetComponent<Image>().overrideSprite = PlayerFall;
        GetComponent<Rigidbody2D>().gravityScale = 200;
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * 500, ForceMode2D.Impulse);
    }

    #endregion

    #region Player run

    void Run()
    {
        if (!IsRunAnimation)
            StartCoroutine(RunAnimation());
    }

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


}




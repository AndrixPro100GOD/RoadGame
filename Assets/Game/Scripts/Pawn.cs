using UnityEngine;
using System.Collections;

[SelectionBase]
public class Pawn : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshPro playerNumberText;

    public int CurrentPosition { get; private set; }
    public int PlayerNumber { get; private set; }

    private Transform _gameCameraTransform;
    private Transform _textTransform;

    private void Awake()
    {
        _gameCameraTransform = Camera.main.transform;
        _textTransform = playerNumberText.transform;
    }

    public Vector3 Test = Vector3.up;

    private void FixedUpdate()
    {
        _textTransform.LookAt(_gameCameraTransform, Test);
    }

    public void SetPlayerNumber(int value)
    {
        PlayerNumber = value;
        playerNumberText.text = value.ToString();
    }

    public void SetPosition(int value)
    {
        CurrentPosition = value;
    }

    /*
     *
     *     [SerializeField]
    [Range(0.1f, 2f)]
    private float speed = 1f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string turnBoolName = "Turn";
    private IEnumerator GoGO()
    {
        t = 0;

        animator.SetTrigger(turnBoolName);

        while (t <= 1)
        {
            yield return null;

            t += Time.deltaTime * speed;

            transform.position = Vector3.Slerp(from.position, to.position, t);
        }
    }

    [NaughtyAttributes.Button("Turn")]
    private void MakeTurn()
    {
        StartCoroutine(GoGO());
    }
    */
}
using UnityEngine;
using Assets.Game.Scripts;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Dice : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private Rigidbody _rigidbody;

    [HideInInspector, SerializeField]
    private Collider _collider;

    [SerializeField]
    private Vector3Int directionValues;

    private Vector3Int _opposingDirectionValues;

    private readonly int[] FaceRepresent = new int[] { 0, 1, 2, 3, 4, 5, 6 };

    public Rigidbody GetRigidbody => _rigidbody;

    public Collider GetCollider => _collider;

    #region Init

    private void Reset()
    {
        InitMono();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        InitMono();
        CalculateOppositeDirSide();
    }

    [NaughtyAttributes.Button("Log side")]
    public void Test()
    {
        CalculateOppositeDirSide();
        Debug.Log(GetSide());
    }

#endif

    private void InitMono()
    {
        _collider = _collider == null ? GetComponent<Collider>() : _collider;
        _rigidbody = _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;
    }

    private void Init()
    {
        CalculateOppositeDirSide();
    }

    #endregion Init

    private void Awake()
    {
        InitMono();
        Init();
    }

    private void CalculateOppositeDirSide()
    {
        _opposingDirectionValues = 7 * Vector3Int.one - directionValues;
    }

    public DiceSide? GetSide()
    {
        if (Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
                                                                         //if ((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0) //Previously
        {
            if (Vector3.Dot(Vector3.up, transform.right) > 0)
            {
                return new DiceSide(FaceRepresent[directionValues.x], Vector3.right);
            }
            else
            {
                return new DiceSide(FaceRepresent[_opposingDirectionValues.x], -Vector3.right);
            }
        }
        else if (Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f) //y axis
        {
            if (Vector3.Dot(Vector3.up, transform.up) > 0)
            {
                return new DiceSide(FaceRepresent[directionValues.y], Vector3.up);
            }
            else
            {
                return new DiceSide(FaceRepresent[_opposingDirectionValues.y], -Vector3.up);
            }
        }
        else if (Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f) //z axis
        {
            if (Vector3.Dot(Vector3.up, transform.forward) > 0)
            {
                return new DiceSide(FaceRepresent[directionValues.z], Vector3.forward);
            }
            else
            {
                return new DiceSide(FaceRepresent[_opposingDirectionValues.z], -Vector3.forward);
            }
        }

        return null;
    }
}
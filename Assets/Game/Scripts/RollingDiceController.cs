using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Game.Scripts
{
    public class RollingDiceController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera rollingDiceCamera;

        [SerializeField]
        private Vector3 viewResultOffset = Vector3.forward * 1;

        [Header("Dice")]
        [SerializeField]
        private List<Dice> diceList = new();

        [SerializeField]
        private Transform startPlaceDicePosition;

        [Header("Animations")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private string idleAnimationTriggerName = "Idle";

        [SerializeField]
        private string rollAnimationTriggerName = "Roll";

        public event Action OnRollingDone;

        public IEnumerable<DiceSide>? GetLastRollingResult { get; private set; }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            PrepareToRoll();
        }

        public void StartRolling()
        {
            RestartRolling();
        }

        [NaughtyAttributes.Button("Reset rolling")]
        private void RestartRolling()
        {
            PrepareToRoll();

            diceList.ForEach(dice => MakeRandomDiceRotation(dice.transform));

            Roll();
        }

        #region Dice Rotation

        private void MakeRandomDiceRotation(in Transform dice)
        {
            dice.eulerAngles = new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle());
        }

        private float GetRandomAngle(float min = 0, float max = 361)
        {
            return UnityEngine.Random.Range(min, max);
        }

        #endregion Dice Rotation

        #region Animations

        private void PrepareToRoll()
        {
            //set dice to start position
            Vector3 dicePlace = startPlaceDicePosition.position;
            diceList.ForEach(dice => dice.transform.position = dicePlace);

            animator.SetTrigger(idleAnimationTriggerName);
        }

        private void Roll()
        {
            FreezeGamingDices(false);
            animator.SetTrigger(rollAnimationTriggerName);
        }

        public void ShowRollResult()//event animation callback
        {
            _ = StartCoroutine(WaitForResult());
        }

        #endregion Animations

        private IEnumerator WaitForResult()
        {
            //TODO: Cache WaitForSeconds and other YieldInstruction
            yield return new WaitForSeconds(2);
            yield return new UnityEngine.WaitUntil(() => diceList[0].GetRigidbody.velocity.magnitude < 0.01f);
            yield return new UnityEngine.WaitUntil(() => diceList[1].GetRigidbody.velocity.magnitude < 0.01f);
            yield return new WaitForSeconds(1);
            ShowDice();
        }

        private void ShowDice()
        {
            List<DiceSide> result = new();

            for (int i = 0; i < diceList.Count; i++)
            {
                DiceSide? diceSide = diceList[i].GetSide();

                if (diceSide == null)
                {
                    RestartRolling();
                    return;
                }

                result.Add((DiceSide)diceSide);
            }

            Vector3 leftPoint = GetShowToCameraResultPosition() + Vector3.forward;
            Vector3 rightPoint = GetShowToCameraResultPosition() + -Vector3.forward;

            FreezeGamingDices(true);

            float t = 0;

            diceList.ForEach(dice =>
                {
                    dice.transform.position = Vector3.Lerp(leftPoint, rightPoint, t += 0.33f);
                    RotateDiceToCamera(dice);
                }
            );

            GetLastRollingResult = result;
            OnRollingDone?.Invoke();
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Vector3 leftPoint = GetShowToCameraResultPosition() + Vector3.forward;
            Vector3 rightPoint = GetShowToCameraResultPosition() + -Vector3.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(leftPoint, rightPoint);
        }

#endif

        private void FreezeGamingDices(bool value)
        {
            diceList.ForEach(dice =>
            {
                dice.GetRigidbody.isKinematic = value;
                dice.GetRigidbody.velocity = Vector3.zero;
            });
        }

        private Vector3 GetShowToCameraResultPosition()
        {
            return rollingDiceCamera.transform.position + viewResultOffset;
        }

        private void RotateDiceToCamera(in Dice dice)
        {
            // направление от кубика до камеры
            Vector3 directionToCamera = rollingDiceCamera.transform.position - dice.transform.position;

            // определяем верхнюю грань кубика, что бы смотрела в сторону камеры
            Quaternion targetRotation = Quaternion.FromToRotation(dice.GetSide().Value.GetSide, directionToCamera);

            dice.transform.rotation = targetRotation;
        }
    }
}
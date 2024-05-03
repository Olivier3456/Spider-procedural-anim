using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProceduralAnimationPerso : MonoBehaviour
{
    [SerializeField] private Transform[] legsTargets;
    [SerializeField] private Transform[] targetPositions;
    [SerializeField] private Transform ground;

    [SerializeField] private float distanceThresholdToMoveLegs = 0.1f;
    //[SerializeField] private float legMoveLength = 0.5f;
    [SerializeField] private float spiderSpeed = 0.5f;

    private Vector3[] targetRealPositions = new Vector3[8];

    private bool[] areLegsCurrentlyMoving = new bool[8];


    void Start()
    {
        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        transform.position -= transform.forward * spiderSpeed * Time.deltaTime;


        //float maxDistanceFromLegToTargetRealPosition = 0f;
        //int legToMoveIndex = -1;

        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (Physics.Raycast(targetPositions[i].position, -transform.up, out RaycastHit hit))
            {
                targetRealPositions[i] = hit.point;

                Debug.DrawLine(legsTargets[i].position, targetRealPositions[i], Color.red);

                float distance = Vector3.Distance(legsTargets[i].position, targetRealPositions[i]);

                int previousLegIndex = i > 0 ? i - 1 : legsTargets.Length - 1;
                int nextLegIndex = i < legsTargets.Length - 1 ? i + 1 : 0;

                if (distance > distanceThresholdToMoveLegs &&
                    !areLegsCurrentlyMoving[i] &&
                    !areLegsCurrentlyMoving[previousLegIndex] &&
                    !areLegsCurrentlyMoving[nextLegIndex])
                {
                    StartCoroutine(ProceedMovement(i, targetRealPositions[i]));
                }
            }
        }
    }



    private IEnumerator ProceedMovement(int legIndex, Vector3 targetPosition)
    {
        float lerp = 0f;
        areLegsCurrentlyMoving[legIndex] = true;

        while (lerp <= 1)
        {
            yield return null;
            lerp += Time.deltaTime / (spiderSpeed * 0.5f);
            legsTargets[legIndex].position = Vector3.Lerp(legsTargets[legIndex].position, targetPosition, lerp);
        }

        legsTargets[legIndex].position = targetPosition;
        areLegsCurrentlyMoving[legIndex] = false;
    }





#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < targetRealPositions.Length; i++)
        {
            Gizmos.DrawWireSphere(targetRealPositions[i], 0.05f);
        }
    }
#endif
}

using UnityEngine;

namespace PathCreation.Examples
{

    public class PathSpawner : MonoBehaviour
    {

        public PathCreator pathPrefab;
        public PathFollower followerPrefab;
        public Transform[] spawnPoints;

        private void Start()
        {
            foreach (Transform t in spawnPoints)
            {
                PathCreator path = Instantiate(pathPrefab, t.position, t.rotation);
                PathFollower follower = Instantiate(followerPrefab);
                follower.pathCreator = path;
                path.transform.parent = t;

            }
        }
    }

}
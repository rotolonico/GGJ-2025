using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private const float spawnChance = 0.25f;

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private RoomHandler roomHandler;
        [SerializeField] private Transform player;

        private IEnumerator Start()
        {
            if (roomHandler.IsStartRoom() || roomHandler.IsBossRoom()) 
                yield break;

            var enemySpawnPositions = GetEnemySpawnPositions(roomHandler.GetRoomType());
            
            yield return new WaitUntil(() => player != null);

            foreach (var enemySpawnPosition in enemySpawnPositions)
            {
                if (roomHandler.GetRoomType() != RoomHandler.RoomType.ITEM_ROOM && UnityEngine.Random.value > spawnChance) continue;

                var enemyPosition = new Vector3(
                    enemySpawnPosition.x * RoomHandler.RoomSize.x,
                    enemySpawnPosition.y * RoomHandler.RoomSize.y,
                    0
                );
                var enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
                enemy.transform.SetParent(transform, false);
                enemy.GetComponent<EnemyBehaviour>().SetTarget(player);

            }
        }

        public void SetPlayer(Transform transform)
        {
            player = transform;
        }

        private Vector2[] GetEnemySpawnPositions(RoomHandler.RoomType roomType)
        {
            switch (roomType)
            {
                case RoomHandler.RoomType.SIMPLE_ROOM:
                    return new[]
                    {
                        new Vector2(0f, 0f),
                        new Vector2(0.25f, -0.25f),
                        new Vector2(0.25f, 0.25f),
                        new Vector2(-0.25f, -0.25f),
                        new Vector2(-0.25f, 0.25f)
                    };
                case RoomHandler.RoomType.ITEM_ROOM:
                    return new[]
                    {
                        new Vector2(0f, 0f)
                    };
                case RoomHandler.RoomType.PUZZLE_ROOM:
                    return Array.Empty<Vector2>();
                default:
                    return Array.Empty<Vector2>();
            }
        }
    }
}
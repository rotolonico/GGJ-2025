using System;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private const float spawnChance = 0.25f;
        
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private RoomHandler roomHandler;


        private void Start()
        {
            GenerateEnemies();
        }

        private void GenerateEnemies()
        {
            if (roomHandler.IsStartRoom() || roomHandler.IsBossRoom()) return;
            
            var enemySpawnPositions = GetEnemySpawnPositions(roomHandler.GetRoomType());
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
                
            }
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
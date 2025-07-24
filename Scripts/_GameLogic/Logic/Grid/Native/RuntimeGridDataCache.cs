using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Grid.Currencies;
using _Game.Scripts.Helper.Extensions.System;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public static class RuntimeGridDataCache
    {
        private static GridTile[,] _gridArray;
        private static readonly Dictionary<GridObjectProductDataContainer.ProductType, List<Product>> Products = new();

        public static void SetGridArray(GridTile[,] gridArray)
        {
            _gridArray = gridArray;
        }

        public static GridTile[,] GetGridArray()
        {
            return _gridArray;
        }

        public static void AddProduct(GridObjectProductDataContainer.ProductType productType, Product product)
        {
            if (!Products.ContainsKey(productType))
            {
                Products[productType] = new List<Product>();
            }

            Products[productType].Add(product);
        }

        public static List<Product> GetAllProductsInType(GridObjectProductDataContainer.ProductType productType)
        {
            return Products.TryGetValue(productType, out var products) ? products : new List<Product>();
        }
        
        public static void RemoveProduct(GridObjectProductDataContainer.ProductType productType)
        {
            Products.Remove(productType);
        }

        public static GridTile GetTileAtPosition(Vector2Int position)
        {
            if (IsValidPosition(position))
            {
                return _gridArray[position.x, position.y];
            }
            return null; 
        }

        private static bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < _gridArray.GetLength(0) &&
                   position.y >= 0 && position.y < _gridArray.GetLength(1);
        }

        public static List<Vector2Int> GetAvailablePositions(GridTile[,] gridArray)
        {
            var positions = new List<Vector2Int>();
            for (var x = 0; x < gridArray.GetLength(0); x++)
            for (var y = 0; y < gridArray.GetLength(1); y++)
                if (gridArray[x, y].IsEmpty)
                    positions.Add(new Vector2Int(x, y));
            return positions;
        }
        
        public static GridTile FindEmptyRandomTile()
        {
            List<GridTile> emptyTiles = GetAvailablePositions(_gridArray)
                .Select(GetTileAtPosition)
                .Where(tile => tile.IsEmpty)
                .ToList();

            if (emptyTiles.Count != 0) return emptyTiles[Random.Range(0, emptyTiles.Count)];
                TDebug.LogWarning("No empty tiles found.");
            return null;

        }
        
        private static List<Vector2Int> GetNeighboringPositions(Vector2Int position)
        {
            var neighbors = new List<Vector2Int>
            {
                new(position.x + 1, position.y), 
                new(position.x - 1, position.y), 
                new(position.x, position.y + 1), 
                new(position.x, position.y - 1)  
            };

            return neighbors;
        }

        public static List<GridTile> FindEmptyNeighborTiles(Vector2Int position, int requiredTiles)
        {
            var emptyTiles = new List<GridTile>();
            var availablePositions = GetAvailablePositions(_gridArray);

            var centerTile = GetTileAtPosition(position);
            if (centerTile != null && centerTile.IsEmpty)
            {
                emptyTiles.Add(centerTile);
            }

            foreach (var tile in from pos in availablePositions where Vector2Int.Distance(pos, position) <= 1 && pos != position select GetTileAtPosition(pos))
            {
                if (tile != null && tile.IsEmpty)
                    emptyTiles.Add(tile);

                if (emptyTiles.Count >= requiredTiles)
                    break;
            }

            return emptyTiles;
        }
        
        public static List<GridTile> FindEmptyNeighborTilesFromSource(GridTile source)
        {
            var emptyTiles = new List<GridTile>();
            var visited = new HashSet<Vector2Int>(); 
            var queue = new Queue<GridTile>(); 
            queue.Enqueue(source); 
            visited.Add(source.GridPosition); 

            while (queue.Count > 0)
            {
                var currentTile = queue.Dequeue();
                var neighbors = GetNeighboringPositions(currentTile.GridPosition);

                foreach (var neighborPos in neighbors)
                {
                    if (visited.Contains(neighborPos)) continue;
                    var neighborTile = GetTileAtPosition(neighborPos);
                    if (neighborTile == null) continue;
                    visited.Add(neighborPos);

                    if (neighborTile.IsEmpty)
                    {
                        emptyTiles.Add(neighborTile);

                        if (emptyTiles.Count >= 3)
                        {
                            return emptyTiles;
                        }
                    }
                    else
                    {
                        queue.Enqueue(neighborTile);
                    }
                }
            }

            return emptyTiles; 
        }
        
        public static void DisableAllTilesHighlight(GridTile[,] gridArray)
        {
            foreach (var tile in gridArray)
            {
                tile.HighlightObject.SetActive(false);
            }
        }
    }
}
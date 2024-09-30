using System.Collections.Generic;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;

public interface INeighborFinder
{
    public delegate float GetDistanceDelegate(Cell from, Cell to);
    public IEnumerable<CellCostPair> FindNeighbors(Cell sourceCell, bool hasJumpCapability, GetDistanceDelegate dist);
}
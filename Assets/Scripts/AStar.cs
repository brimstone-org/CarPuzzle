using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    public static AStar instance { get; private set; }

    public PathFinder finder { get; private set; }

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        finder = new PathFinder();
    }

    void Start() {
        //finder.Find(2, 5, 9, 8);
    }

    public class Node {
        public BaseTile tile { get; set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F { get { return this.G + this.H; } }
        public NodeState state { get; set; }
        public Node parentNode { get; set; }

        public Node() {
            G = 0;
            state = NodeState.Untested;
            parentNode = null;
        }

        public Node(Node parent) {
            G = parent.G + 1;
            state = NodeState.Untested;
            parentNode = parentNode;
        }
        
        public void CalculateHeuristic(Node target) {
            H = Mathf.Abs(target.tile.x - tile.x) + Mathf.Abs(target.tile.y - tile.y);
            //Debug.Log("Heurisitc " + H);
        }

        public void CalculateG(Node parent) {
            G = parent.G + 1;
        }

    }

    public enum NodeState { Untested, Open, Closed };

    public class PathFinder {
        List<Node> openList;
        List<Node> closedList;

        Node[,] nodes;

		public List<BaseTile> Find(int startX, int startY, int targetX, int targetY, int unusableX, int unusableY) {
            startX = Mathf.Abs(startX);
            startY = Mathf.Abs(startY);
            targetX = Mathf.Abs(targetX);
            targetY = Mathf.Abs(targetY);

            List<BaseTile> path = new List<BaseTile>();

			if (IsInGrid (unusableX, unusableY)) {
				//Debug.Log (unusableX + " " + unusableY);
				MapHolder.instance.map [unusableX, unusableY].Passable = false;
			}

            nodes = new Node[MapHolder.instance.sizeX, MapHolder.instance.sizeY];
            for(int i=0; i<MapHolder.instance.sizeX; i++) {
                for(int j=0; j<MapHolder.instance.sizeY; j++) {
                    nodes[i, j] = new Node();
                    nodes[i, j].tile = MapHolder.instance.map[i, j];
                }
            }

            if (!IsInGrid(targetX, targetY))
                return path;

            //Debug.Log(startX + " " + startY);
            Node currentNode = nodes[startX, startY];
            Node targetNode = nodes[targetX, targetY];

            openList = new List<Node>();
            openList.Add(currentNode);
            closedList = new List<Node>();

            while (currentNode != targetNode) {

                currentNode.state = NodeState.Closed;
                closedList.Add(currentNode);
                openList.Remove(currentNode);

                List<Node> adjacent = FindAdjacentNodes(currentNode);
                for (int i = 0; i < adjacent.Count; i++) {
                    adjacent[i].CalculateHeuristic(targetNode);
                    if (adjacent[i].state != NodeState.Open) {
                        openList.Add(adjacent[i]);
                        adjacent[i].parentNode = currentNode;
                        adjacent[i].CalculateG(currentNode);
                        adjacent[i].state = NodeState.Open;
                    } else {
                        if(currentNode.G + 1 < adjacent[i].G) {
                            adjacent[i].parentNode = currentNode;
                        }
                    }
                }

				if (openList.Count == 0) {
					if (IsInGrid (unusableX, unusableY)) {
						Debug.Log (unusableX + " " + unusableY);
						MapHolder.instance.map [unusableX, unusableY].Passable = true;
					}
					return path;
				}

                float min = openList[0].F;
                currentNode = openList[0];
                for(int i=1; i<openList.Count; i++) {
                    if(openList[i].F < min) {
                        min = openList[i].F;
                        currentNode = openList[i];
                    }
                }

            }

            //Debug.Log(currentNode.tile);
            
            DFS(currentNode, path);
			if (IsInGrid (unusableX, unusableY)) {
				Debug.Log (unusableX + " " + unusableY);
				MapHolder.instance.map [unusableX, unusableY].Passable = true;
			}
            return path;

        }

        private void DFS(Node node, List<BaseTile> path) {
            if (node.parentNode == null) {
                path.Add(node.tile);
                return;
            }
            DFS(node.parentNode, path);
            path.Add(node.tile);
        }

        private List<Node> FindAdjacentNodes(Node node) {
            List<Node> nodes = new List<Node>();

            int x = node.tile.x;
            int y = node.tile.y;

            if (IsInGrid(x + 1, y) && MapHolder.instance.map[x + 1, y].Passable) {
                Node adj = this.nodes[x + 1, y];
                if(adj.state != NodeState.Closed)
                    nodes.Add(adj);
            }
            if (IsInGrid(x - 1, y) && MapHolder.instance.map[x - 1, y].Passable) {
                Node adj = this.nodes[x - 1, y];
                if (adj.state != NodeState.Closed)
                    nodes.Add(adj);
            }
            if (IsInGrid(x, y + 1) && MapHolder.instance.map[x, y + 1].Passable) {
                Node adj = this.nodes[x, y + 1];
                if (adj.state != NodeState.Closed)
                    nodes.Add(adj);
            }
            if (IsInGrid(x, y - 1) && MapHolder.instance.map[x, y - 1].Passable) {
                Node adj = this.nodes[x, y - 1];
                if (adj.state != NodeState.Closed)
                    nodes.Add(adj);
            }

            return nodes;
        }

        private bool IsInGrid(int x, int y) {
            bool result = true;
            if (x < 0 || x >= MapHolder.instance.sizeX || y < 0 || y >= MapHolder.instance.sizeY)
                result = false;
            return result;
        }

    }
}

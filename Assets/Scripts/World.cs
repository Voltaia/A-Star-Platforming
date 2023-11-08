using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// General
	private List<Platform> paths = new List<Platform>();

	private void Start()
	{
		IndexedPriorityQueue priorityQueue = new IndexedPriorityQueue();
		priorityQueue.Push('A', 60);
		priorityQueue.Push('B', 40);
		priorityQueue.Push('C', 50);
		priorityQueue.Push('D', 20);
		priorityQueue.Push('E', 30);
		priorityQueue.Push('F', 70);
		priorityQueue.Pop();
		priorityQueue.Pop();
		priorityQueue.Push('G', 10);
		Debug.Log(priorityQueue);
	}
}

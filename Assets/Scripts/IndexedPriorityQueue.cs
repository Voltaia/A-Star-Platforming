using System;
using System.Collections.Generic;
using UnityEngine;

public class IndexedPriorityQueue
{
	// General
	private Dictionary<object, int> indexes = new Dictionary<object, int>();
	private Dictionary<object, float> priorities = new Dictionary<object, float>();
	private Dictionary<int, object> keys = new Dictionary<int, object>();
	private int count = 0;

	// Push onto binary heap
	public void Push(object key, float priority)
	{
		// Push element to last index and then reorder it up
		count++;
		int lastIndex = count - 1;
		indexes[key] = lastIndex;
		keys[lastIndex] = key;
		priorities[key] = priority;
		ReorderUp(lastIndex);
	}

	// Pop off of binary heap
	public object Pop()
	{
		// If we are out of keys, return null
		if (count <= 0) return null;

		// Pop it
		count--;
		object poppedKey = keys[0];
		indexes.Remove(poppedKey);
		priorities.Remove(poppedKey);

		// Put the lowest key in the popped key's place and reorder down
		if (count > 0)
		{
			// Get replacement key
			int lastIndex = count;
			object newRootKey = keys[lastIndex];
			keys[0] = newRootKey;
			indexes[newRootKey] = 0;
			keys.Remove(lastIndex);
			ReorderDown(0);
		}

		// Return
		return poppedKey;
	}
	
	// Get above index in binary heap
	public int GetParentIndex(int childIndex)
	{
		return (childIndex - 1) % 2;
	}

	// Get lower left index in binary heap
	public int GetLeftChildIndex(int parentIndex)
	{
		return 2 * parentIndex + 1;
	}

	// Get lower right index in binary heap
	public int GetRightChildIndex(int parentIndex)
	{
		return 2 * parentIndex + 2;
	}

	// Reorder element up the binary heap
	public void ReorderUp(int index)
	{
		// Return if we have reached the top
		if (index == 0) return;

		// Get the above key
		object key = keys[index];
		int parentIndex = GetParentIndex(index);
		object parentKey = keys[parentIndex];

		// Check if current key is smaller than parent key
		if (priorities[key] < priorities[parentKey])
		{
			// Swap parent and child
			indexes[key] = parentIndex;
			keys[index] = parentKey;
			indexes[parentKey] = index;
			keys[parentIndex] = key;

			// Recurse
			ReorderUp(parentIndex);
		}
	}

	// Reorder element down in the binary heap
	public void ReorderDown(int index)
	{
		// Get the left and right indexes
		object key = keys[index];
		int leftIndex = GetLeftChildIndex(index);
		int rightIndex = GetRightChildIndex(index);

		// Check that we are within bounds
		if (leftIndex < count)
		{
			object leftKey = keys[leftIndex];
			if (rightIndex < count)
			{
				object rightKey = keys[rightIndex];

				// Determine if the left or right element is smaller
				object swapKey;
				if (priorities[leftKey] < priorities[rightKey]) swapKey = leftKey;
				else swapKey = rightKey;

				// If the smaller child is smaller than the parent, swap them
				if (priorities[key] > priorities[swapKey])
				{
					// Swap child and parent
					int swapIndex = indexes[swapKey];
					indexes[key] = swapIndex;
					keys[index] = swapKey;
					indexes[swapKey] = index;
					keys[swapIndex] = key;

					// Recurse
					ReorderDown(swapIndex);
				}
			}
		}
	}

	// Convert to string
	public override string ToString()
	{
		string keysString = "Keys Dictionary: ";
		foreach (KeyValuePair<int, object> pair in keys)
		{
			keysString += $"[{pair.Key}: {pair.Value}]";
		}
		string indexesString = "Indexes Dictionary: ";
		foreach (KeyValuePair<object, int> pair in indexes)
		{
			indexesString += $"[{pair.Key}: {pair.Value}]";
		}
		return keysString + " " + indexesString;
	}
}
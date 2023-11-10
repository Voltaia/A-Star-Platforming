using System;
using System.Collections.Generic;
using UnityEngine;

public class IndexedPriorityQueue<KeyType>
{
	// General
	private Dictionary<KeyType, int> indexes = new Dictionary<KeyType, int>();
	private Dictionary<KeyType, float> priorities = new Dictionary<KeyType, float>();
	private Dictionary<int, KeyType> keys = new Dictionary<int, KeyType>();
	public int Count { get; private set; } = 0;

	// Push onto binary heap
	public void Push(KeyType key, float priority)
	{
		// Push element to last index and then reorder it up
		Count++;
		int lastIndex = Count - 1;
		indexes[key] = lastIndex;
		keys[lastIndex] = key;
		priorities[key] = priority;
		ReorderUp(lastIndex);
	}

	// Pop off of binary heap
	public object Pop()
	{
		// If we are out of keys, return null
		if (Count <= 0) return null;

		// Pop it
		Count--;
		KeyType poppedKey = keys[0];
		indexes.Remove(poppedKey);
		priorities.Remove(poppedKey);

		// Put the lowest key in the popped key's place and reorder down
		if (Count > 0)
		{
			// Get replacement key
			int lastIndex = Count;
			KeyType newRootKey = keys[lastIndex];
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
		KeyType key = keys[index];
		int parentIndex = GetParentIndex(index);
		KeyType parentKey = keys[parentIndex];

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
		KeyType key = keys[index];
		int leftIndex = GetLeftChildIndex(index);
		int rightIndex = GetRightChildIndex(index);

		// Check that we are within bounds
		if (leftIndex < Count)
		{
			KeyType leftKey = keys[leftIndex];
			if (rightIndex < Count)
			{
				KeyType rightKey = keys[rightIndex];

				// Determine if the left or right element is smaller
				KeyType swapKey;
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

	// Increase priority of key
	public void IncreasePriority(KeyType key, float priority)
	{
		if (priority <= priorities[key]) return;
		priorities[key] = priority;
		ReorderUp(indexes[key]);
	}

	// Decrease priority of key
	public void DecreasePriority(KeyType key, float priority)
	{
		if (priority >= priorities[key]) return;
		priorities[key] = priority;
		ReorderDown(indexes[key]);
	}

	// Check if key is in binary heap
	public bool ContainsKey(KeyType key)
	{
		return indexes.ContainsKey(key);
	}

	// Convert to string
	public override string ToString()
	{
		string keysString = "Keys Dictionary: ";
		foreach (KeyValuePair<int, KeyType> pair in keys)
		{
			keysString += $"[{pair.Key}: {pair.Value}]";
		}
		string indexesString = "Indexes Dictionary: ";
		foreach (KeyValuePair<KeyType, int> pair in indexes)
		{
			indexesString += $"[{pair.Key}: {pair.Value}]";
		}
		return keysString + " " + indexesString;
	}
}
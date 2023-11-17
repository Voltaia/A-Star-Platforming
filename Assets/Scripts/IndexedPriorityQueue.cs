using System;
using System.Collections.Generic;
using UnityEngine;

public class IndexedPriorityQueue<Key>
{
	// General
	private Dictionary<Key, int> _indexes = new Dictionary<Key, int>();
	private Dictionary<Key, float> _priorities = new Dictionary<Key, float>();
	private Dictionary<int, Key> _keys = new Dictionary<int, Key>();
	public int Count { get; private set; } = 0;

	// Push onto binary heap
	public void Push(Key key, float priority)
	{
		// Push element to last index and then reorder it up
		Count++;
		int lastIndex = Count - 1;
		_indexes[key] = lastIndex;
		_keys[lastIndex] = key;
		_priorities[key] = priority;
		ReorderUp(lastIndex);
	}

	// Pop off of binary heap
	public object Pop()
	{
		// If we are out of keys, return null
		if (Count <= 0) return null;

		// Pop it
		Count--;
		Key poppedKey = _keys[0];
		_indexes.Remove(poppedKey);
		_priorities.Remove(poppedKey);

		// Put the lowest key in the popped key's place and reorder down
		if (Count > 0)
		{
			// Get replacement key
			int lastIndex = Count;
			Key newRootKey = _keys[lastIndex];
			_keys[0] = newRootKey;
			_indexes[newRootKey] = 0;
			_keys.Remove(lastIndex);
			ReorderDown(0);
		}

		// Return
		return poppedKey;
	}
	
	// Get above index in binary heap
	public int GetParentIndex(int childIndex)
	{
		return (childIndex - 1) / 2;
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
		Key key = _keys[index];
		int parentIndex = GetParentIndex(index);
		Key parentKey = _keys[parentIndex];

		// Check if current key is smaller than parent key
		if (_priorities[key] < _priorities[parentKey])
		{
			// Swap parent and child
			_indexes[key] = parentIndex;
			_keys[index] = parentKey;
			_indexes[parentKey] = index;
			_keys[parentIndex] = key;

			// Recurse
			ReorderUp(parentIndex);
		}
	}

	// Reorder element down in the binary heap
	public void ReorderDown(int index)
	{
		// Get the left and right indexes
		Key key = _keys[index];
		int leftIndex = GetLeftChildIndex(index);
		int rightIndex = GetRightChildIndex(index);

		// Check that we are within bounds
		Key swapKey;
		if (leftIndex < Count)
		{
			// Replace with right key if lower
			Key leftKey = _keys[leftIndex];
			swapKey = leftKey;
			if (rightIndex < Count)
			{
				Key rightKey = _keys[rightIndex];
				if (_priorities[rightKey] < _priorities[leftKey]) swapKey = rightKey;
			}

			// If the smaller child is smaller than the parent, swap them
			if (_priorities[key] > _priorities[swapKey])
			{
				// Swap child and parent
				int swapIndex = _indexes[swapKey];
				_indexes[key] = swapIndex;
				_keys[index] = swapKey;
				_indexes[swapKey] = index;
				_keys[swapIndex] = key;

				// Recurse
				ReorderDown(swapIndex);
			}
		}
	}

	// Increase priority of key
	public void IncreasePriority(Key key, float priority)
	{
		if (priority <= _priorities[key]) return;
		_priorities[key] = priority;
		ReorderUp(_indexes[key]);
	}

	// Decrease priority of key
	public void DecreasePriority(Key key, float priority)
	{
		if (priority >= _priorities[key]) return;
		_priorities[key] = priority;
		ReorderDown(_indexes[key]);
	}

	// Check if key is in binary heap
	public bool ContainsKey(Key key)
	{
		return _indexes.ContainsKey(key);
	}

	// Convert to string
	public override string ToString()
	{
		string keysString = "Keys Dictionary: ";
		foreach (KeyValuePair<int, Key> pair in _keys)
		{
			keysString += $"[{pair.Key}: {pair.Value}]";
		}
		string indexesString = "Indexes Dictionary: ";
		foreach (KeyValuePair<Key, int> pair in _indexes)
		{
			indexesString += $"[{pair.Key}: {pair.Value}]";
		}
		return keysString + " " + indexesString;
	}
}
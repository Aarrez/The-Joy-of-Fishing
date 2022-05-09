using System;
using System.Collections.Generic;

namespace Stem
{
	public static class LocalID
	{
		public const int None = 0;
	}

	internal class LocalIDStorageRuntime<T>
	{
		private Dictionary<int, T> items = new Dictionary<int, T>();
		private Random random = new Random();

		internal void Clear()
		{
			items.Clear();
		}

		internal bool Contains(int id)
		{
			return items.ContainsKey(id);
		}

		internal T Get(int id)
		{
			if (id == LocalID.None)
				return default(T);

			T result = default(T);
			items.TryGetValue(id, out result);

			return result;
		}

		internal int Add(int id, T item)
		{
			if (id == LocalID.None)
				id = GetUniqueID();

			T existingItem = default(T);
			items.TryGetValue(id, out existingItem);

			// regular insertion
			if (existingItem == null)
			{
				items.Add(id, item);
				return id;
			}

			// id collision
			if (existingItem.Equals(item))
			{
				id = GetUniqueID();
				items.Add(id, item);
			}

			return id;
		}

		internal void Remove(int id)
		{
			items.Remove(id);
		}

		private int GetUniqueID()
		{
			int id = random.Next();

			while(id == LocalID.None || items.ContainsKey(id))
				id = random.Next();

			return id;
		}
	}
}

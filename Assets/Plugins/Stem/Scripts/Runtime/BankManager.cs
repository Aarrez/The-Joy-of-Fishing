using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Stem
{
	internal interface IManagerRuntime<T> where T : ScriptableObject
	{
		void Init(T bank);
	}

	internal class BankManager<BankType, RuntimeType>
		where RuntimeType : MonoBehaviour, IManagerRuntime<BankType>
		where BankType : ScriptableObject
	{
		private List<BankType> banks = new List<BankType>();
		private ReadOnlyCollection<BankType> banksRO = null;

		private List<RuntimeType> bankRuntimes = new List<RuntimeType>();
		private ReadOnlyCollection<RuntimeType> bankRuntimesRO = null;

		private List<GameObject> bankGameObjects = new List<GameObject>();
		private int primaryIndex = -1;

		internal ReadOnlyCollection<BankType> Banks
		{
			get
			{
				if (banksRO == null)
					banksRO = banks.AsReadOnly();

				return banksRO;
			}
		}

		internal ReadOnlyCollection<RuntimeType> Runtimes
		{
			get
			{
				if (bankRuntimesRO == null)
					bankRuntimesRO = bankRuntimes.AsReadOnly();

				return bankRuntimesRO;
			}
		}

		internal int PrimaryBankIndex
		{
			get { return primaryIndex; }
		}

		internal BankType PrimaryBank
		{
			get
			{
				if (primaryIndex == -1)
					return null;

				return banks[primaryIndex];
			}
			set
			{
				primaryIndex = banks.IndexOf(value);
			}
		}

		internal bool RegisterBank(BankType bank)
		{
			if (banks.Contains(bank))
			{
				Debug.LogWarningFormat("BankManager.RegisterBank(): {0} \"{1}\" was already registered", typeof(BankType).Name, bank.name);
				return false;
			}

			banks.Add(bank);
			bankRuntimes.Add(null);
			bankGameObjects.Add(null);

			if (primaryIndex == -1)
				primaryIndex = 0;

			return true;
		}

		internal bool DeregisterBank(BankType bank)
		{
			int index = banks.FindIndex((x) => { return x == bank; });
			if (index == -1)
			{
				Debug.LogWarningFormat("BankManager.DeregisterBank(): {0} \"{1}\" was not found", typeof(BankType).Name, bank.name);
				return false;
			}

			GameObject bankGameObject = bankGameObjects[index];
			if (bankGameObject != null)
				GameObject.Destroy(bankGameObject);

			banks.RemoveAt(index);
			bankRuntimes.RemoveAt(index);
			bankGameObjects.RemoveAt(index);

			return true;
		}

		internal RuntimeType FetchRuntime(int index)
		{
			BankType bank = banks[index];

			RuntimeType runtime = bankRuntimes[index];
			if (runtime != null)
				return runtime;

			GameObject gameObject = new GameObject();
			gameObject.name = bank.name;
			GameObject.DontDestroyOnLoad(gameObject);

			runtime = gameObject.AddComponent<RuntimeType>();
			runtime.Init(bank);

			bankRuntimes[index] = runtime;
			bankGameObjects[index] = gameObject;

			return runtime;
		}
	}
}

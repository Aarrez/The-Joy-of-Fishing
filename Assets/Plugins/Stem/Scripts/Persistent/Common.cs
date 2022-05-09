using UnityEngine;

namespace Stem
{
	/// <summary>
	/// Defines how audio clips will be managed in memory.
	/// </summary>
	public enum AudioClipManagementMode
	{
		/// <summary>Preload audio clip data during startup and keep it in memory.</summary>
		PreloadAndKeepInMemory,
		/// <summary>Unload audio clip data if it was not used for some time.</summary>
		UnloadUnused,
		/// <summary>Do not manage audio clip data and instead allow the developer to take control.</summary>
		Manual,
	}

	/// <summary>
	/// Defines how new audio content will be created after the drag-drop event. Provided audio clips will be used as sound variations or playlist tracks.
	/// </summary>
	public enum AudioClipImportMode
	{
		/// <summary>Create a single item and put all audio clips to it.</summary>
		SingleItemWithAllClips,
		/// <summary>Create multiple items with a single audio clips.</summary>
		MultipleItemsWithSingleClip,
	}

	/// <summary>
	/// The universal unique identifier used to reference bank content.
	/// </summary>
	[System.Serializable]
	public struct ID : System.IEquatable<ID>
	{
		[SerializeField]
		private int bankGuidA;

		[SerializeField]
		private int bankGuidB;

		[SerializeField]
		private int bankGuidC;

		[SerializeField]
		private int bankGuidD;

		[SerializeField]
		private int itemId;

		/// <summary>
		/// The shorthand for writing ID(0, 0, 0, 0, 0) that does not refer to anything.
		/// </summary>
		/// <value>A read-only ID.</value>
		public static readonly ID None = new ID(0, 0, 0, 0, 0);

		/// <summary>
		/// Creates a new ID based on bank and sound/sound bus/playlist/music player identifiers.
		/// </summary>
		/// <param name="guidA">The first part of a bank identifier.</param>
		/// <param name="guidB">The second part of a bank identifier.</param>
		/// <param name="guidC">The third part of a bank identifier.</param>
		/// <param name="guidD">The fourth part of a bank identifier.</param>
		/// <param name="id">Unique identifier of a sound, sound bus, playlist or music player.</param>
		public ID(int guidA, int guidB, int guidC, int guidD, int id)
		{
			bankGuidA = guidA;
			bankGuidB = guidB;
			bankGuidC = guidC;
			bankGuidD = guidD;
			itemId = id;
		}

		/// <summary>
		/// The first part of a bank this ID refers to.
		/// </summary>
		/// <value>An integer value.</value>
		public int BankGuidA
		{
			get { return bankGuidA; }
		}

		/// <summary>
		/// The second part of a bank this ID refers to.
		/// </summary>
		/// <value>An integer value.</value>
		public int BankGuidB
		{
			get { return bankGuidB; }
		}

		/// <summary>
		/// The third part of a bank this ID refers to.
		/// </summary>
		/// <value>An integer value.</value>
		public int BankGuidC
		{
			get { return bankGuidC; }
		}

		/// <summary>
		/// The fourth part of a bank this ID refers to.
		/// </summary>
		/// <value>An integer value.</value>
		public int BankGuidD
		{
			get { return bankGuidD; }
		}

		/// <summary>
		/// The sound, sound bus, playlist or music player this ID refers to.
		/// </summary>
		/// <value>An integer value.</value>
		/// <remarks>
		/// <para>
		/// This value corresponds to <see cref="Sound"/>, <see cref="SoundBus"/>, <see cref="Playlist"/> or <see cref="MusicPlayer"/> ID.
		/// </para>
		/// </remarks>
		public int ItemId
		{
			get { return itemId; }
		}

		/// <summary>
		/// Checks if two IDs are referencing to the same bank.
		/// </summary>
		/// <param name="id">An ID to compare.</param>
		/// <returns>
		/// <para>True, if both IDs reference to the same bank. False otherwise.</para>
		/// </returns>
		public bool BankEquals(ID id)
		{
			return (BankGuidA == id.BankGuidA)
			&& (BankGuidB == id.BankGuidB)
			&& (BankGuidC == id.BankGuidC)
			&& (BankGuidD == id.BankGuidD);
		}

		/// <summary>
		/// Checks if two IDs are equal.
		/// </summary>
		/// <param name="id">An ID to compare.</param>
		/// <returns>
		/// <para>True, if both IDs are equal. False otherwise.</para>
		/// </returns>
		public bool Equals(ID id)
		{
			return (ItemId == id.ItemId) && BankEquals(id);
		}

		/// <summary>
		/// Calculates the hash of an ID.
		/// </summary>
		/// <returns>
		/// <para>A hash code for the ID.</para>
		/// </returns>
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 16777619 + BankGuidA.GetHashCode();
			hash = hash * 16777619 + BankGuidB.GetHashCode();
			hash = hash * 16777619 + BankGuidC.GetHashCode();
			hash = hash * 16777619 + BankGuidD.GetHashCode();
			hash = hash * 16777619 + ItemId.GetHashCode();
			
			return hash;
		}

		/// <summary>
		/// Checks if two object instances are equal.
		/// </summary>
		/// <param name="obj">A reference to an object.</param>
		/// <returns>
		/// <para>True, if <paramref name="obj"/> is an ID and both IDs are equal. False otherwise.</para>
		/// </returns>
		public override bool Equals(object obj)
		{
			return (obj is ID) && Equals((ID)obj);
		}

		/// <summary>
		/// Returns a string that represents the ID.
		/// </summary>
		/// <returns>
		/// <para>A string representation of an ID.</para>
		/// </returns>
		public override string ToString()
		{
			return string.Format("({0:X}:{1:X}:{2:X}:{3:X}): {4}", bankGuidA, bankGuidB, bankGuidC, bankGuidD, itemId);
		}

		/// <summary>
		/// Checks if two IDs are equal.
		/// </summary>
		/// <param name="id1">The first ID to compare.</param>
		/// <param name="id2">The second ID to compare.</param>
		/// <returns>
		/// <para>True, if both IDs are equal. False otherwise.</para>
		/// </returns>
		public static bool operator ==(ID id1, ID id2)
		{
			return Equals(id1, id2);
		}

		/// <summary>
		/// Checks if two IDs are not equal.
		/// </summary>
		/// <param name="id1">The first ID to compare.</param>
		/// <param name="id2">The second ID to compare.</param>
		/// <returns>
		/// <para>True, if both IDs are not equal. False otherwise.</para>
		/// </returns>
		public static bool operator !=(ID id1, ID id2)
		{
			return !Equals(id1, id2);
		}
	}

	/// <summary>
	/// The common bank interface for runtime state management.
	/// </summary>
	public interface IBank
	{
		/// <summary>
		/// Returns the bank <see cref="Stem.ID"/>.
		/// </summary>
		/// <returns>
		/// <para>An ID value.</para>
		/// </returns>
		ID GetBankID();

		/// <summary>
		/// Generates a new unique <see cref="Stem.ID"/> for the bank.
		/// </summary>
		/// <remarks>
		/// <para>This method is automatically called by Stem during serialization process. Don't call it manually as it may break existing ID references.</para>
		/// </remarks>
		void RegenerateBankID();
	}

	/// <summary>
	/// The container interface used by memory manager for audio clip management.
	/// </summary>
	public interface IAudioClipContainer
	{
		/// <summary>
		/// Gets the number of audio clips in the container.
		/// </summary>
		/// <returns>
		/// The number of audio clips.
		/// </returns>
		int GetNumAudioClips();

		/// <summary>
		/// Gets the audio clip at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the audio clip to get.</param>
		/// <returns>
		/// A reference to an audio clip.
		/// </returns>
		AudioClip GetAudioClip(int index);

		/// <summary>
		/// Gets the audio clip unload interval of the container.
		/// </summary>
		/// <remarks>
		/// <para>This value is only used if <see cref="IAudioClipContainer.GetAudioClipManagementMode"/> return value is <see cref="AudioClipManagementMode.UnloadUnused"/></para>
		/// </remarks>
		/// <returns>
		/// The time interval in seconds.
		/// </returns>
		float GetAudioClipUnloadInterval();

		/// <summary>
		/// Gets the audio clip management mode of the container.
		/// </summary>
		/// <returns>
		/// An enum value.
		/// </returns>
		AudioClipManagementMode GetAudioClipManagementMode();
	}
}

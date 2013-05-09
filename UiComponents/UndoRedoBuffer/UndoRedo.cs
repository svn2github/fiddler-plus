using System;
using System.Collections;

namespace UndoRedoBuffer
{
	public class UndoRedoException : ApplicationException
	{
		public UndoRedoException(string msg) : base(msg)
		{
		}
	}

	/// <summary>
	/// A class that manages an array of IMemento objects, implementing
	/// undo/redo capabilities for the IMemento originator class.
	/// </summary>
	public class UndoBuffer
	{
		protected ArrayList buffer;
		protected int idx;

		/// <summary>
		/// Returns true if there are items in the undo buffer.
		/// </summary>
		public bool CanUndo
		{
			get {return idx > 0;}
		}

		/// <summary>
		/// Returns true if the current position in the undo buffer will
		/// allow for redo's.
		/// </summary>
		public bool CanRedo
		{
			// idx+1 because the topmost buffer item is the topmost state
			get {return buffer.Count > idx+1;}
		}

		/// <summary>
		/// Returns true if at the top of the undo buffer.
		/// </summary>
		public bool AtTop
		{
			get {return idx == buffer.Count;}
		}

		/// <summary>
		/// Returns the count of undo items in the buffer.
		/// </summary>
		public int Count
		{
			get {return buffer.Count;}
		}

		/// <summary>
		/// Returns the action text associated with the Memento that holds
		/// the last known state.  An empty string is returned if there are
		/// no items to undo.
		/// </summary>
		public string UndoAction
		{
			get
			{
				string ret=String.Empty;
				if (idx > 0)
				{
					ret=((IMemento)buffer[idx-1]).Action;
				}
				return ret;
			}
		}

		/// <summary>
		/// Returns the action text associated with the Memento that holds
		/// the current state.  An empty string is returned if there are
		/// no items to redo.
		/// </summary>
		public string RedoAction
		{
			get
			{
				string ret=String.Empty;
				if (idx < buffer.Count)
				{
					ret=((IMemento)buffer[idx]).Action;
				}
				return ret;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public UndoBuffer()
		{
			buffer=new ArrayList();
			idx=0;
		}

		/// <summary>
		/// Saves the current state.  This does not adjust the current undo indexer.
		/// Use this method only when performing an Undo and AtTop==true, so that 
		/// the current state, before the Undo, can be saved, allowing a Redo to 
		/// be applied.
		/// </summary>
		/// <param name="mem"></param>
		public void PushCurrentState(IMemento mem)
		{
			buffer.Add(mem);
		}

		/// <summary>
		/// Save the current state at the index position.  Anything past the index position is lost.  This means that the "redo" action is no longer possible.
		/// Scenario--The user does 10 things.  The user undo's 5 of them, then does something new.  He can only undo now, he cannot "redo".  If he does one 
		/// undo, then he can do one "redo".
		/// </summary>
		/// <param name="mem">The memento holding the current state.</param>
		public void Do(IMemento mem)
		{
			if (buffer.Count > idx)
			{
				buffer.RemoveRange(idx, buffer.Count-idx);
			}
			buffer.Add(mem);
			++idx;
		}

		/// <summary>
		/// Returns the current memento.
		/// </summary>
		public IMemento Undo()
		{
			if (idx==0)
			{
				throw new UndoRedoException("Invalid index.");
			}
			--idx;
			return (IMemento)buffer[idx];
		}

		/// <summary>
		/// Returns the next memento.
		/// </summary>
		public IMemento Redo()
		{
			++idx;
			return (IMemento)buffer[idx];
		}

		/// <summary>
		/// Removes all state information.
		/// </summary>
		public void Flush()
		{
			buffer.Clear();
			idx=0;
		}
	}
}

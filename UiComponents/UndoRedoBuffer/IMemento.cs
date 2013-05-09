using System;

namespace UndoRedoBuffer
{
	public interface IMemento
	{
		object State
		{
			get;
			set;
		}

		string Action
		{
			get;
			set;
		}
	}

	public interface ISupportMemento
	{
		IMemento Memento
		{
			get;
			set;
		}
	}

	public class Memento : IMemento
	{
		protected object state;
		protected string action;

		public virtual object State
		{
			get {return state;}
			set {state=value;}
		}

		public string Action
		{
			get {return action;}
			set {action=value;}
		}
	}
}

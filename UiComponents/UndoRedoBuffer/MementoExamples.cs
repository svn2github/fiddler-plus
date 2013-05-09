using System;

namespace UndoRedoBuffer
{
	// Example 1:  MyClass1 is responsible for encoding/decoding state

	public class MyClass1 : ISupportMemento
	{
		public IMemento Memento
		{
			get
			{
				Memento mcm=new Memento();
				mcm.State=GetMyState();
				return mcm;
			}
			set
			{
				SetMyState(value.State);
			}
		}
  
		protected object GetMyState()
		{
			// ... implementation ...
			return null;
		}

		protected void SetMyState(object state)
		{
			// ... implementation ...
		}
	}

	// Example 2: MyClassMemento is responsible for encoding/decoding state

	public class MyClass2 : ISupportMemento
	{
		public IMemento Memento
		{
			get
			{
				IMemento mcm=new MyClassMemento(this);
				return mcm;
			}
			set
			{
				((MyClassMemento)value).SetState(this);
			}
		}
	}

	public class MyClassMemento : Memento
	{
		public MyClassMemento(MyClass2 mc)
		{
			// get MyClass2 state by querying properties of MyClass instance.
		}

		public void SetState(MyClass2 mc)
		{
			// set MyClass2 state by setting properties of MyClass2 instance.
		}
	}
}

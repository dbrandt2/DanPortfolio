using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDD2200_DoublyLinkedList
{
	/// <summary>
	/// DoublyLinkedList
	/// </summary>
	public class DoublyLinkedList
	{
        #region DoublyLinkedListNode Class
        /// <summary>
        /// DoublyLinkedListNode
        /// Private class that is only known to the DoublyLinkedList class
        /// and cannot be accessed by any "outside" code
        /// </summary>
        private class DoublyLinkedListNode
		{
			/// <summary>
			/// Reference to the next node in the list
			/// </summary>
			public DoublyLinkedListNode Next { get; set; }

			/// <summary>
			/// Reference to the previous node in the list
			/// </summary>
			public DoublyLinkedListNode Prev { get; set; }

			/// <summary>
			/// Item stored in the node
			/// </summary>
			public int Item { get; set; }

			/// <summary>
			/// Node constructor
			/// </summary>
			/// <param name="item">item to store in the node</param>
			/// <param name="next">next node in the list</param>
			/// <param name="prev">previous node in the list</param>
			public DoublyLinkedListNode(int item, DoublyLinkedListNode next = null, DoublyLinkedListNode prev = null)
			{
				Item = item;
				Next = next;
				Prev = prev;
			}
		}
        #endregion

        /// <summary>
        /// Reference to the "front" of the list
        /// </summary>
        private DoublyLinkedListNode Head { get; set; }

		/// <summary>
		/// Reference to the "end" of the list
		/// </summary>
		private DoublyLinkedListNode Tail { get; set; }

		/// <summary>
		/// Number of items in the list
		/// </summary>
		public int Count { get; private set; }

		public int this[int index]
		{
			get
			{
				if (index >= Count || index < 0)
				{
					throw new IndexOutOfRangeException();
				}
				else
				{
					DoublyLinkedListNode curr = Head;
					int counter = 0;
					while (curr != null && counter != index)
					{
						curr = curr.Next;
						counter++;
					}
					return curr.Item;
				}
			}
			set
			{
				if (index >= Count || index < 0)
				{
					throw new IndexOutOfRangeException();
				}
				else
				{
					DoublyLinkedListNode curr = Head;
					int counter = 0;
					while (curr != null && counter != index)
					{
						curr = curr.Next;
						counter++;
					}
					curr.Item = value;
				}
			}
		}

		/// <summary>
		/// Add the item to the front of the list
		/// </summary>
		/// <param name="item">item to add</param>
		public void AddFront(int item)
		{
			DoublyLinkedListNode curr = new DoublyLinkedListNode(item);
			if (Head == null)
			{
				Head = curr;
				Tail = curr;
			}
			else
			{
				curr.Next = Head;
				Head.Prev = curr;
				Head = curr;
			}
			Count++;
		}

		/// <summary>
		/// Add the item to the back of the list
		/// </summary>
		/// <param name="item">item to add</param>
		public void AddBack(int item)
		{
			DoublyLinkedListNode curr = new DoublyLinkedListNode(item);
			if (Head == null)
			{
				Head = curr;
				Tail = curr;
			}
			else
			{
				Tail.Next = curr;
				curr.Prev = Tail;
				Tail = curr;
			}
			Count++;
		}

		/// <summary>
		/// Remove item from the back of the list
		/// </summary>
		/// <returns>item removed</returns>
		public int RemoveBack()
		{
			if (Head == null)
			{
				throw new Exception("Cannot remove from an empty list");
			}
			else if (Head == Tail)
			{
				int item = Head.Item;
				Head = null;
				Tail = null;
				Count--;
				return item;
			}
			else
			{
				int item = Tail.Item;
				Tail = Tail.Prev;
				Tail.Next.Prev = null;
				Tail.Next = null;
				Count--;
				return item;
			}
		}

		/// <summary>
		/// Remove the item from the front of the list
		/// </summary>
		/// <returns>item removed</returns>
		public int RemoveFront()
		{
			if (Head == null)
			{
				throw new Exception("Cannot remove from an empty list");
			}
			else if (Head == Tail)
			{
				int item = Head.Item;
				Head = null;
				Tail = null;
				Count--;
				return item;
			}
			else
			{
				int item = Head.Item;
				Head = Head.Next;
				Head.Prev.Next = null;
				Head.Prev = null;
				Count--;
				return item;
			}
		}

		/// <summary>
		/// Remove the item provided from the list
		/// </summary>
		/// <param name="item">item to remove</param>
		/// <returns>true if found and removed, false otherwise</returns>
		public bool RemoveItem(int item)
		{
			DoublyLinkedListNode curr = Head;
			while (curr != null)
			{
				if (curr.Item == item)
				{
					if (curr == Head)
					{
						RemoveFront();
					}
					else if (curr == Tail)
					{
						RemoveBack();
					}
					else
					{
						curr.Next.Prev = curr.Prev;
						curr.Prev.Next = curr.Next;
						Count--;
					}
					return true;
				}
				curr = curr.Next;
			}
			return false;
		}

		/// <summary>
		/// Search for the item in the list
		/// </summary>
		/// <param name="item">true if found, false if otherwise</param>
		/// <returns></returns>
		public bool Search(int item)
		{
			DoublyLinkedListNode curr = Head;
			while (curr != null)
			{
				if (curr.Item == item)
				{
					return true;
				}
				curr = curr.Next;
			}
			return false;
		}

		/// <summary>
		/// Convert the list to a string
		/// </summary>
		/// <returns>The list as a string</returns>
		public override string ToString()
		{
			DoublyLinkedListNode curr = Head;
			StringBuilder output = new StringBuilder("HEAD->");
			while (curr != null)
			{
				output.Append(curr.Item);
				output.Append("->");
				curr = curr.Next;
			}
			output.Append("NULL\n");

			output.Append("TAIL->");
			curr = Tail;
			while (curr != null)
			{
				output.Append(curr.Item);
				output.Append("->");
				curr = curr.Prev;
			}
			output.Append("NULL");

			return output.ToString();
		}
	}
}

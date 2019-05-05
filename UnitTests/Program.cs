using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDD2200_DoublyLinkedList
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create a list
			DoublyLinkedList list = new DoublyLinkedList();

			// Add the items 0 to 9 to the list
			Console.WriteLine("******************************************");
			for (int i = 0; i < 10; ++i)
			{
				list.AddFront(i);
				Console.WriteLine("AddFront " + i + ": ");
				Console.WriteLine(list);
			}

			// Search for each of the added nodes
			Console.WriteLine("******************************************");
			for (int i = 0; i < 11; ++i)
			{
				Console.WriteLine("Search for " + i + ": " + list.Search(i));
			}

			// Remove all the items in the list
			Console.WriteLine("******************************************");
			while (list.Count > 0)
			{
				Console.WriteLine("RemoveFront: " + list.RemoveFront());
				Console.WriteLine(list);
			}

			// Add the items 0 to 9 to the list
			Console.WriteLine("******************************************");
			for (int i = 0; i < 10; ++i)
			{
				list.AddBack(i);
				Console.WriteLine("AddBack " + i + ": ");
				Console.WriteLine(list);
			}

			// Search for each of the added nodes
			Console.WriteLine("******************************************");
			for (int i = 0; i < 11; ++i)
			{
				Console.WriteLine("Search for " + i + ": " + list.Search(i));
			}

			// Remove all the items in the list
			Console.WriteLine("******************************************");
			while (list.Count > 0)
			{
				Console.WriteLine("RemoveBack: " + list.RemoveBack());
				Console.WriteLine(list);
			}

			Console.WriteLine("******************************************");
			Random rand = new Random();
			List<int> items = new List<int>();
			
			// Add the items 0 to 9 to the list
			for (int i = 0; i < 10; ++i)
			{
				list.AddFront(i);
				items.Add(i);
			}

			// Try removing an item that doesn't exist
			Console.WriteLine("RemoveItem 20: " + list.RemoveItem(20));

			// Remove all the items in the list in random order
			while (items.Count > 0)
			{
				int item = items[rand.Next(items.Count)];
				items.Remove(item);
				Console.WriteLine("RemoveItem " + item + ": " + list.RemoveItem(item));
				Console.WriteLine(list);
			}

			// Try removing an item that doesn't exist
			Console.WriteLine("RemoveItem 20: " + list.RemoveItem(20));

			// Try removing from an empty list
			Console.WriteLine("******************************************");
			list = new DoublyLinkedList();
			try
			{
				list.RemoveBack();
			}
			catch (Exception e)
			{
				Console.WriteLine("Caught exception correctly: " + e.Message);
			}

			try
			{
				list.RemoveFront();
			}
			catch (Exception e)
			{
				Console.WriteLine("Caught exception correctly: " + e.Message);
			}

            // Try testing the index
            for (int i = 0; i < 5; ++i)
            {
                list.AddFront(i);
            }

            Console.WriteLine("Before indexing:\n" + list);
            for (int i = 0; i < list.Count; ++i)
            {
                list[i] = list[i] * list[i];
            }
            Console.WriteLine("After  indexing:\n" + list);

            Console.ReadKey();
		}
	}
}

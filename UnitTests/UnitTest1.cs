using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GDD2200_DoublyLinkedList;
using System.Collections.Generic;

namespace DoublyLinkedListTestProject
{
	// Attributes
	// [TestClass()] - class that includes a set of tests
	// [TestMethod()] - method that runs a specific (short!) test on something
	// [TestInitialize()] - runs before each test to setup environment
	// [TestCleanup()] - runs after each test to destroy data or clean up memory

	[TestClass()]
	public class UnitTest1
	{
		DoublyLinkedList _list;

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the AddFront Method
		/// </summary>
		[TestMethod()]
		public void AddFrontTest_SingleItem()
		{
			_list = new DoublyLinkedList();

			// Test the addition of a single item
			_list.AddFront(0);
			Assert.AreEqual(1, _list.Count);
			Assert.AreEqual("HEAD->0->NULL\nTAIL->0->NULL", _list.ToString());
		}

		[TestMethod()]
		public void AddFrontTest_SecondItem()
		{
			_list = new DoublyLinkedList();

			// Test the addition of the second item
			_list.AddFront(0);
			_list.AddFront(1);
			Assert.AreEqual(2, _list.Count);
			Assert.AreEqual("HEAD->1->0->NULL\nTAIL->0->1->NULL", _list.ToString());
		}

		[TestMethod()]
		public void AddFrontTest_MultipleItems()
		{
			_list = new DoublyLinkedList();

			// Test the addition of a single item
			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			Assert.AreEqual(5, _list.Count);
			Assert.AreEqual("HEAD->4->3->2->1->0->NULL\nTAIL->0->1->2->3->4->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the RemoveBack method (after AddBack)
		/// </summary>
		[TestMethod()]
		public void RemoveBackAddBackTest_SingleItem()
		{
			_list = new DoublyLinkedList();

			// Test the removal of a single item
			_list.AddBack(0);
			int item = _list.RemoveBack();
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveBackAddBackTest_SecondItem()
		{
			_list = new DoublyLinkedList();

			_list.AddBack(0);
			_list.AddBack(1);

			// Test the removal of the second item
			Assert.AreEqual(1, _list.RemoveBack());
			Assert.AreEqual(0, _list.RemoveBack());
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveBackAddBackTest_MultipleItems()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddBack(i);
			}

			// Test the removal of all items
			for (int i = 4; i >= 0; --i)
			{
				int item = _list.RemoveBack();
				Assert.AreEqual(i, item);
			}

			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the RemoveFront method (after AddBack)
		/// </summary>
		[TestMethod()]
		[ExpectedException(typeof(Exception))]
		public void RemoveFrontException()
		{
			_list = new DoublyLinkedList();

			// Test throw of an exception from empty _list
			_list.RemoveFront();
		}

        //////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Test the RemoveFront method (after AddBack)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void RemoveBackException()
        {
            _list = new DoublyLinkedList();

            // Test throw of an exception from empty _list
            _list.RemoveBack();
        }

        [TestMethod()]
		public void RemoveFrontAddBackTest_SingleItem()
		{
			_list = new DoublyLinkedList();

			// Test the removal of a single item
			_list.AddBack(0);
			int item = _list.RemoveFront();
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveFrontAddBackTest_SecondItem()
		{
			_list = new DoublyLinkedList();

			_list.AddBack(0);
			_list.AddBack(1);

			// Test the removal of the second item
			Assert.AreEqual(0, _list.RemoveFront());
			Assert.AreEqual(1, _list.RemoveFront());
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveFrontAddBackTest_MultipleItems()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddBack(i);
			}

			// Test the removal of all items
			for (int i = 0; i < 5; ++i)
			{
				int item = _list.RemoveFront();
				Assert.AreEqual(i, item);
			}

			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the RemoveFront method (after AddFront)
		/// </summary>
		[TestMethod()]
		public void RemoveFrontAddFrontTest_SingleItem()
		{
			_list = new DoublyLinkedList();

			// Test the removal of a single item
			_list.AddFront(0);
			int item = _list.RemoveFront();
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveFrontAddFrontTest_SecondItem()
		{
			_list = new DoublyLinkedList();

			_list.AddFront(0);
			_list.AddFront(1);

			// Test the removal of the second item
			Assert.AreEqual(1, _list.RemoveFront());
			Assert.AreEqual(0, _list.RemoveFront());
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the RemoveBack method (after AddFront)
		/// </summary>
		[TestMethod()]
		public void RemoveBackAddFrontTest_SingleItem()
		{
			_list = new DoublyLinkedList();

			// Test the removal of a single item
			_list.AddFront(0);
			int item = _list.RemoveBack();
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveBackAddFrontTest_SecondItem()
		{
			_list = new DoublyLinkedList();

			_list.AddFront(0);
			_list.AddFront(1);

			// Test the removal of the second item
			Assert.AreEqual(0, _list.RemoveBack());
			Assert.AreEqual(1, _list.RemoveBack());
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveBackAddFrontTest_MultipleItems()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			// Test the removal of all items
			for (int i = 0; i < 5; ++i)
			{
				int item = _list.RemoveBack();
				Assert.AreEqual(i, item);
			}

			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the RemoveItem method
		/// </summary>
		[TestMethod()]
		public void RemoveItemExistingAddFrontTest()
		{
			_list = new DoublyLinkedList();
			Random rand = new Random();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			Assert.AreEqual(true, _list.RemoveItem(2));
			Assert.AreEqual(4, _list.Count);
			Assert.AreEqual("HEAD->4->3->1->0->NULL\nTAIL->0->1->3->4->NULL", _list.ToString());

			Assert.AreEqual(true, _list.RemoveItem(3));
			Assert.AreEqual(3, _list.Count);
			Assert.AreEqual("HEAD->4->1->0->NULL\nTAIL->0->1->4->NULL", _list.ToString());

			Assert.AreEqual(true, _list.RemoveItem(0));
			Assert.AreEqual(2, _list.Count);
			Assert.AreEqual("HEAD->4->1->NULL\nTAIL->1->4->NULL", _list.ToString());

			Assert.AreEqual(true, _list.RemoveItem(4));
			Assert.AreEqual(1, _list.Count);
			Assert.AreEqual("HEAD->1->NULL\nTAIL->1->NULL", _list.ToString());

			Assert.AreEqual(true, _list.RemoveItem(1));
			Assert.AreEqual(0, _list.Count);
			Assert.AreEqual("HEAD->NULL\nTAIL->NULL", _list.ToString());
		}

		[TestMethod()]
		public void RemoveItemNonExistingAddFrontTest()
		{
			_list = new DoublyLinkedList();
			Random rand = new Random();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			Assert.AreEqual(false, _list.RemoveItem(5));
			Assert.AreEqual(5, _list.Count);
			Assert.AreEqual("HEAD->4->3->2->1->0->NULL\nTAIL->0->1->2->3->4->NULL", _list.ToString());

			Assert.AreEqual(false, _list.RemoveItem(-1));
			Assert.AreEqual(5, _list.Count);
			Assert.AreEqual("HEAD->4->3->2->1->0->NULL\nTAIL->0->1->2->3->4->NULL", _list.ToString());

			Assert.AreEqual(false, _list.RemoveItem(-10));
			Assert.AreEqual(5, _list.Count);
			Assert.AreEqual("HEAD->4->3->2->1->0->NULL\nTAIL->0->1->2->3->4->NULL", _list.ToString());

			Assert.AreEqual(false, _list.RemoveItem(10));
			Assert.AreEqual(5, _list.Count);
			Assert.AreEqual("HEAD->4->3->2->1->0->NULL\nTAIL->0->1->2->3->4->NULL", _list.ToString());
		}

		//////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test the Search method
		/// </summary>
		[TestMethod()]
		public void SearchExistingAddFrontTest()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			// Test search for all added items
			for (int i = 0; i < 5; ++i)
			{
				Assert.AreEqual(true, _list.Search(i));
			}
		}

		[TestMethod()]
		public void SearchExistingAddBackTest()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddBack(i);
			}

			// Test search for all added items
			for (int i = 0; i < 5; ++i)
			{
				Assert.AreEqual(true, _list.Search(i));
			}
		}

		[TestMethod()]
		public void SearchNonExistingAddFrontTest()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			Assert.AreEqual(false, _list.Search(-25));
			Assert.AreEqual(false, _list.Search(-5));
			Assert.AreEqual(false, _list.Search(-1));
			Assert.AreEqual(false, _list.Search(5));
			Assert.AreEqual(false, _list.Search(10));
			Assert.AreEqual(false, _list.Search(25));
		}

		[TestMethod()]
		public void SearchRemoveBackAddFrontTest()
		{
			_list = new DoublyLinkedList();

			for (int i = 0; i < 5; ++i)
			{
				_list.AddFront(i);
			}

			_list.RemoveBack();

			Assert.AreEqual(false, _list.Search(0));
		}

        //pulling index from empty list
        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ThisIndex_checkIndex()
        {
            _list = new DoublyLinkedList();

            //check empty list 
            int test = _list[0];
        }

        //setting index in empty list
        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SetIndex_emptyList()
        {
            _list = new DoublyLinkedList();

            //setting an index in list that is out of range of list 
            _list[0] = 1;
        }

        [TestMethod()]
        public void GetIndex_IndexCounter()
        {
            _list = new DoublyLinkedList();
            _list.AddFront(1);
            _list.AddFront(2);

            //Test that counter in the list works properly 
            Assert.AreEqual(1, _list[1]);
        }

        [TestMethod()]
        public void GetIndex_SetIndexCounter()
        {
            _list = new DoublyLinkedList();
            _list.AddFront(1);
            _list.AddFront(2);

            _list[1] = 2;

            //Test that counter in the list works properly 
            Assert.AreEqual(2, _list[1]);
        }
    }
}
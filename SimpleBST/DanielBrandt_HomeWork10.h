#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

struct LinkedListNode
{
	//elements needing to be stored from the csv
	float price_m;
	int isAvailable_m;
	char make_m[20];
	char model_m[20];
	char size_m[20];
	char color_m[20];
	char power_m[20];

	//pointers for next in the linked list 
	struct LinkedListNode *leftChildPtr;
	struct LinkedListNode *rightChildPtr;
};

typedef struct LinkedListNode ListNode;
typedef struct LinkedListNode *ListNodePtr;

//prototypes for needed functions
void PrintMenu();
void PrintCar(ListNode car);

int MenuSelection(); 
void GetModel(char model[]);
int CompareStrings(char input[], char model[]);

//Recursive Functions
void Insert(ListNode car, ListNodePtr currPtr); 
bool RemoveCar(ListNode car);
bool Remove(ListNode car, ListNodePtr currPtr);
ListNodePtr RemoveNode(ListNodePtr currPtr);
bool Search(ListNode car, ListNodePtr currPtr);

//recursive print functions
bool InOrderPrint(ListNodePtr currPtr);
bool PreOrderPrint(ListNodePtr currPtr);
bool PostOrderPrint(ListNodePtr currPtr);

//pointer to head of list 
ListNodePtr rootPtr = NULL; //the head pointer starts pointing at nothing






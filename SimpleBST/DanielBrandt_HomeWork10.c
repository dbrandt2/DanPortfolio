//Daniel Brandt HomeWork 10
//Trying to convert this stack to a BST order by price since the float is an easy comparison
//5/2/19
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include "DanielBrandt_HomeWork10.h"

int main(int argc, char *argv[]) {
	
	int input = 0;
	char rModel[20];
	float rPrice;
	char rMake[20];
	char rSize[20];
	char rColor[20];
	char rPower[20];
	int rentResult;
	
	
	do {
	input = MenuSelection();
		switch (input)
		{	
			case 1: //Enter a car
				printf("Please enter the car you would like to Insert into the BST (based on car's price): \n");
				
				//create memory for the car that is going to be pushed
				ListNode pushCar;  
				
				printf("Please enter the make of the car: ");
				scanf("%s", rMake);
				
				printf("Please enter the model of the car: ");
				scanf("%s", rModel);
				
				printf("Please enter the size of the car: ");
				scanf("%s", rSize);
				
				printf("Please enter the color of the car: ");
				scanf("%s", rColor);
				
				printf("Please enter the power of the car: ");
				scanf("%s", rPower);
				
				printf("Please enter the price of the car: ");
				scanf("%f", &rPrice);
				
				pushCar.isAvailable_m = 1;
				strcpy(pushCar.make_m, rMake);
				strcpy(pushCar.model_m, rModel);
				strcpy(pushCar.size_m, rSize);
				strcpy(pushCar.color_m, rColor);
				strcpy(pushCar.power_m, rPower);
				pushCar.price_m = rPrice;
				
				Insert(pushCar, rootPtr);
				break;
				
			case 2: //Remove a car
				printf("Please enter the car you would like to Remove from BST (based on car's price): \n");
				
				ListNode removeCar;
				//create memory for the car that is going to be pushed
				
				printf("Please enter the make of the car: ");
				scanf("%s", rMake);
				
				printf("Please enter the model of the car: ");
				scanf("%s", rModel);
				
				printf("Please enter the size of the car: ");
				scanf("%s", rSize);
				
				printf("Please enter the color of the car: ");
				scanf("%s", rColor);
				
				printf("Please enter the power of the car: ");
				scanf("%s", rPower);
				
				printf("Please enter the price of the car: ");
				scanf("%f", &rPrice);
				
				removeCar.isAvailable_m = 1;
				strcpy(removeCar.make_m, rMake);
				strcpy(removeCar.model_m, rModel);
				strcpy(removeCar.size_m, rSize);
				strcpy(removeCar.color_m, rColor);
				strcpy(removeCar.power_m, rPower);
				removeCar.price_m = rPrice;
				
				RemoveCar(removeCar);
				break;
				
			case 3: //Print the list of cars (InOrder)
				InOrderPrint(rootPtr);
				break;
				
			case 4: //Print the list of cars (PreOrder)
				PreOrderPrint(rootPtr);
				break;
				
			case 5: //Print the list of cars (PostOrder)
				PostOrderPrint(rootPtr);
				break;
				
			case 6: //Search for a car
				printf("Please enter the car you would like to Search for (based on car's price): \n");
				
				ListNode searchCar;
				//create memory for the car that is going to be pushed
				
				printf("Please enter the make of the car: ");
				scanf("%s", rMake);
				
				printf("Please enter the model of the car: ");
				scanf("%s", rModel);
				
				printf("Please enter the size of the car: ");
				scanf("%s", rSize);
				
				printf("Please enter the color of the car: ");
				scanf("%s", rColor);
				
				printf("Please enter the power of the car: ");
				scanf("%s", rPower);
				
				printf("Please enter the price of the car: ");
				scanf("%f", &rPrice);
				
				searchCar.isAvailable_m = 1;
				strcpy(searchCar.make_m, rMake);
				strcpy(searchCar.model_m, rModel);
				strcpy(searchCar.size_m, rSize);
				strcpy(searchCar.color_m, rColor);
				strcpy(searchCar.power_m, rPower);
				searchCar.price_m = rPrice;
				
				Search(searchCar, rootPtr);
				break;
				
				
		}
	}
	while (input != 7);

	puts("Exiting Program...");
	return 0;
}

//Insert a car into the BST
void Insert(ListNode car, ListNodePtr currPtr)
{
	// create the memmory for the new car node
	ListNodePtr newNode = malloc(sizeof(ListNode));
	
	//copy in car's information into newNode
	strcpy(newNode->make_m, car.make_m);
	strcpy(newNode->model_m, car.model_m);
	strcpy(newNode->size_m, car.size_m);
	strcpy(newNode->color_m, car.color_m);
	strcpy(newNode->power_m, car.power_m);
	newNode->price_m = car.price_m;
	newNode->isAvailable_m = car.isAvailable_m;
	newNode->leftChildPtr = NULL;
	newNode->rightChildPtr = NULL;
	
	// if root is null then we are the are we are going to insert
	//else recurse through the tree till we fin our insertion point
	if (rootPtr == NULL)  
	{
		//set new car to rootPtr
		rootPtr = newNode;
	}
	else 
	{		
		//check to see if we will be inserting at left child
		if (newNode->price_m < currPtr->price_m)
		{
			//if the left child is null then we insert here
			if(currPtr->leftChildPtr == NULL)
			{
				currPtr->leftChildPtr = newNode;
			}
			else //recure through the tree
			{
				Insert(car, currPtr->leftChildPtr);
			}
		}
		else if (newNode->price_m > currPtr->price_m) // we are inserting as a right child
		{
			if (currPtr->rightChildPtr == NULL)
			{
				currPtr->rightChildPtr = newNode;
			}
			else //recure through the tree
			{
				Insert(car, currPtr->rightChildPtr);
			}
		}
	}
}

//Search the tree for a car
bool Search(ListNode car, ListNodePtr currPtr)
{
	if (currPtr == NULL)
	{
		return false;
	}
	
	if (car.price_m < currPtr->price_m)
	{
		Search(car, currPtr->leftChildPtr);
	}
	else if (car.price_m > currPtr->price_m)
	{
		Search(car, currPtr->rightChildPtr);
	}
	else 
	{
		PrintCar(*(currPtr));
		return true;
	}
}



//Compare strings return 1 if same return 0 if not
int CompareStrings(char input[], char model[])
{
	int i = 0;
	
	//while the letteres of the strings are equal	
	while (input[i] == model[i])
	{
		if(input[i] == '\0' || model[i] == '\0') //if we reach end of either string break
		{
			break;
		}
		
		//increment the pointer on the strings
		input++;
		model++;
	}
	
	//if we have reached the end of both strings without breaking then they are equal
	if (input[0] == '\0' && model[i] == '\0')
	{
		return 1;
	}
	else //otherwise they are not equal
	{
		return 0;
	}
}


//Remove a car from the tree (based on the cars price)
bool RemoveCar(ListNode car)
{
	//list is empty
	if (rootPtr == NULL)
	{
		printf("Cannot remove from empty tree.\n");
		return false;
	}
	else if (rootPtr->price_m == car.price_m) // then we are removing the root node
	{
		//we need to find the replacement for the root 
		if (rootPtr->leftChildPtr != NULL && rootPtr->rightChildPtr == NULL) // has left child
		{
			ListNodePtr tempPtr = rootPtr;
			rootPtr = tempPtr->leftChildPtr;
			free(tempPtr);
			tempPtr = NULL;
			return true;
		}
		else if (rootPtr->leftChildPtr == NULL && rootPtr->rightChildPtr != NULL) //has right child
		{
			ListNodePtr tempPtr = rootPtr;
			rootPtr = tempPtr->rightChildPtr;
			free(tempPtr);
			tempPtr = NULL;
			return true;
		}
		else if (rootPtr->leftChildPtr == NULL && rootPtr->rightChildPtr == NULL)//root has no children
		{
			free(rootPtr);
			rootPtr = NULL;
			return true;
		} 
		else //root has two children
		{
			RemoveNode(rootPtr);
			return true;
		}
	}
	else //we are not removing the root
	{
		return Remove(car, rootPtr);
	}
}

//overlaod remove
bool Remove(ListNode car, ListNodePtr currPtr)
{
	if (currPtr == NULL)
	{
		printf("cannot remove currPtr == NULL\n");
		return false;
	}
	if (car.price_m < currPtr->price_m) //remove from left subtree
	{
		if (currPtr->leftChildPtr == NULL)
		{
			return false;
		}
		if(car.price_m == currPtr->leftChildPtr->price_m)
		{
			currPtr->leftChildPtr = RemoveNode(currPtr->leftChildPtr);
			return true;	
		}
		else 
		{
			Remove(car, currPtr->leftChildPtr);
		}
	}
	else if (car.price_m > currPtr->price_m) //remove from right subtree
	{
		if (currPtr->rightChildPtr == NULL)
		{
			return false;
		}
		if (car.price_m == currPtr->rightChildPtr->price_m)
		{
			currPtr->rightChildPtr = RemoveNode(currPtr->rightChildPtr);
		}
		else 
		{
			Remove(car, currPtr->rightChildPtr);
		}
	}
}

ListNodePtr RemoveNode(ListNodePtr currPtr)
{
	if (currPtr->leftChildPtr == NULL && currPtr->rightChildPtr == NULL) //currPtr has no children
	{
		free(currPtr);
		currPtr = NULL;
		return NULL;
	}
	else if (currPtr->rightChildPtr != NULL && currPtr->leftChildPtr == NULL) //currPtr has right child
	{
		ListNodePtr tempPtr = currPtr->rightChildPtr;
		free(currPtr);
		currPtr = NULL;
		return tempPtr;
	}
	else if (currPtr->rightChildPtr == NULL && currPtr->leftChildPtr != NULL) //currPTr has left child
	{
		ListNodePtr tempPtr = currPtr->leftChildPtr;
		free(currPtr);
		currPtr = NULL;
		return tempPtr;
	}
	else //currPtr has both children
	{
		//create all temp pointers needed to find replacement node for currPtr
		ListNodePtr parPtr = currPtr;
		ListNodePtr minPtr = currPtr->rightChildPtr;
		ListNodePtr tempPtr = minPtr;
		
		while (minPtr->leftChildPtr != NULL) //trverse the right subtree intil min is found (far left of right subtree)
		{
			parPtr = minPtr;
			minPtr = parPtr->leftChildPtr;
		}
		
		//currPtr's right child is min
		if (parPtr == currPtr)
		{
			//copy information into currPtr
			strcpy(currPtr->rightChildPtr->make_m, minPtr->make_m);
			strcpy(currPtr->rightChildPtr->model_m, minPtr->model_m);
			strcpy(currPtr->rightChildPtr->size_m, minPtr->size_m);
			strcpy(currPtr->rightChildPtr->color_m, minPtr->color_m);
			strcpy(currPtr->rightChildPtr->power_m, minPtr->power_m);
			currPtr->rightChildPtr->price_m = minPtr->price_m;
			currPtr->rightChildPtr->isAvailable_m = minPtr->isAvailable_m;
			
			//set currPtr's right to minPtr's right
			currPtr->rightChildPtr = minPtr->rightChildPtr;
			free(minPtr);
			minPtr = NULL;
			return currPtr;
		}
		else //currPtr's right is not min
		{
			//copy information into currPtr
			strcpy(currPtr->make_m, minPtr->make_m);
			strcpy(currPtr->model_m, minPtr->model_m);
			strcpy(currPtr->size_m, minPtr->size_m);
			strcpy(currPtr->color_m, minPtr->color_m);
			strcpy(currPtr->power_m, minPtr->power_m);
			currPtr->price_m = minPtr->price_m;
			currPtr->isAvailable_m = minPtr->isAvailable_m;
			
			if (minPtr->rightChildPtr != NULL)
			{
				tempPtr = minPtr->rightChildPtr;
			}
			else if (minPtr->leftChildPtr != NULL)
			{
				tempPtr = minPtr->leftChildPtr;
			}
			else 
			{
				tempPtr = NULL;
			}
			
			//clean up used memory
			free(minPtr);
			minPtr = NULL;
			parPtr->leftChildPtr = tempPtr;
			return currPtr;
		}
	}
	
}


/////////////////////////////////////
//////Print Functions
////////////////////////////////////
//print the tree, prints the list in numeric order

//Print a car and all elements of the car
void PrintCar(ListNode car)
{
	if (car.isAvailable_m == 1)
	{
	 printf("%10s:%10s:%10s:%10s:%10s:  %.2f: Available to rent \n",  car.make_m, car.model_m, car.size_m, car.color_m, car.power_m, car.price_m);
	}
	else 
	{
	 printf("%10s:%10s:%10s:%10s:%10s:  %.2f: Not available to rent \n",  car.make_m, car.model_m, car.size_m, car.color_m, car.power_m, car.price_m);
	}
}

//prints the menu for user to select an option
void PrintMenu()
{
	printf("Please make a selection: \n");
	printf("1 - Insert car into BST. (This is solely based on the car's price) \n");
	printf("2 - Remove car from BST. (This is solely based on the car's price)\n");
	printf("3 - InOrderPrint Cars.\n");
	printf("4 - PreOrderPrint Cars. \n");
	printf("5 - PostOrderPrint Cars. \n");
	printf("6 - Search for a Car (based on car's price)\n");
	printf("7 - Exit. \n");
}

//get the model of the car to rent from the user
void GetModel(char model[])
{
	scanf("%s", model);
}

//gets the users input 
int MenuSelection()
{
	int selection = 0;
	
	do {
		PrintMenu();
		scanf("%d", &selection);
	} while (selection == 0 || selection > 7);
	
	return selection;
}

bool InOrderPrint(ListNodePtr currPtr)
{
	//tree is empty
	if (currPtr == NULL)
	{
		printf("Done printing. \n");
		return false;
	}
	
	//recure through and print tree
	//print left first
	if(currPtr->leftChildPtr != NULL)
	{
		InOrderPrint(currPtr->leftChildPtr);
	}
	
	//print current
	PrintCar(*(currPtr));
	
	if (currPtr->rightChildPtr != NULL)
	{
		InOrderPrint(currPtr->rightChildPtr);
	}
	
	return true;
}

bool PreOrderPrint(ListNodePtr currPtr)
{
	//check if list is not empty
	if (currPtr == NULL)
	{
		printf("Done printing. \n");
		return false;
	}
	
	PrintCar(*(currPtr));
	
	if (currPtr->leftChildPtr != NULL)
	{
		PreOrderPrint(currPtr->leftChildPtr);
	}
	
	if (currPtr->rightChildPtr != NULL)
	{
		PreOrderPrint(currPtr->rightChildPtr);
	}
}

bool PostOrderPrint(ListNodePtr currPtr)
{
	if (currPtr == NULL)
	{
		printf("Done printing. \n");
		return false;
	}
	
	if (currPtr->leftChildPtr != NULL)
	{
		PostOrderPrint(currPtr->leftChildPtr);
	}
	
	if (currPtr->rightChildPtr != NULL)
	{
		PostOrderPrint(currPtr->rightChildPtr);
	}
	
	PrintCar(*(currPtr));
}





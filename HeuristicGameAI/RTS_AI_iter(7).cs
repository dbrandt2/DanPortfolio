using Assets.Scripts.EnumTypes;
using Assets.Scripts.GameElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	// Planning Agent is the over-head planner that decided where
	// individual units go and what tasks they perform.  Low-level
	// AI is handled by other classes (like pathfinding).
	public class PlanningAgent : Agent
	{
        #region Private Variables
        // variables for myOptions (heuristic values)
        private List<float> myOptions;

        //dictionary for grid positions 
       //Dictionary<Vector3Int, <Vector3Int, float>> distanceMap = 

       // private float goldNeeded;
        private bool isSavingBase;
        private bool isSavingWorker;
        private bool isSavingBarracks;
        private bool isSavingRefinery;
        private bool isSavingArcher; 
        private bool isSavingSoldier;
        
        //initial build phase for refinery
        private bool buildInitial;

        //attack states, defend and attack 
        private bool isAttackStarted;
        private int fullArmy;
        private bool needsDefence;
        private int defenceRange;
        private List<int> incomingTroops;

        //magic numbers for heuristics 
        private int num_Refineries;
        private int num_Workers;
        private int num_Bases;
        private int num_Barracks;
        private int archerArmyFull;
        private int soldierArmyFull;

        //magic number for actions
        //ADD: work on dealing with to much gold
        #endregion


        #region Private Methods

        // Handy short-cuts for pulling all of the relevant data that you
        // might use for each decision.  Feel free to add your own.
        public int enemyAgentNbr { get; set; }
		public int mainMineNbr { get; set; }
		public int mainBaseNbr { get; set; }
		public bool lastFighterWasSoldier { get; set; }

		public List<int> mines { get; set; }

		public List<int> myWorkers { get; set; }
		public List<int> mySoldiers { get; set; }
		public List<int> myArchers { get; set; }
		public List<int> myBases { get; set; }
		public List<int> myBarracks { get; set; }
		public List<int> myRefineries { get; set; }

		public List<int> enemyWorkers { get; set; }
		public List<int> enemySoldiers { get; set; }
		public List<int> enemyArchers { get; set; }
		public List<int> enemyBases { get; set; }
		public List<int> enemyBarracks { get; set; }
		public List<int> enemyRefineries { get; set; }

		public List<Vector3Int> buildPositions { get; set; }

		/// <summary>
		/// Finds all of the possible build locations for a specific UnitType.
		/// Currently, all structures are 3x3, so these positions can be reused
		/// for all structures (Base, Barracks, Refinery)
		/// Run this once at the beginning of the game and have a list of
		/// locations that you can use to reduce later computation.  When you
		/// need a location for a build-site, simply pull one off of this list,
		/// determine if it is still buildable, determine if you want to use it
		/// (perhaps it is too far away or too close or not close enough to a mine),
		/// and then simply remove it from the list and build on it!
		/// This method is called from the Awake() method to run only once at the
		/// beginning of the game.
		/// </summary>
		/// <param name="unitType">the type of unit you want to build</param>
		public void FindProspectiveBuildPositions(UnitType unitType)
		{
			// For the entire map
			for (int i = 0; i < GameManager.Instance.MapSize.x; ++i)
			{
				for (int j = 0; j < GameManager.Instance.MapSize.y; ++j)
				{
					// Construct a new point near gridPosition
					Vector3Int testGridPosition = new Vector3Int(i, j, 0);

					// Test if that position can be used to build the unit
					if (Utility.IsValidGridLocation(testGridPosition)
						&& GameManager.Instance.IsBoundedAreaBuildable(unitType, testGridPosition))
					{
						// If this position is buildable, add it to the list
						buildPositions.Add(testGridPosition);
					}
				}
			}
		}

		/// <summary>
		/// Assuming you run the FindProspectiveBuildPositions, this method takes that
		/// list and finds the closest build position to the gridPosition.  This can be
		/// used to find a position close to a mine, close to the enemy base, close to
		/// your barracks, close to your base, close to a troop, etc.
		/// </summary>
		/// <param name="gridPosition">position that you want to build near</param>
		/// <param name="unitType">type of unit you want to build</param>
		/// <returns></returns>
		public Vector3Int FindClosestBuildPosition(Vector3Int gridPosition, UnitType unitType)
		{
			// Variables to store the closest position as we find it
			float minDist = float.MaxValue;
			Vector3Int minBuildPosition = gridPosition;

			// For all the possible build postions that we already found
			foreach (Vector3Int buildPosition in buildPositions)
			{
				// if the distance to that build position is closer than any other seen so far
				if (Vector3.Distance(gridPosition, buildPosition) < minDist && GameManager.Instance.IsBoundedAreaBuildable(unitType, buildPosition))
				{
					// Store this build position as the closest seen so far
					minDist = Vector3.Distance(gridPosition, buildPosition);
					minBuildPosition = buildPosition;
				}
			}

			// Return the closest build position
			return minBuildPosition;
		}

		/// <summary>
		/// Find the closest unit to the gridPosition out of a list of units.
		/// Use this method to find the enemy soldier closest to your archer,
		/// or the closest base to a mine, or the closest mine to a base, etc.
		/// </summary>
		/// <param name="gridPosition">position of an agent or base</param>
		public int FindClosestUnit(Vector3Int gridPosition, List<int> unitNbrs)
		{
			// Variables to store the closest unit as we find it
			int closestUnitNbr = -1;
			float closestUnitDist = float.MaxValue;

			// Iterate through all of the units
			foreach (int unitNbr in unitNbrs)
			{
				Unit unit = GameManager.Instance.GetUnit(unitNbr);
				float unitDist = Vector3.Distance(unit.GridPosition, gridPosition);

				// If this object is closer than any seen so far, save it
				if (!(unitDist < closestUnitDist)) continue;
				closestUnitDist = unitDist;
				closestUnitNbr = unitNbr;
			}

			// Return the closest unit's number
			return closestUnitNbr;
		}

        //The following methods compute the heuristic values 

        //should we build a base
        private float Compute_BuildBase()
        {
            if (Gold >= Constants.COST[UnitType.BASE] && myBases.Count() < num_Bases) // if we have enough gold and no base then: return 1
            {
                return 1;
            }
            else if (Gold < Constants.COST[UnitType.BASE] && myBases.Count() < num_Bases) // if we don't have enough gold and we don't have a base then save: return 0
            {
                isSavingBase = true;
                return 0;
            }
            else // if we have a base with or without enough gold: return 0
            {
                isSavingBase = false;
                return 0;
            }
        }

        //should we build a barracks
        private float Compute_BuildBarracks()
        {
            if (Gold >= Constants.COST[UnitType.BARRACKS]) // we have enough gold to build barracks, return: should we build barracks
            {
                return 1 - (myBarracks.Count() / (myBarracks.Count() + enemyBarracks.Count() + 1));
            }
            else
            {
                isSavingBarracks = true;
                return 0;
            }
        }

        //should we build a refinery
        private float Compute_BuildRefinery()
        {
            //if (buildInitial && Gold >= Constants.COST[UnitType.REFINERY]) //TEST
            //{
            //    return 1;
            //}

            if (Gold >= Constants.COST[UnitType.REFINERY]) // we have enough gold to build refinery, return: should we build refinery
            {
                return 1 - (myRefineries.Count() / num_Refineries);
            }
            else
            {
                isSavingRefinery = true;
                return 0;
            }
        }

        //should we train a worker
        private float Compute_TrainWorker()
        {
            if (Gold >= Constants.COST[UnitType.WORKER] && myBases.Count() >= num_Bases && myWorkers.Count < num_Workers) // if we have enough gold to train a worker and we have a base and we have less than 7 (tweak) workers, return: should we train
            {
                return 1 - (myWorkers.Count() / (enemyWorkers.Count() + 1)); //+ myWorkers.Count() + 2)); // try to maintian 2 more workers than opponent
            }
            else // we don't have enough gold for a worker
            {
                if (enemyWorkers.Count() > myWorkers.Count()) // if opponent has more workers then save for a worker
                {
                   isSavingWorker = true;
                }
                else
                {
                   isSavingWorker = false;
                }
                return 0;
            }
        }

        //should we train archer 
        private float Compute_TrainArcher()
        {
            if (Gold >= Constants.COST[UnitType.ARCHER] && myBarracks.Count() >= num_Barracks) // if we have enough gold, return: should we train archer
            {
                return 1 - (myArchers.Count() / (enemySoldiers.Count() + 3)); // train archers for the enemy soldiers, try to have 1 more archers than enemy soldiers
            }
            else
            {
                isSavingArcher = true;
                return 0;
            }
        }

        //should we train soldier
        private float Compute_TrainSoldier()
        {
            if (Gold >= Constants.COST[UnitType.SOLDIER] && myBarracks.Count() >= num_Barracks) // if we have enough gold, return: should we train soldier
            {
                return 1 - (mySoldiers.Count() / (enemyArchers.Count() + 2)); // train soldiers for the enemy archers, try to have 3 more soldiers than enemy archers
            }
            else
            {
                isSavingSoldier = true;
                return 0;
            }
        }

        //should we attack opponent
        private float Compute_Attack()
        {
            // We won't attack unless we have more troops than opponent or the troop count matches desired troop count
            if (myArchers.Count() > 0 || mySoldiers.Count() > 0)
            {
                if (isAttackStarted)
                {
                    return 1;
                }
                else if (myArchers.Count() >= archerArmyFull && mySoldiers.Count() >= soldierArmyFull) // if we have 10 archers and 5 soldiers then attack (FULL ARMY)
                {
                    isAttackStarted = true;
                    return 1;
                }
                else // more likely the greater the difference between my troops and opponents 
                {
                    //return ((mySoldiers.Count() + myArchers.Count()) / (enemySoldiers.Count() + enemyArchers.Count() + 3)); // more likely to attack if we have 3 more troops than enemy
                    return (mySoldiers.Count() + myArchers.Count()) / (mySoldiers.Count() + myArchers.Count() + enemySoldiers.Count() + enemyArchers.Count() + 1);
                    // return 1 - (enemySoldiers.Count() + enemyArchers.Count()) / (mySoldiers.Count() + myArchers.Count() + 1);
                }
            }
            else
            {
                return 0;
            }
        }

        //should we defend (work on for second iteration)
        private float Compute_Defence()
        {
            foreach (int worker in myWorkers) // check each worker to see if they need defence
            {
                Unit unit = GameManager.Instance.GetUnit(worker);

                if (unit.Health < Constants.HEALTH[UnitType.WORKER]) //check workers health and defend if necessary
                {
                    needsDefence = true;
                    return 1;
                }
            }

            foreach (int building in myBases) // check each base to see if they need defence
            {
                Unit unit = GameManager.Instance.GetUnit(building);

                if (unit.Health < Constants.HEALTH[UnitType.BASE]) //check workers health and defend if necessary
                {
                    needsDefence = true;
                    return 1;
                }
            }

            foreach (int building in myBarracks) // check each barracks to see if they need defence
            {
                Unit unit = GameManager.Instance.GetUnit(building);

                if (unit.Health < Constants.HEALTH[UnitType.BARRACKS]) //check workers health and defend if necessary
                {
                    needsDefence = true;
                    return 1;
                }
            }

            needsDefence = false;
            return 0;
        }

        //should we move
        private float Compute_Move()
        {
            int closeTroops = 0;
            if (mySoldiers.Count() > 0 || myArchers.Count() > 0)
            {
                // If more of my troops are closer to my base than not (within distance), move
                foreach (int archer in myArchers)
                {
                    Unit unit = GameManager.Instance.GetUnit(archer);
                    // if troop is closer to my base then the enemies, add it to the amount of troops closer to my base than enemies
                    if (unit != null && enemyBases.Count() > 0 && myBases.Count() > 0 && (unit.WorldPosition - GameManager.Instance.GetUnit(mainBaseNbr).GridPosition).magnitude < (GameManager.Instance.GetUnit(enemyBases[0]).GridPosition - GameManager.Instance.GetUnit(mainBaseNbr).WorldPosition).magnitude)
                    {
                        closeTroops++;
                    }
                }

                foreach (int soldier in mySoldiers)
                {
                    Unit unit = GameManager.Instance.GetUnit(soldier);
                    // if troop is closer to my base then the enemies, add it to the amount of troops closer to my base than enemies
                    if (unit != null && enemyBases.Count() > 0 && myBases.Count() > 0 && (unit.WorldPosition - GameManager.Instance.GetUnit(mainBaseNbr).WorldPosition).magnitude < (GameManager.Instance.GetUnit(enemyBases[0]).WorldPosition - GameManager.Instance.GetUnit(mainBaseNbr).WorldPosition).magnitude)
                    {
                        closeTroops++;
                    }
                }
                return (closeTroops / (mySoldiers.Count() + myArchers.Count() + 1));
            }
            else
            {
                return .001f;
            }
            //return 1 - (closeTroops / (mySoldiers.Count() + myArchers.Count() + 1));
            
          //  return .001f;
        }

        //should we gather
        private float Compute_Gather()
        {
            int gatheringUnits = 0;
            int buildingUnits = 0;

            foreach (int worker in myWorkers)
            {
                Unit unit = GameManager.Instance.GetUnit(worker);
                if (unit != null)
                {
                    if (unit.CurrentAction == UnitAction.GATHER)
                    {
                        gatheringUnits++;
                    }
                    else
                    {
                        buildingUnits++;
                    }
                }
            }
            
            if (Gold <= Constants.COST[UnitType.WORKER]) // take out?, if we don't have enough gold to train a new worker then, gather gold
            {
                return 1;
            }
            else if (isSavingBase) // if we are saving for a base
            {
                return 1 - (Gold / Constants.COST[UnitType.BASE]);
            }
            else if (isSavingBarracks) //if we are saving for a barracks
            {
                return 1 - (Gold / Constants.COST[UnitType.BARRACKS]);
            }
            else if (isSavingRefinery) // if we are saving for a refinery
            {
                return 1 - (Gold / Constants.COST[UnitType.REFINERY]);
            }
            else // we are saving for a troop
            {
                return 1 - (gatheringUnits / (buildingUnits + 1)); // return Constants.COST[UnitType.ARCHER] / Gold + 1; // 1 - Constants.COST[UnitType.ARCHER] / Gold + 1
                // return Gold / Constants.COST[UnitType.ARCHER];
            }

            // return Mathf.Max(1 - (goldNeeded / (Gold + 1)), 0) + .001f; 
        }


		#endregion

		#region Public Methods

		/// <summary>
		/// Called when the object is instantiated in the scene 
		/// </summary>
		public void Awake()
		{
            // variables for myOptions 
            myOptions = new List<float> { };
            isSavingBase = false;
            isSavingWorker = false;
            isSavingBarracks = false;
            isSavingRefinery = false;
            isSavingArcher = false;
            isSavingSoldier = false;

            //attack and defence variables 
            isAttackStarted = false; // start attack phase
            needsDefence = false; // defends against initial opponent attack
            fullArmy = 8; //Default: 10
            incomingTroops = new List<int> { };
            defenceRange = 80;

            //magic numbers 
            num_Refineries = 4; //default: 4 
            num_Workers = 7; //default: 7
            num_Bases = 1; //Default: 1
            num_Barracks = 1; //Default: 1
            buildInitial = true; // build initial refinery, possible use for barracks (initial build phase)
            archerArmyFull = 7; //Default: 7
            soldierArmyFull = 5; //Default: 5

            lastFighterWasSoldier = false;

			buildPositions = new List<Vector3Int>();

			FindProspectiveBuildPositions(UnitType.BASE);

			// Set the main mine and base to "non-existant"
			mainMineNbr = -1;
			mainBaseNbr = -1;

			// Initialize all of the unit lists
			mines = new List<int>();

			myWorkers = new List<int>();
			mySoldiers = new List<int>();
			myArchers = new List<int>();
			myBases = new List<int>();
			myBarracks = new List<int>();
			myRefineries = new List<int>();

			enemyWorkers = new List<int>();
			enemySoldiers = new List<int>();
			enemyArchers = new List<int>();
			enemyBases = new List<int>();
			enemyBarracks = new List<int>();
			enemyRefineries = new List<int>();
		}

		/// <summary>
		/// Updates the game state for the Agent - called once per frame for GameManager
		/// Pulls all of the agents from the game and identifies who they belong to
		/// </summary>
		public void UpdateGameState()
		{
			// Update the common resources
			mines = GameManager.Instance.GetUnitNbrsOfType(UnitType.MINE);

			// Update all of my unitNbrs
			myWorkers = GameManager.Instance.GetUnitNbrsOfType(UnitType.WORKER, AgentNbr);
			mySoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, AgentNbr);
			myArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, AgentNbr);
			myBarracks = GameManager.Instance.GetUnitNbrsOfType(UnitType.BARRACKS, AgentNbr);
			myBases = GameManager.Instance.GetUnitNbrsOfType(UnitType.BASE, AgentNbr);
			myRefineries = GameManager.Instance.GetUnitNbrsOfType(UnitType.REFINERY, AgentNbr);

			// Update the enemy agents & unitNbrs
			List<int> enemyAgentNbrs = GameManager.Instance.GetEnemyAgentNbrs(AgentNbr);
			if (enemyAgentNbrs.Any())
			{
				enemyAgentNbr = enemyAgentNbrs[0];
				enemyWorkers = GameManager.Instance.GetUnitNbrsOfType(UnitType.WORKER, enemyAgentNbr);
				enemySoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, enemyAgentNbr);
				enemyArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, enemyAgentNbr);
				enemyBarracks = GameManager.Instance.GetUnitNbrsOfType(UnitType.BARRACKS, enemyAgentNbr);
				enemyBases = GameManager.Instance.GetUnitNbrsOfType(UnitType.BASE, enemyAgentNbr);
				enemyRefineries = GameManager.Instance.GetUnitNbrsOfType(UnitType.REFINERY, enemyAgentNbr);
			}
		}

		// Update the GameManager - called once per frame
		public void Update()
		{
			UpdateGameState();

			// If we have at least one base, assume the first one is our "main" base
			if (myBases.Count > 0)
			{
				mainBaseNbr = myBases[0];
			}
			else
			{
				mainBaseNbr = -1;
			}

			// If we have a base, find the closest mine to the base
			if (mines.Count > 0 && mainBaseNbr >= 0)
			{
				Unit baseUnit = GameManager.Instance.GetUnit(mainBaseNbr);
				mainMineNbr = FindClosestUnit(baseUnit.GridPosition, mines);
			}

            //add the heuristic to the list, in priority order: Base, worker, barracks, archer, refinery, attack, move, soldier, gather (order subject to change)
            myOptions.Clear(); //clear list for new heuristic calculations
            myOptions.Add(Compute_BuildBase()); // build base heuristic
            myOptions.Add(Compute_TrainWorker()); // train worker heuristic
            myOptions.Add(Compute_BuildBarracks()); // build barracks heuristic
            myOptions.Add(Compute_TrainArcher()); // train arhcer heuristic
            myOptions.Add(Compute_BuildRefinery()); // build refinery heuristic
            myOptions.Add(Compute_Attack()); // Attack heuristic
            myOptions.Add(Compute_Defence()); // Defence heuristic work on for second iteration
            myOptions.Add(Compute_TrainSoldier()); // train soldier heuristic
            myOptions.Add(Compute_Move()); // Move heuristic
            myOptions.Add(Compute_Gather()); // gather heuristic

            float maxOptionValue = 0;
            int maxOption = 1 - myWorkers.Count();

            for (int i = 0; i < myOptions.Count(); i++)
            {
                if (myOptions[i] > maxOptionValue)
                {
                    maxOptionValue = myOptions[i];
                    maxOption = i;
                }
            }

            switch (maxOption)
            {
                case 0: // build base
                    Debug.Log("BUILD BASE");
                    foreach (int worker in myWorkers)
                    {
                        //grab unit 
                        Unit unit = GameManager.Instance.GetUnit(worker);

                        if (unit != null && unit.CurrentAction != UnitAction.BUILD)
                        {     
                            // Find the closest build position to the closest mine
                            // build the base there
                            mainMineNbr = FindClosestUnit(unit.GridPosition, mines);
                            Vector3Int toBuild = FindClosestBuildPosition(GameManager.Instance.GetUnit(mainMineNbr).GridPosition, UnitType.BASE);
                            if (toBuild != Vector3Int.zero)
                            {  
                                Build(unit, toBuild, UnitType.BASE);
                            }
                        }
                    }
                    break;

                case 1: // train worker
                    Debug.Log("TRAIN WORKER");
                    Unit mainBase = GameManager.Instance.GetUnit(mainBaseNbr);

                    if (mainBase != null && mainBase.CurrentAction == UnitAction.IDLE)
                    {
                        Train(mainBase, UnitType.WORKER); // have base train a worker
                    }
                    break;

                case 2: // build barracks
                    Debug.Log("BUILD BARRACKS");
                    foreach (int worker in myWorkers)
                    {
                        Unit unit = GameManager.Instance.GetUnit(worker);

                        if (unit != null && unit.CurrentAction == UnitAction.IDLE || unit.CurrentAction == UnitAction.GATHER)
                        {
                            //Find closest build position to the friendly base (may change to the middle position between friendly and enemy bases)
                            //build barracks there
                            int buildNumber = FindClosestUnit(unit.GridPosition, myBases);
                            Vector3Int toBuild = FindClosestBuildPosition(GameManager.Instance.GetUnit(buildNumber).GridPosition, UnitType.BARRACKS);
                            if (toBuild != Vector3Int.zero)
                            {
                                Build(unit, toBuild, UnitType.BARRACKS);
                            }
                        }
                    }
                    break;

                case 3: // train archer
                    Debug.Log("TRAIN ARCHER");
                    foreach (int barrack in myBarracks)
                    {
                        Unit unit = GameManager.Instance.GetUnit(barrack);

                        if (unit != null && unit.CurrentAction != UnitAction.TRAIN) // get all barracks not training someone 
                        {
                            Train(unit, UnitType.ARCHER);
                        }
                    }
                    break;

                case 4: // build refinery 
                    Debug.Log("BUILD REFINERY");
                    foreach (int worker in myWorkers)
                    {
                        Unit unit = GameManager.Instance.GetUnit(worker);

                        if (unit != null && unit.CurrentAction == UnitAction.IDLE || unit.CurrentAction == UnitAction.GATHER)
                        {
                            //Find closest build position to the friendly base (may change to the middle position between friendly and enemy bases)
                            //build barracks there
                            int unitNumber = FindClosestUnit(unit.GridPosition, myBases);
                            Vector3Int toBuild = FindClosestBuildPosition(GameManager.Instance.GetUnit(unitNumber).GridPosition, UnitType.REFINERY);
                            if (toBuild != Vector3Int.zero && Gold > Constants.COST[UnitType.REFINERY])
                            {
                               // buildInitial = false;
                                Build(unit, toBuild, UnitType.REFINERY);
                            }
                        }
                    }
                    break;

                case 5: // attack opponent
                    Debug.Log("ATTACK");

                    List<int> unitsArcher = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, AgentNbr);
                    List<int> unitsSoldier = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, AgentNbr);

                    // (FULL ARMY) send 7 archers to enemy base then barracks then refineries, then soldiers then archers
                    if (myArchers.Count() >= 7 && mySoldiers.Count() >= 5) 
                    {
                        for (int i = 0; i < myArchers.Count(); i++)
                        {
                            Unit unit = GameManager.Instance.GetUnit(unitsArcher[i]);
                            if (unit != null && unit.CurrentAction != UnitAction.ATTACK) // we have usable agent
                            {
                                if (enemyBases.Count() > 0) // if enemy has a base, attack random base
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBases)));
                                }
                                else if (enemyBarracks.Count() > 0) // if enemy has a barracks, attack random barracks
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBarracks)));
                                }
                                else if (enemyRefineries.Count() > 0) // if enemy has a refineries, refinery
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyRefineries)));
                                }
                                else if (enemyWorkers.Count() > 0) // if enemy has workers, attack random worker
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyWorkers)));
                                }
                                else if (enemySoldiers.Count() > 0) // if enemy has a soldiers, attack random soldier
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemySoldiers)));
                                }
                                else if (enemyArchers.Count() > 0) // if enemy has a archers, attack random archer
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyArchers)));
                                }
                            }

                        }

                        for (int i = 0; i < mySoldiers.Count(); i++) //send 5 soldiers to enemy archers then soldiers then barracks then base then refineries
                        {
                            Unit unit = GameManager.Instance.GetUnit(unitsSoldier[i]);
                            if (unit != null && unit.CurrentAction != UnitAction.ATTACK) // we have usable agent
                            {
                                if (enemyWorkers.Count() > 0) // if enemy has workers, attack random worker
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyWorkers)));
                                }
                                else if (enemyArchers.Count() > 0) // if enemy has archers, attack random archer 
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyArchers)));
                                }
                                else if (enemySoldiers.Count() > 0) // if enemy has soldiers, attack random soldiers
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemySoldiers)));
                                }
                                else if (enemyBarracks.Count() > 0) // if enemy has barracks, attack random barracks
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBarracks)));
                                }
                                else if (enemyBases.Count() > 0) // if enemy has bases, attack random base
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBases)));
                                }
                                else if (enemyRefineries.Count() > 0) // if enemy has refineries, attack random refinery
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyRefineries)));
                                }
                            }
                        }
                    }
                    else //if (isAttackStarted) we do not have a full army 7 archers and 5 soldiers, attack enemy troops first
                    {
                        foreach (int soldier in mySoldiers) // send all soldiers to attack enemy troops first
                        {
                            Unit unit = GameManager.Instance.GetUnit(soldier);

                            if (unit != null && unit.CurrentAction != UnitAction.ATTACK) // if unit is usable 
                            {
                                if (enemySoldiers.Count() > 0) // if enemy has soldiers attack random soldier
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemySoldiers)));
                                }
                                else if (enemyArchers.Count() > 0) // if enemy has archers, attack random archer 
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyArchers)));
                                }
                                else if (enemyBarracks.Count() > 0) // if enemy has barracks, attack random barracks
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemySoldiers)));
                                }
                                else if (enemyBases.Count() > 0) // if enemy has bases, attack random base
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBases)));
                                }
                                else if (enemyRefineries.Count() > 0) // if enemy has refineries, attack random refinery
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyRefineries)));
                                }
                            }
                        }

                        foreach (int archer in myArchers) // send all archers to attack enemy troops first
                        {
                            Unit unit = GameManager.Instance.GetUnit(archer);

                            if (unit != null && unit.CurrentAction != UnitAction.ATTACK) // if unit is usable 
                            {
                                if (enemySoldiers.Count() > 0) // if enemy has soldiers attack random soldier
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemySoldiers)));
                                }
                                else if (enemyArchers.Count() > 0) // if enemy has archers, attack random archer 
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyArchers)));
                                }
                                else if (enemyBarracks.Count() > 0) // if enemy has barracks, attack random barracks
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBarracks)));
                                }
                                else if (enemyBases.Count() > 0) // if enemy has bases, attack random base
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyBases)));
                                }
                                else if (enemyRefineries.Count() > 0) // if enemy has refineries, attack random refinery
                                {
                                    Attack(unit, GameManager.Instance.GetUnit(FindClosestUnit(unit.GridPosition, enemyRefineries)));
                                }
                            }
                        }
                    }
                    break;


                case 6:
                    Debug.Log("DEFEND");
                    List<int> unitArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, AgentNbr);
                    List<int> unitSoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, AgentNbr);

                    // defend workers (send 3 archers and 3 soldiers)
                    if (needsDefence)
                    {
                        incomingTroops.Clear(); //clear the list of incoming troops, to find the new incoming troops
                        List<int> incomingSoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, enemyAgentNbr);
                        List<int> incomingArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, enemyAgentNbr);

                        foreach (int enemySoldier in incomingSoldiers) // find enemy soldiers close to my base
                        {
                            Unit enemyUnit = GameManager.Instance.GetUnit(enemySoldier);
                            if (enemyUnit != null && myBases.Count() > 0 && (enemyUnit.GridPosition - GameManager.Instance.GetUnit(mainBaseNbr).GridPosition).magnitude <= defenceRange)
                            {
                                incomingTroops.Add(enemySoldier);
                            }
                        }

                        foreach (int enemyArcher in incomingArchers) //find enemy archers close to my base
                        {
                            Unit enemyUnit = GameManager.Instance.GetUnit(enemyArcher);
                            if (enemyUnit != null && myBases.Count() > 0 && (enemyUnit.GridPosition - GameManager.Instance.GetUnit(mainBaseNbr).GridPosition).magnitude <= defenceRange)
                            {
                                incomingTroops.Add(enemyArcher);
                            }
                        }

                        if (incomingTroops.Count() < fullArmy && myArchers.Count() >= 3) // if incoming troops is less than a full army, only send a few troops 
                        {
                            for (int i = 0; i < 3; i++) // send 3 archers and soldier to defend workers
                            {
                                Unit archer = GameManager.Instance.GetUnit(unitArchers[i]);
                                Unit soldier = GameManager.Instance.GetUnit(unitSoldiers[i]);
                                if (archer != null && archer.CurrentAction != UnitAction.ATTACK) //send archers to defend
                                {
                                    Attack(archer, GameManager.Instance.GetUnit(FindClosestUnit(archer.GridPosition, incomingTroops)));
                                }

                                if (soldier != null && soldier.CurrentAction != UnitAction.ATTACK) //send soldiers to defend
                                {
                                    Attack(soldier, GameManager.Instance.GetUnit(FindClosestUnit(soldier.GridPosition, incomingTroops)));
                                }
                            }
                        }
                        else if (incomingTroops.Count() >= fullArmy) // if incoming troops is a full army, send all troops and start attack
                        {
                            isAttackStarted = true; 
                        }
                    }
                    needsDefence = false; // reset defence
                    break;

                case 7: // train soldier
                    Debug.Log("TRAIN SOLDIER");
                    foreach (int barrack in myBarracks)
                    {
                        Unit unit = GameManager.Instance.GetUnit(barrack);

                        if (unit != null && unit.CurrentAction != UnitAction.TRAIN) // get all barracks not training someone 
                        {
                            Train(unit, UnitType.SOLDIER);
                        }
                    }
                    break;

                case 8: // move
                    Debug.Log("MOVE");

                    //move archers to either base or barracks (random)
                    int moveLocation = Random.Range(0, 1);
                    foreach (int archer in myArchers) // move all archers to the center
                    {
                        Unit unit = GameManager.Instance.GetUnit(archer);

                        if (unit != null && unit.CurrentAction != UnitAction.ATTACK)
                        {
                            if (myBases.Count() > 0 && moveLocation == 0) // move to base
                            {
                                Unit moveBase = GameManager.Instance.GetUnit(mainBaseNbr);
                                Move(unit, moveBase.GridPosition + new Vector3Int(1, 1, 0)); //TEST
                            }
                            else if (myBarracks.Count() > 0 && moveLocation == 1) //move to random barracks 
                            {
                                Unit moveBarracks = GameManager.Instance.GetUnit(myBarracks[UnityEngine.Random.Range(0, myBarracks.Count)]);
                                Move(unit, moveBarracks.GridPosition + new Vector3Int(1, 1, 0));
                            }
                        }
                    }

                    //move soldiers to either barracks or base
                    foreach (int soldier in mySoldiers) // move all archers to the center
                    {
                        Unit unit = GameManager.Instance.GetUnit(soldier);

                        if (unit != null && unit.CurrentAction != UnitAction.ATTACK)
                        {
                            if (myBases.Count() > 0 && moveLocation == 0) // move to base
                            {
                                Unit moveBase = GameManager.Instance.GetUnit(mainBaseNbr);
                                Move(unit, moveBase.GridPosition + new Vector3Int(1, 1, 0));
                            }
                            else if (myBarracks.Count() > 0 && moveLocation == 1) //move to random barracks 
                            {
                                Unit moveBarracks = GameManager.Instance.GetUnit(myBarracks[UnityEngine.Random.Range(0, myBarracks.Count)]);
                                Move(unit, moveBarracks.GridPosition + new Vector3Int(1, 1, 0));
                            }
                        }
                    }
                    break;

                case 9: // gather 
                    Debug.Log("GATHER");
                    foreach (int worker in myWorkers)
                    {
                        Debug.Log("GATHER");
                        Unit unit = GameManager.Instance.GetUnit(worker);

                        if (unit != null && unit.CurrentAction == UnitAction.IDLE)
                        {
                            //get the mine and base for this agent
                            Unit mineUnit = GameManager.Instance.GetUnit(mainMineNbr);
                            Unit baseUnit = GameManager.Instance.GetUnit(mainBaseNbr);
                            if (mineUnit != null && baseUnit != null)
                            {
                                Gather(unit, mineUnit, baseUnit);
                            }
                        }
                    }
                    break;
            }
        }
		#endregion
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using WJ_MCTS;
using System.Linq;
using UnityEngine.Assertions;

namespace WJ_Controller
{
    public class MCTSBot : CharacterIA
    {
        private MCTSNode<GameState>[] arrayNode;
        private int currentSize = 0;
        public void Start()
        {
           arrayNode = new MCTSNode<GameState>[GameManager.Instance.NumbersNode+1];
        } 
        public override void UpdateBehaviour()
        {
            Actions(ComputeMCTS(gameState,GameManager.Instance.NumbersNode),gameState,factionId);
            /*if(currentSize > 0)
            {
                return;
            }
            ComputeMCTS(gameState,GameManager.Instance.NumbersNode);
            Debug.Log(RecurseDebug(arrayNode[0]));*/
        }

        public string RecurseDebug(MCTSNode<GameState> node,int depth = 0)
        {
            string result = string.Concat("",node.wi,"/",node.ni,node.list.Count > 0 ? "(" : ",");
            foreach(MCTSNode<GameState> n in node.list)
            {
                result += RecurseDebug(n,depth+1);
            }
            return result+(node.list.Count > 0 ? ")" : "");
        }

        public int ComputeMCTS(GameState gs,int numbersTest)
        {
            arrayNode[0] = new MCTSNode<GameState>(gs.copy());
            currentSize = 0;
            MCTSNode<GameState> selectedNode = null;
            MCTSNode<GameState> newNode = null;
            for(int i=0; i < numbersTest; i++)
            {
                selectedNode = Selection(arrayNode);
                newNode = Expand(arrayNode,selectedNode);
                Simulation(newNode);                
                BackPropagation(newNode);
            }
            return GetFirstAction(newNode);
        }

        public int GetFirstAction(MCTSNode<GameState> node)
        {
            MCTSNode<GameState> currentNode = node;
            while(currentNode.parent.parent != null)
            {
                currentNode = currentNode.parent;
            }
            return currentNode.action[factionId];
        }

        public List<int> GetPosibleAction(int fid,MCTSNode<GameState> node)
        {
            List<int> action = new List<int>();
            bool isNotUsed = false; 
            if(node.data.characterDatas[fid].handObject)
            {
                for (int i = 0; i < numberAction; i++)
                {
                    isNotUsed = true;
                    for(int j = 0; j < node.list.Count && isNotUsed;j++)
                    {
                        isNotUsed = node.list[j].action[fid] != i;
                    }
                    if(isNotUsed)
                    {
                        action.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    isNotUsed = true;
                    for(int j = 0; j < node.list.Count && isNotUsed;j++)
                    {
                        isNotUsed = node.list[j].action[fid] != i;
                    }
                    if(isNotUsed)
                    {
                        action.Add(i);
                    }
                }
            }
            return action;
        }

        public MCTSNode<GameState> Selection(MCTSNode<GameState>[] arrayNode)
        {
            if(GameManager.Instance.PercentExplorationExploitation >= Random.Range(0,100))
            {
                //Exploration
                return arrayNode[Random.Range(0,currentSize+1)];
            }
            else
            {
                //Exploitation
                int selectedNode = -1;
                float maxScore = float.MinValue;
                float calc;
                for (int i = 0; i < currentSize+1; i++)
                {
                    if(arrayNode[i].list.Count < numberAction)
                    {
                        calc = calculeScoreNode(arrayNode[i]);
                        if(calc > maxScore)
                        {
                            selectedNode = i;
                            maxScore = calc;
                        }
                    }
                }
                return selectedNode == -1 ? null : arrayNode[selectedNode];
            }
        }

        public float calculeScoreNode(MCTSNode<GameState> node)
        {
            if(node.ni == 0)
            {
                return 0.0f;
            }
            return node.wi/node.ni;//	wi/ni + c*sqrt(t)/ni
        }

        public MCTSNode<GameState> Expand(MCTSNode<GameState>[] arrayNode,MCTSNode<GameState> node)
        {        
            MCTSNode<GameState> newNode = node.Add(new MCTSNode<GameState>(node.data.copy()));            
            arrayNode[++currentSize] = newNode;
            List<int> valueLeft = GetPosibleAction(0,newNode);
            List<int> valueRight = GetPosibleAction(1,newNode);
            newNode.action[0] = valueLeft[Random.Range(0,valueLeft.Count)];
            newNode.action[1] = valueRight[Random.Range(0,valueRight.Count)];
            GameManager.Instance.Simulate(newNode.data,GameManager.Instance.DeltaBehaviour);
            return newNode;
        }

        public void Simulation(MCTSNode<GameState> node)
        {   
            int winLeftIteration = 0;
            int winRightIteration = 0;
            int maxIteration = 0;
            GameState gs = null;
            for(int i = 0; i < GameManager.Instance.NumberSimulation;i++)
            {
                maxIteration = GameManager.Instance.NumbersMaxIteration;
                gs = node.data.copy();
                while(node.data.GameManagerData.scoreLeft==gs.GameManagerData.scoreLeft && node.data.GameManagerData.scoreRight==gs.GameManagerData.scoreRight && !gs.GameManagerData.endSet  && maxIteration > 0)
                {
                    Actions(gs.characterDatas[0].handObject ? Random.Range(9,numberAction) : Random.Range(0, 9),gs,0);
                    Actions(gs.characterDatas[1].handObject ? Random.Range(9,numberAction) : Random.Range(0, 9),gs,1);
                    GameManager.Instance.Simulate(gs,GameManager.Instance.DeltaBehaviour);
                    maxIteration--;
                }
                if(gs.GameManagerData.scoreLeft != node.data.GameManagerData.scoreLeft)
                {
                    winLeftIteration++;
                }
                else
                {
                    winRightIteration++;
                }
            }
            if(Faction.Left == faction)
            {
                node.wi += winLeftIteration;    
                node.wi -= winRightIteration;                
            }
            else
            {
                node.wi += winRightIteration;    
                node.wi -= winLeftIteration;    
            }      
            node.ni+=GameManager.Instance.NumberSimulation*5;    
        }

        public void BackPropagation(MCTSNode<GameState> node)
        {
            MCTSNode<GameState> currentNode = node;
            while(currentNode.parent != null)
            {
                currentNode.parent.wi += node.wi; 
                currentNode.parent.ni += node.ni;
                currentNode = currentNode.parent;
            }
        }
    }
}
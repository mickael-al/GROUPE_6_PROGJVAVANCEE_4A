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
        private MCTSNode<GameState>[] arrayNodeUnfinished;
        private int currentSizeUnFinished = 0;
        private float maxScore;
        private int selectedNodeScore = -1;
        private float calc;
        private MCTSNode<GameState> selectedNode = null;
        public void Start()
        {
           arrayNode = new MCTSNode<GameState>[GameManager.Instance.NumbersNode+1];
           arrayNodeUnfinished = new MCTSNode<GameState>[GameManager.Instance.NumbersNode+1]; 
        } 
        public override void UpdateBehaviour()
        {
            if(gameState.GameManagerData.endSet)
            {
                return;
            }
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
            string result = string.Concat("",calculeScoreNode(node).ToString("F2"),node.list.Count > 0 ? "(" : ",");
            foreach(MCTSNode<GameState> n in node.list)
            {
                result += RecurseDebug(n,depth+1);
            }
            return result+(node.list.Count > 0 ? ")" : "");
        }

        public int ComputeMCTS(GameState gs,int numbersTest)
        {
            arrayNode[0] = new MCTSNode<GameState>(gs.copy());
            arrayNodeUnfinished[0] = arrayNode[0];
            currentSize = 0;
            currentSizeUnFinished = 0;
            selectedNode = null;
            MCTSNode<GameState> newNode = null;
            for(int i=0; i < numbersTest; i++)
            {
                selectedNode = Selection();
                Assert.IsTrue(selectedNode != null);
                newNode = Expand(arrayNode,selectedNode);
                Simulation(newNode);                
                BackPropagation(newNode);
            }
            return GetFirstBestAction();
        }

        public int GetFirstBestAction()
        {
            maxScore = float.MinValue;
            selectedNodeScore = -1;
            for(int i = 0; i < arrayNode[0].list.Count;i++)
            {
                calc = calculeScoreNode(arrayNode[0].list[i]);
                if(calc > maxScore)
                {
                    maxScore = calc;
                    selectedNodeScore = i;
                }
            }
            //Debug.Log(arrayNode[0].list[selectedNodeScore].action[factionId]);
            //Assert.IsTrue(arrayNode[0].list[selectedNodeScore].action[factionId] != -1);
            return arrayNode[0].list[selectedNodeScore].action[factionId];
        }

        public List<int> GetPosibleAction(int fid,MCTSNode<GameState> node)
        {
            List<int> action = new List<int>();
            bool isNotUsed = false; 
            if(node.data.characterDatas[fid].handObject)
            {
                for (int i = 9; i < numberAction; i++)
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

        public MCTSNode<GameState> Selection()
        {
            if(GameManager.Instance.PercentExplorationExploitation >= Random.Range(0,100))
            {
                return arrayNodeUnfinished[Random.Range(0,currentSizeUnFinished+1)];
            }
            else
            {
                //Exploitation
                selectedNodeScore = -1;
                maxScore = float.MinValue;
                for (int i = 0; i < currentSizeUnFinished+1; i++)
                {
                    if(arrayNodeUnfinished[i].list.Count < numberAction)
                    {
                        calc = calculeScoreNode(arrayNodeUnfinished[i]);
                        if(calc > maxScore)
                        {
                            selectedNodeScore = i;
                            maxScore = calc;
                        }
                    }
                }
                return selectedNodeScore == -1 ? null : arrayNodeUnfinished[selectedNodeScore];
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

        public MCTSNode<GameState>  Expand(MCTSNode<GameState>[] arrayNode,MCTSNode<GameState> node)
        {        
            MCTSNode<GameState> newNode = node.Add(new MCTSNode<GameState>(node.data.copy()));            
            arrayNode[++currentSize] = newNode;
            List<int> valueLeft = GetPosibleAction(0,newNode);
            List<int> valueRight = GetPosibleAction(1,newNode);
            newNode.action[0] = valueLeft[Random.Range(0,valueLeft.Count)];
            newNode.action[1] = valueRight[Random.Range(0,valueRight.Count)];
            GameManager.Instance.Simulate(newNode.data,GameManager.Instance.DeltaBehaviour);
            if(!newNode.data.GameManagerData.endSet)
            {
                arrayNodeUnfinished[++currentSizeUnFinished] = newNode;
            }
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
                else if(gs.GameManagerData.scoreRight != node.data.GameManagerData.scoreRight)
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
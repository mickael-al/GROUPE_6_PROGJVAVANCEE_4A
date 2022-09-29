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
        public override void UpdateBehaviour()
        {
            Actions(ComputeMCTS(gameState,GameManager.Instance.NumbersTest),gameState,factionId);
        }

        public int ComputeMCTS(GameState gs,int numbersTest)
        {
            MCTSNode<GameState>[] arrayNode = new MCTSNode<GameState>[numbersTest+1];
            arrayNode[0] = new MCTSNode<GameState>(gs);
            int size = 1;
            MCTSNode<GameState> selectedNode = null;
            MCTSNode<GameState> newNode = null;
            for(int i=0; i < numbersTest; ++i)
            {
                selectedNode = Selection(arrayNode,size);
                //Assert.IsNotNull(selectedNode);
                Debug.Log("1");
                newNode = Expand(arrayNode,selectedNode,ref size);
                Debug.Log("2");
                Simulation(newNode);
                Debug.Log("3");
                BackPropagation(newNode,newNode.wi,newNode.ni);
                Debug.Log("4");
            }
            return GetFirstAction(newNode);
        }

        public int GetFirstAction(MCTSNode<GameState> node)
        {
            if(node == null)
            {
                return -1;
            }
            MCTSNode<GameState> currentNode = node;
            while(currentNode.parent.parent != null)
            {
                currentNode = node.parent;
            }
            return currentNode.action[factionId];
        }

        public List<int> GetPosibleAction(int fid,MCTSNode<GameState> node)
        {
            List<int> action = new List<int>();
            if(node.data.characterDatas[fid].handObject)
            {
                for (int i = 0; i < numberAction; i++)
                {
                    if(!node.list.Any(x => x.action[fid] == i))
                    {
                        action.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if(!node.list.Any(x => x.action[fid] == i))
                    {
                        action.Add(i);
                    }
                }
            }
            return action;
        }

        public MCTSNode<GameState> Selection(MCTSNode<GameState>[] arrayNode,int size)
        {
            if(GameManager.Instance.PercentExplorationExploitation >= Random.Range(0,100))
            {
                //Exploration
                return arrayNode[Random.Range(0,size)];
            }
            else
            {
                //Exploitation
                int selectedNode = -1;
                float maxScore = float.MinValue;
                float calc;
                for (int i = 0; i < size; i++)
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

        public MCTSNode<GameState> Expand(MCTSNode<GameState>[] arrayNode,MCTSNode<GameState> node, ref int size)
        {        
            size++;
            MCTSNode<GameState> newNode = node.Add(new MCTSNode<GameState>(node.data));
            List<int> valueLeft = GetPosibleAction((int)Faction.Left,node);
            List<int> valueRight = GetPosibleAction((int)Faction.Right,node);
            //Assert.IsFalse(valueLeft.Count == 0 || valueRight.Count == 0);
            newNode.action[0] = valueLeft[Random.Range(0,valueLeft.Count)];
            newNode.action[1] = valueRight[Random.Range(0,valueRight.Count)];
            GameManager.Instance.Simulate(newNode.data,GameManager.Instance.DeltaBehaviour);            
            arrayNode[size] = newNode;
            return newNode;
        }

        public void Simulation(MCTSNode<GameState> node)
        {   
            int winLeftIteration = 0;
            int winRightIteration = 0;
            for(int i = 0; i < GameManager.Instance.NumberSimulation;i++)
            {
                int maxIteration = 100;
                GameState gs = node.data.FullCopy();
                while(node.data.GameManagerData.scoreLeft==gs.GameManagerData.scoreLeft && node.data.GameManagerData.scoreRight==gs.GameManagerData.scoreRight && !gs.GameManagerData.endSet  && maxIteration > 0)
                {       
                    Actions(Random.Range(0,node.data.characterDatas[0].handObject ? numberAction : 9),gs,0);
                    Actions(Random.Range(0,node.data.characterDatas[1].handObject ? numberAction : 9),gs,1);
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
            node.ni++;    
        }

        public void BackPropagation(MCTSNode<GameState> node,float wi,float ni)
        {
            MCTSNode<GameState> currentNode = node;
            while(currentNode.parent != null)
            {
                currentNode.parent.wi += wi; 
                currentNode.parent.ni += ni;
                currentNode = node.parent;
            }
        }
    }
}
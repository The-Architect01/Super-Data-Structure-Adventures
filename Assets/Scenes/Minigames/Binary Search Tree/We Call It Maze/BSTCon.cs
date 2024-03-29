using BinaryTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BSTCon : Minigame {

    public Text LeftText;
    public Text RightText;
    public Text CurrentText;
    public Text GoalText;
    public Text MovesUsed;
    public static BinaryTree<string> BinTree;
    public static string Goal;
    public CountDown Timer;

    static int _moves;
    int Moves { get { return _moves; } set { _moves = value; MovesUsed.text = $"Moves Used: {value}"; } }

    public static Node<string> CurrentNode = null;

    public Direction Direction;

    private void Start() {
        base.Start();
        Log = new GameLogging.Log() {
            IsPractice = Zombie.IsPractice,
            IsTeamGame = !Zombie.IsSolo,
            ErrorsMade = -1,
            TurnsUsed = 0,
            TimeTaken = 0,
        };

        Moves = 0;
        BinTree = new BinaryTree<string>();
        List<string> Goals = new List<string>();

        SetItems.SetArgs args = SetItems.SetArgs.Empty;
        if (Random.value > .5f) { args = SetItems.SelectSet(); }
        while (BinTree.Count < 25) {
            if(!args.Equals(SetItems.SetArgs.Empty)) { 
                string x = args.Set.ElementAt(Random.Range(0, args.Set.Count-1));
                args.Set.Remove(x);
                BinTree.Insert(x);
                Goals.Add(x);
            } else {
                string x = Random.Range(1, 999).ToString();
                BinTree.Insert(x, x.Length);
                Goals.Add(x);
            }
        }
        Goal = Goals[Random.Range(0, Goals.Count-1)].ToString();
        GoalText.text = $"Goal: {Goal}";
        Moves = 0;

        CurrentText.text = BinTree.Root.Data.ToString();
        try { LeftText.text = BinTree.Root.Left.Data.ToString(); } catch { LeftText.text = ""; }
        try { RightText.text = BinTree.Root.Right.Data.ToString(); } catch { RightText.text = ""; }
        CurrentNode = BinTree.Root;
    }

    public void OnEnter() {
        GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void OnExit() {
        GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    public void OnClick() {
        Moves++;
        Log.TurnsUsed++;
        if (Direction == Direction.Right) {
            if(RightText.text == Goal.ToString()) {
                //GameWin.Show();
                //Log.TimeTaken = Timer.TimeElapsed;
                //Zombie.CurrentProfileStats.Stats["Binary Search Tree"]["We Call It Maze"].GameLog.Add(Log);
                Win();
                return;
            }
#if !UNITY_WEBGL
            if (CurrentNode.Right != null)
#else
            try {
            if (CurrentNode.Right.Data != "")
#endif 
                CurrentNode = CurrentNode.Right;
            else
                MDM.RegisterError();
#if UNITY_WEBGL
            } catch {
                MDM.RegisterError();
            }
#endif

        } else if(Direction == Direction.Left) {
            if (LeftText.text == Goal.ToString()) {
                //GameWin.Show();
                //Log.TimeTaken = Timer.TimeElapsed;
                //Zombie.CurrentProfileStats.Stats["Binary Search Tree"]["We Call It Maze"].GameLog.Add(Log);
                Win();
                return;
            }
#if !UNITY_WEBGL
            if (CurrentNode.Left != null)
#else
            try {
            if (CurrentNode.Left.Data != "")
#endif                
                CurrentNode = CurrentNode.Left;
            else
                MDM.RegisterError();
#if UNITY_WEBGL
            } catch {
                MDM.RegisterError();
            }
#endif

        } else if(Direction == Direction.Center) {
            if (CurrentText.text == Goal.ToString()) {
                //GameWin.Show();
                //Log.TimeTaken = Timer.TimeElapsed;
                //Zombie.CurrentProfileStats.Stats["Binary Search Tree"]["We Call It Maze"].GameLog.Add(Log);
                Win();
                return; 
            }
#if !UNITY_WEBGL
            if (CurrentNode.Parent != null)
#else
            try {
            if (CurrentNode.Parent.Data != "")
#endif
                CurrentNode = CurrentNode.Parent;
            else
                MDM.RegisterError();
#if UNITY_WEBGL
            } catch {
                MDM.RegisterError();
            }
#endif 
        }
        CurrentText.text = CurrentNode.Data.ToString();
        try {
            LeftText.text = CurrentNode.Left.Data.ToString();
        } catch { LeftText.text = ""; }
        try {
            RightText.text = CurrentNode.Right.Data.ToString();
        }catch { RightText.text = ""; }
    }
}
namespace BinaryTree {

    public enum Direction { Center, Left, Right }

    public class Node<T> where T : System.IComparable<T> {
        public T Data { get; set; }
        public int NumberLength { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Parent { get; set; } = null;
        public bool IsLeaf { get {
                return Left == null && Right == null;
            }
        }

        public int CompareTo(Node<T> other) {
            if(NumberLength != other.NumberLength)
                return NumberLength - other.NumberLength;
            else
                return Data.CompareTo(other.Data);
        }

        public Node(T data, int numberLength, Node<T> Left, Node<T> Right, Node<T> Parent = null) {
            Data = data;
            this.Left = Left;
            this.Right = Right;
            this.Parent = Parent;
            this.NumberLength = numberLength;
        }
    }

    public class BinaryTree<T> where T : System.IComparable<T> {
        public Node<T> Root { get; private set; } = null;
        public int Count { get { return CountNodes(Root); } }

        public void Insert(T data, int numberLength = 0) {
            if(Root == null) {
                Root = new Node<T>(data, numberLength, null, null);
            } else {
                InsertSort(Root, new Node<T>(data, numberLength, null, null));
            }
        }
        
        void InsertSort(Node<T> Base, Node<T> InsertValue) {
            InsertValue.Parent = Base;
            if(InsertValue.CompareTo(Base) < 0) {
                if(Base.Left == null) {
                    Base.Left = InsertValue;
                } else {
                    InsertSort(Base.Left, InsertValue);
                }
            } else {
                if (Base.Right == null) {
                    Base.Right = InsertValue;
                } else {
                    InsertSort(Base.Right, InsertValue);
                }
            }
        }

        int CountNodes(Node<T> Root) {
            if(Root == null) { return 0; }
            return 1 + CountNodes(Root.Left) + CountNodes(Root.Right);
        }

        string Tostring(Node<T> node) {
            if(node == null) { return "T"; }
            return Tostring(node.Left) + " " + node.Data.ToString() + " " + Tostring(node.Right);
        } 

        public override string ToString() {
            return Tostring(Root);
        }

    }

}
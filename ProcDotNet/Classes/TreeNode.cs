using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet.Classes
{
    [Serializable, DataContract(IsReference = true)]
    public class TreeNode<ProcMon> : IEnumerable<TreeNode<ProcMon>>
    {

        [DataMember] public ProcMon Data { get; set; }
        [DataMember] public TreeNode<ProcMon> Parent { get; set; }
        [DataMember] public ICollection<TreeNode<ProcMon>> Children { get; set; }

        //?[DataMember]
        public bool IsRoot
        {
            get { return Parent == null; }
        }

        //[DataMember]
        public bool IsLeaf
        {
            get { return Children.Count == 0; }
        }

        //[DataMember]
        public int Level
        {
            get
            {
                if (IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }


        public TreeNode(ProcMon data)
        {
            Data = data;
            Children = new LinkedList<TreeNode<ProcMon>>();

            ElementsIndex = new LinkedList<TreeNode<ProcMon>>();
            ElementsIndex.Add(this);
        }

        public TreeNode<ProcMon> AddChild(ProcMon child)
        {
            TreeNode<ProcMon> childNode = new(child) { Parent = this };
            Children.Add(childNode);

            RegisterChildForSearch(childNode);
            return childNode;
        }

        public override string ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }


        #region searching

        private ICollection<TreeNode<ProcMon>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<ProcMon> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public TreeNode<ProcMon> FindTreeNode(Func<TreeNode<ProcMon>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        #endregion


        #region iterating

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode<ProcMon>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        internal TreeNode<ProcMon> AddChild(TreeNode<ProcMon> two)
        {
            Children.Add(two);
            return this;
        }
        #endregion
    }

    public interface ISerializableNode
    {
        object ToSerializableObject(Func<TreeNode<ProcMon>, bool> excludeCondition);
    }
}

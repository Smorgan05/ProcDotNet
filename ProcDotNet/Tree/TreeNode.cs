using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet.Tree
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        public bool IsLeaf
        {
            get { return Children.Count == 0; }
        }

        public int Level
        {
            get
            {
                if (IsRoot)
                    return 0;
                return Parent.Level + 1;
            }
        }


        public TreeNode(T data)
        {
            Data = data;
            Children = new LinkedList<TreeNode<T>>();

            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new(child) { Parent = this };
            Children.Add(childNode);

            RegisterChildForSearch(childNode);
            return childNode;
        }

        public override string ToString()
        {
            return Data != null ? Data.ToString() : "[data null]";
        }


        #region searching

        private ICollection<TreeNode<T>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        #endregion


        #region iterating

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        internal TreeNode<T> AddChild(TreeNode<T> two)
        {
            Children.Add(two);
            return this;
        }
        #endregion
    }
}

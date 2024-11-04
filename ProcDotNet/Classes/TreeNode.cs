using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProcDotNet.Classes
{
    [Serializable]
    public class JsonNode<ProcMon> : IEnumerable<JsonNode<ProcMon>>
    {

        public ProcMon Data { get; set; }
        public JsonNode<ProcMon> Parent { get; set; }
        public ICollection<JsonNode<ProcMon>> Children { get; set; }

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

        public JsonNode(ProcMon data)
        {
            Data = data;
            Children = new LinkedList<JsonNode<ProcMon>>();
            ElementsIndex = new LinkedList<JsonNode<ProcMon>>();
            ElementsIndex.Add(this);
        }

        public JsonNode()
        {
        }

        public JsonNode<ProcMon> AddChild(ProcMon child)
        {
            JsonNode<ProcMon> childNode = new(child) { Parent = this };
            Children.Add(childNode);

            RegisterChildForSearch(childNode);
            return childNode;
        }


        #region searching

        private ICollection<JsonNode<ProcMon>> ElementsIndex { get; set; }

        private void RegisterChildForSearch(JsonNode<ProcMon> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildForSearch(node);
        }

        public JsonNode<ProcMon> FindTreeNode(Func<JsonNode<ProcMon>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate);
        }

        #endregion


        #region iterating

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<JsonNode<ProcMon>> GetEnumerator()
        {
            yield return this;
            foreach (var directChild in Children)
            {
                foreach (var anyChild in directChild)
                    yield return anyChild;
            }
        }

        internal JsonNode<ProcMon> AddChild(JsonNode<ProcMon> two)
        {
            Children.Add(two);
            return this;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet.Tree
{
    internal class SampleSearching
    {
        static TreeNode<string> treeRoot = SampleData.GetSet1();
        static TreeNode<string> found = treeRoot.FindTreeNode(node => node.Data != null && node.Data.Contains("210"));
    }
}

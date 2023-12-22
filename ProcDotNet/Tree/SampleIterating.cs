using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet.Tree
{
    internal class SampleIterating
    {
        public static void MainTest()
        {
            TreeNode<string> treeRoot = SampleData.GetSet1();
            foreach (TreeNode<string> node in treeRoot)
            {
                string indent = CreateIndent(node.Level);
                Console.WriteLine(indent + (node.Data ?? "null"));
            }
            Console.WriteLine();
        }

        private static String CreateIndent(int depth)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }
}

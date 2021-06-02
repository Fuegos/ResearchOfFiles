using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFiles
{
    class Node
    {
        public string sName { get; set; }

        private Node parentNode { get; set; }



        public Node(string sName, Node parentNode)
        {
            this.sName = sName;
            this.parentNode = parentNode;
        }
    }
}

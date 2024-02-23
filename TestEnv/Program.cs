using EZCodeLanguage;
using System.Diagnostics;

string code = File.ReadAllText("Code.ezcode");

#if false

EZCode ezcode = new EZCode();
EZCode.LineWithTokens[] tokens = ezcode.Tokenize(code);

// Build the tree
TreeNode root = TreeVisualizer.BuildTree(tokens.Select(x => (object)x));

// Print the tree
TreeVisualizer.PrintTree(root);

class TreeNode
{
    public object Value { get; set; }
    public List<TreeNode> Children { get; set; }

    public TreeNode(object value)
    {
        Value = value;
        Children = new List<TreeNode>();
    }

    public void AddChild(TreeNode child)
    {
        Children.Add(child);
    }
}

class TreeVisualizer
{
    public static TreeNode BuildTree(object item)
    {
        TreeNode root = new TreeNode(item);
        BuildSubtree(root, item);
        return root;
    }

    private static void BuildSubtree(TreeNode parent, object obj)
    {
        if (obj is IEnumerable<object> enumerable)
        {
            for (int i = 0; i < enumerable.Count(); i++)
            {
                object child = enumerable.ElementAt(i);
                TreeNode newNode = new TreeNode(child);
                parent.AddChild(newNode);

                newNode.Value = $"{(i < enumerable.Count() - 1 ? "├──" : "└──")} {child}";
                BuildSubtree(newNode, child);
            }
        }
        else if (obj != null && !obj.GetType().IsPrimitive && obj.GetType() != typeof(string))
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                object propertyValue = property.GetValue(obj);
                TreeNode newNode = new TreeNode($"{property.Name}: {propertyValue}");
                parent.AddChild(newNode);
                BuildSubtree(newNode, propertyValue);
            }
        }
    }

    public static void PrintTree(TreeNode node, string indent = "")
    {
        Console.WriteLine(indent + node.Value);

        for (int i = 0; i < node.Children.Count; i++)
        {
            PrintTree(node.Children[i], indent + (i < node.Children.Count - 1 ? "│   " : "    "));
        }
    }
}

#else
Stopwatch stopwatch = Stopwatch.StartNew();
Tokenizer tokenizer = new Tokenizer();
Tokenizer.LineWithTokens[] tokens = tokenizer.Tokenize(code);
List<Tokenizer.TokenType[]> tokenTypes = [];
for (int i = 0; i < tokens.Length; i++)
{
    tokenTypes.Add(tokens[i].Tokens.Select(x => x.Type).ToArray());
}
string tokenString = "";
for (int i = 0; i < tokenTypes.Count; i++)
{
    for (int j = 0; j < tokenTypes[i].Length; j++)
    {
        tokenString += tokenTypes[i][j].ToString() + " ";
    }
    tokenString += "\n";
}

stopwatch.Stop();
long Omili = stopwatch.ElapsedMilliseconds;
Console.WriteLine(code + "\n--------------------\n"
    //+ tokenString + "--------------------\n"
    );

stopwatch.Restart();

Interpreter interpreter = new Interpreter("C:/Base.test", tokenizer);
interpreter.Interperate();


stopwatch.Stop();
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine("\n--------------------\n" + "Tokenize Miliseconds:" + Omili.ToString() + "\nInterperate Miliseconds:" + mili.ToString() + "\nOverall Miliseconds:" + (Omili + mili).ToString());
#endif
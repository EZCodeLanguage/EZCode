using EZCodeLanguage.Tokenizer;
using System.Diagnostics;

string code = """
    make var {NAME} => var {NAME} new :
    static class reflect {
        static method do : @str:does {
            runexec => 'does'
        }
        static method any : @str:does => @var {
            return runexec => 'does'
        }
        static method int : @str:does => @int {
            return runexec => 'does'
        }
        static method str : @str:does => @str {
            return runexec => 'does'
        }
        static method float : @str:does => @float {
            return runexec => 'does'
        }
        static method bool : @str:does => @bool {
            return runexec => 'does'
        }
    }
    class var {
        explicit params {val} => set : val
        undefined Value
        method set : val {
            Value = val
        }
        get => @str {
            return reflection str : EZCodeLanguage.EZHelp.String.Parse ~> 'Value'
        }
        get => @int {
            return reflection int : EZCodeLanguage.EZHelp.Int.Parse ~> 'Value'
        }
        nocol method = : val {
            if val is @int : Value => AsInt : val
            elif val is @str : Value => AsStr : val
        }
        static method AsInt : val => @int {
            return reflection int : EZCodeLanguage.EZHelp.Int.Parse ~> 'val'
        }
        static method AsStr : val => @str {
            return reflection int : EZCodeLanguage.EZHelp.String.Parse ~> 'val'
        }
        nocol method == : val => @bool {
            return reflection bool : EZCodeLanguage.EZHelp.Bool.Equals ~> 'Value', 'val'
        }
    }
    semi class str {
        explicit typeof => EZCodeLanguage.EZCode.DataType("string")
        explicit watch ""(.*? {text})"" => set : text
        explicit watch .*?'(.* {text})'.*? => set : text
        var Value \!
        method set : text {
            Value => format : text
        }
        static method format : @str:text => @str {
            return reflection str : EZCode.EZHelp.String.Format ~> 'text'
        }
    }
    class int {
        explicit typeof => EZCodeLanguage.EZCode.DataType("int")
        var Value 0
        method set : val {
            Value = val
        }
    }
    class bool {
        explicit typeof => EZCodeLanguage.EZCode.DataType("bool")
        var Value false
        method set : val {
            Value = val
        }
    }
    static ontop class ONTOP { 
        nocol method print : @str:text {
            reflection do : EZCodeLanguage.EZHelp.Print ~> 'text'
        }
    }

    method Main {
        var index 10
        var text ""this\_is\_some\_text""
        if index == 10 {
            print TEXT='text'
        }
    }
    """;

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
Tokenizer ezcode = new Tokenizer();
Tokenizer.LineWithTokens[] tokens = ezcode.Tokenize(code);
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
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine("Miliseconds:" + mili.ToString() + "\n--------------------\n\n\n--------------------\n" +
    code + "\n--------------------\n" +
    tokenString
    );

#endif
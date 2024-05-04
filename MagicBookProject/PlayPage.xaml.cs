using MagicBookProject.ViewModel;
//using MetalPerformanceShaders;

namespace MagicBookProject;


public partial class PlayPage : ContentPage
{

    public class Node
    {
        public int index;
        public List<string> text;
        public int background;
        public string question;
        public string text_1, text_2;
        public Node left;
        public Node right;

        public Node(int i, string q, List<string> t, string t1, string t2, int b)
        {
            index = i;
            question = q;
            text = new List<string> { };
            foreach (string s in t)
            {
                text.Add(s);
            }
            background = b;
            text_1 = t1;
            text_2 = t2;
            left = null;
            right = null;
        }
        ~Node()
        {
            text.Clear();
        }
    }

    public class BinaryTree
    {
        private Node root;

        public BinaryTree()
        {
            root = null;
        }

        public void AddNode(int index, string question, List<string> text, string text_1, string text_2, int background)
        {
            root = AddRec(root, index, question, text, text_1, text_2, background);
        }

        private Node AddRec(Node root, int index, string question, List<string> text, string text_1, string text_2, int background)
        {
            if (root == null)
            {
                root = new Node(index, question, text, text_1, text_2, background);
                return root;
            }

            if (index < root.index)
            {
                root.left = AddRec(root.left, index, question, text, text_1, text_2, background);
            }
            else if (index > root.index)
            {
                root.right = AddRec(root.right, index, question, text, text_1, text_2, background);
            }

            return root;
        }

        public Node FindNode(int index)
        {
            return FindNodeRec(root, index);
        }

        private Node FindNodeRec(Node root, int index)
        {
            if (root == null || root.index == index)
            {
                return root;
            }

            if (index < root.index)
            {
                return FindNodeRec(root.left, index);
            }

            return FindNodeRec(root.right, index);
        }

        private void ClearTree(Node node)
        {
            if (node == null)
                return;

            ClearTree(node.left);
            ClearTree(node.right);

            node = null;
        }

        public void Clear()
        {
            ClearTree(root);
            root = null;
        }
    }
    BinaryTree TreeStory = new BinaryTree();
    bool lever = true;
    public static int storyIndex = 0;

    public async void WriteTextToFile(string text, string targetFileName)
    {
        // Write the file content to the app data directory  
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
        using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);
        await streamWriter.WriteAsync(text);
    }

    public async Task<string> ReadTextFile(string filePath)
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        using StreamReader reader = new StreamReader(fileStream);

        return await reader.ReadToEndAsync();
    }
    async void InitTreeFromFile()
    {
        string Data = await ReadTextFile("Story.txt");
        string[] DataFromFile = Data.Split('\n');

        List<string> Text= new List<string> { };
        string Text_1 = "", Text_2 = "", Question="";
        int Index = 0, Background = 0;

        for (int i=0; i<DataFromFile.Length; i+=3)
        {
            int.TryParse(DataFromFile[i], out Index);
            int.TryParse(DataFromFile[i+1], out Background);
            i += 2;
            while (DataFromFile[i] != "==\r")
            {
                Text.Add(DataFromFile[i]);
                i++;
                
            }
            i++;
            Question = DataFromFile[i];
            string[] Elections = DataFromFile[i+1].Split('/');
            Text_1 = Elections[0];
            Text_2 = Elections[1];
            TreeStory.AddNode(Index, Question, Text, Text_1, Text_2, Background);
            Text.Clear();
        }
    }
    /////////////////////////////////////
    public static double SpeedText = 0.5;
    public PlayPage(PlayViewModel vm)
	{
        
        InitTreeFromFile();
        InitializeComponent();
		BindingContext = vm;
 
	}

    private void Ending()
    {
        return;
    }

    int index = 0;
    int status = 0;
    private async void AnimatedText(Label text) //status: 0 - text, 1-question
	{
        Node node = TreeStory.FindNode(storyIndex);
        string downloadText;
        if (node.text.Count == 0)
        {
            status = 1;
        }
        if (status == 0)
        {
            downloadText = node.text[index];
            ChoiceLeft.IsVisible = false;
            ChoiceLeft.IsEnabled = false;
            ChoiceRight.IsVisible = false;
            ChoiceRight.IsEnabled = false;
        }
        else
        {
            downloadText = node.question;
            ChoiceLeft.IsVisible = true;
            ChoiceLeft.IsEnabled = true;
            ChoiceRight.IsVisible = true;
            ChoiceRight.IsEnabled = true;
            ChoiceLeft.Text = node.text_1;
            ChoiceRight.Text = node.text_2;
        }

        if (lever)
		{
			text.Text = string.Empty;
            foreach (char c in downloadText)
            {
                if (lever)
                    lever = false;
                text.Text += c;
                await Task.Delay((int)(100 * SpeedText));
            }
            if (status == 0)
            {
                if (node.text.IndexOf(downloadText) + 1 == node.text.Count)
                {
                    index = 0;
                    status = 1;
                }
                else
                    index++;
                lever = true;
            }
            else
            {
                status = 0;
                storyIndex++;
                if (TreeStory.FindNode(storyIndex) == null)
                {
                    Ending();
                    storyIndex--;
                }
            }
        }
    }
	private void OnImageTapped(object sender, EventArgs e)
	{
		if (sender is Image image)
		{
			var text = (Label)image.Parent.FindByName("MainText");
			AnimatedText(text);
		}
	}

    private void leftChoiceClicked(object sender, EventArgs e)
    {
        lever = true;
        storyIndex = 1;
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/save.txt"))
            File.Create($"{mainDir}/save.txt").Close();
        WriteTextToFile(storyIndex.ToString(), "save.txt");
        var text = (Label)MainText;
        AnimatedText(text);
    }

    private void rightChoiceClicked(object sender, EventArgs e)
    {
        storyIndex = -1;
        lever = true;
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/save.txt"))
            File.Create($"{mainDir}/save.txt").Close();
        WriteTextToFile(storyIndex.ToString(), "save.txt");
        var text = (Label)MainText;
        AnimatedText(text);
    }
}
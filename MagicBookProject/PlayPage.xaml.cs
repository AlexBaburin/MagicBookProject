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
        public int character;
        public string text_1, text_2;
        public Node? left;
        public Node? right;

        public Node(int i, string q, List<string> t, int c, string t1, string t2, int b)
        {
            index = i;
            question = q;
            text = new List<string> { };
            foreach (string s in t)
            {
                text.Add(s);
            }
            character = c;
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
        private Node? root;

        public BinaryTree()
        {
            root = null;
        }

        public void AddNode(int index, string question, List<string> text, int character, string text_1, string text_2, int background)
        {
            root = AddRec(root, index, question, text, character, text_1, text_2, background);
        }

        private Node AddRec(Node? root, int index, string question, List<string> text, int character, string text_1, string text_2, int background)
        {
            if (root == null)
            {
                root = new Node(index, question, text, character, text_1, text_2, background);
                return root;
            }

            if (index < root.index)
            {
                root.left = AddRec(root.left, index, question, text, character, text_1, text_2, background);
            }
            else if (index > root.index)
            {
                root.right = AddRec(root.right, index, question, text, character, text_1, text_2, background);
            }

            return root;
        }

        public Node? FindNode(int index)
        {
            return FindNodeRec(root, index);
        }

        private Node? FindNodeRec(Node? root, int index)
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

        private void ClearTree(Node? node)
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
        FileStream fileStream = File.Open(targetFile, FileMode.Open);
        // Write the file content to the app data directory 
        fileStream.SetLength(0);
        fileStream.Close();

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
        string Data = await ReadTextFile("evening.txt");
        string[] DataFromFile = Data.Split('\n');

        List<string> Text= new List<string> { };
        string Text_1 = "", Text_2 = "", Question="";
        int Index = 0, Background = 0, Character = 0;

        for (int i=0; i<DataFromFile.Length; i+=3)
        {
            int.TryParse(DataFromFile[i], out Index);
            int.TryParse(DataFromFile[i + 1], out Background);
            int.TryParse(DataFromFile[i + 2], out Character);
            i += 3;
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
            TreeStory.AddNode(Index, Question, Text, Character, Text_1, Text_2, Background);
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
        characterNames.Add("");
        characterNames.Add("Соседка");
        characterNames.Add("Друг");
        BackgroundImage.IsVisible = true;
        PanelImage.IsVisible = true;
    }

    private void Ending()
    {
        BackgroundImage.Aspect = Aspect.AspectFill;
        BackgroundImage.HorizontalOptions = LayoutOptions.Start;
        if (storyIndex == 13)
            BackgroundImage.Source = "end_1.png";
        else if (storyIndex == 36)
            BackgroundImage.Source = "end_2.png";
        else if (storyIndex == -1)
            BackgroundImage.Source = "end_3.png";
        CharacterImage.IsVisible = false;
        PanelImage.IsVisible = false;
        PanelImage.IsEnabled = false;
        NameImage.IsVisible = false;
        MainText.IsVisible = false;
        NameText.IsVisible = false;

        ChoiceRight.IsVisible = false;
        ChoiceLeft.IsVisible = false;
        ChoiceRight.IsEnabled = false;
        ChoiceLeft.IsEnabled = false;
        index = 0;
        storyIndex = 0;
        WriteTextToFile(storyIndex.ToString(), "save.txt");
    }

    int index = 0;
    int status = 0;
    bool leverlever = false;
    bool buttonFlag = true;
    int counter = 0;
    int prevCharacter = 0;
    List<string> characterNames = new List<string>();
    private async void AnimatedImage(bool direction) //false apper, true disappear
    {
        CharacterImage.Opacity = 0;
        if (!direction)
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(1);
                CharacterImage.Opacity = i / 100.0;
            }
        else
            for (int i = 100; i >= 0; i--)
            {
                await Task.Delay(1);
                CharacterImage.Opacity = i / 100.0;
            }
    }
    private async void AnimatedText(Label text) //status: 0 - text, 1-question
	{
        Node? node = TreeStory.FindNode(storyIndex);
        string downloadText;
        string characterName = characterNames[node.character];
        if (status >= 3)
            status = 0;
        if (node.character != 0)
        {
            CharacterImage.IsVisible = true;
            CharacterImage.Source = $"character_{node.character.ToString()}";
        }
        else
            CharacterImage.IsVisible = false;

        if (node.text_1 == "Далее" && (node.text_2 == "Далее\r" || node.text_2 == "Далее") && status > 0)
        {
            Ending();
            return;
        }
        if (node.text_1 == " " && status > 0)
        {
            storyIndex = node.left.index;
            status = 0;
            AnimatedText(text);
            return;
        }
        if (prevCharacter != node.character)
        {
            prevCharacter = node.character;
            if (node.character != 0)
            {
                AnimatedImage(false);
            }
        }
        if (node.background != 0)
            BackgroundImage.Source = $"bg_{node.background.ToString()}";
        if (characterName == "")
        {
            NameText.IsVisible = false;
            NameImage.IsVisible = false;
        }
        else
        {
            NameText.IsVisible = true;
            NameImage.IsVisible = true;
            NameText.Text = characterName;
        }
        if (node.text.Count == 0 && status == 0)
        {
            status = 1;
        }
        if (status == 0)
        {
            if (counter == 0)
                leverlever = true;
            downloadText = node.text[index];
            if(downloadText=="Я проснулся.\r" && storyIndex==0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Flashlight.TurnOnAsync();
                    await Task.Delay(10);
                    Flashlight.TurnOffAsync();
                }
                Vibration.Vibrate(100);
            }
            ChoiceLeft.IsVisible = false;
            ChoiceLeft.IsEnabled = false;
            ChoiceRight.IsVisible = false;
            ChoiceRight.IsEnabled = false;
            counter++;
        }
        else
        {
            downloadText = node.question;
            if (storyIndex == 14 || storyIndex == 38)
            {
                ChoiceLeft.IsVisible = true;
                ChoiceLeft.IsEnabled = true;

                ChoiceLeft.Text = node.text_1;
            }
            else
            {
                ChoiceLeft.IsVisible = true;
                ChoiceLeft.IsEnabled = true;
                ChoiceRight.IsVisible = true;
                ChoiceRight.IsEnabled = true;
                ChoiceLeft.Text = node.text_1;
                ChoiceRight.Text = node.text_2;
            }
        }
        if (leverlever)
        {
            lever = true;
            leverlever = false;
        }

        if (lever)
		{
            buttonFlag = false;
			text.Text = string.Empty;
            foreach (char c in downloadText)
            {
                if (lever)
                {
                    lever = false;
                }
                text.Text += c;
                await Task.Delay((int)(10 * SpeedText));
            }
            if (status == 0)
            {
                if (node.text.IndexOf(downloadText) + 1 == node.text.Count)
                {
                    index = 0;
                    counter = 0;
                    status = 1;
                    leverlever = true;
                }
                else
                    index++;
                lever = true;
            }
            else
            {
                status++;
            }

            buttonFlag = true;
        }
    }
	private async void OnImageTapped(object sender, EventArgs e)
	{
		if (sender is Image image)
		{
			var text = (Label)image.Parent.FindByName("MainText");
			AnimatedText(text);
		}
	}

    private void leftChoiceClicked(object sender, EventArgs e)
    {
        status++;
        if (!buttonFlag)
            return;
        leverlever = true;
        Node? node = TreeStory.FindNode(storyIndex).left;
        if (node != null)
            storyIndex = node.index;
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/save.txt"))
            File.Create($"{mainDir}/save.txt").Close();
        WriteTextToFile(storyIndex.ToString(), "save.txt");
        var text = (Label)MainText;
        AnimatedText(text);
        Vibration.Vibrate(100);
    }

    private void rightChoiceClicked(object sender, EventArgs e)
    {

        status++;
        if (!buttonFlag)
            return;
        leverlever = true;
        Node? node = TreeStory.FindNode(storyIndex).right;
        if (node != null)
            storyIndex = node.index;
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/save.txt"))
            File.Create($"{mainDir}/save.txt").Close();
        WriteTextToFile(storyIndex.ToString(), "save.txt");
        var text = (Label)MainText;
        AnimatedText(text);
        Vibration.Vibrate(100);
    }
}
using Newtonsoft.Json.Linq;
using System.Security.Principal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace modinstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Sound);
            player.Play();
        }

        private void DownloadForge()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods");
            string profileName = textBox1.Text;
            string destinationPathOLD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "essential_mod\\forge\\1.20.1", profileName, "mods");
            string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", profileName, "mods");

            CreateProfile(profileName);

            //CreateMinecraftProfile(profileName);

            if(CreateProfile(profileName)) {
                if (checkBox1.Checked)
                {
                    foreach (string filePath in Directory.GetFiles(sourcePath))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string destinationFilePath = Path.Combine(destinationPathOLD, fileName);

                        File.Move(filePath, destinationFilePath, true);
                    }
                }
                else
                {
                    foreach (string filePath in Directory.GetFiles(sourcePath))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string destinationFilePath = Path.Combine(destinationPath, fileName);

                        File.Move(filePath, destinationFilePath, true);
                    }
                }

                MessageBox.Show("Profile created successfully!");
            }
        }

        private Boolean CreateProfile(string profileName)
        {
            //Find .minecraft Path
            string dotMinecraftPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");

            //Create DVSSMP Directory in .mincraft Path
            System.IO.Directory.CreateDirectory(Path.Combine(dotMinecraftPath, profileName));

            //Create Mods folder
            System.IO.Directory.CreateDirectory(Path.Combine(dotMinecraftPath, profileName, "mods"));



            System.IO.Directory.CreateDirectory(Path.Combine(dotMinecraftPath, "versions", "1.20.1-forge-47.2.0-wrapper"));
            string versionDir = Path.Combine(dotMinecraftPath, "versions", "1.20.1-forge-47.2.0-wrapper");

            


            foreach (string filePath in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forgeWrapper1.20.1")))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationFilePath = Path.Combine(versionDir, fileName);

                File.Copy(filePath, destinationFilePath, true);
            }

            //Move all DVSSMP files to the directory
            string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DVSSMP");
            string destinationPath = Path.Combine(dotMinecraftPath, profileName);
            foreach (string filePath in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationFilePath = Path.Combine(destinationPath, fileName);

                File.Copy(filePath, destinationFilePath, true);
            }

            
            string launcherProfilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "launcher_profiles.json");

            if (!File.Exists(launcherProfilesPath))
            {
                MessageBox.Show("Minecraft launcher_profiles.json not found!");
                return false;
            }


            // Read existing launcher_profiles.json into JObject
            string launcherProfilesJson = File.ReadAllText(launcherProfilesPath);
            JObject profiles = JObject.Parse(launcherProfilesJson);

            // Create a new profile entry
            JObject newProfile = new JObject(
                new JProperty("created", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                new JProperty("gameDir", Path.Combine(dotMinecraftPath, profileName)),
                new JProperty("javaArgs", "-Xmx5G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M"),
                new JProperty("lastUsed", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                new JProperty("lastVersionId", "1.20.1-forge-47.2.0-wrapper"),
                new JProperty("name", profileName),
                new JProperty("type", "custom")
            );

            // Add the new profile to the profiles object
            profiles["profiles"][profileName] = newProfile;

            // Serialize JObject back to JSON formatted string
            string updatedJson = profiles.ToString();

            // Write the updated JSON back to launcher_profiles.json
            File.WriteAllText(launcherProfilesPath, updatedJson);

            return true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}

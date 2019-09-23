namespace ConsoleMusic
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMusic tc = new ConsoleMusic();
            tc.ReadFile("sound.cmu");
            tc.Play();
        }
    }
}

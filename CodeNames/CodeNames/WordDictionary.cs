using System.Collections.ObjectModel;

namespace CodeNames.Logic
{
    public class WordDictionary : ObservableCollection<Word>
    {
        public WordDictionary()
        {
            NewWord();
        }

        string[] GetDefaultWords()
        {
            return ("Bucket,Sack,Leaf,Family,Relative,Freak,Chicken,Adventure,Heated,Sausage,Bird," +
                    "Shark,Lone,Chilli,Point,Toe,Light,Greet,Rule,Fear,Mennonite,Action,Drive,Leave," +
                    "Kobe,Christmas,Jolly,Lit,Dope,Drug,Shoe,Vaccines,Mom,Conspiricy,Politicion,Moon," +
                    "Adolf,Notify,Ring,Doornob,Amish,Plug,Painting,Left,Moose,Ear,Lace,Todo," +
                    "Thread,Corner,Peru,Wall,Kindle,Nail,Code,Hike,Learn,Far,Fair,Speak,Climber," +
                    "Surfer,Loud,Vent,High,Hair dresser,Chef,Coat,Shady,Fire,Bunz,Flax,Running," +
                    "Light bulb,School,Tech,Tinker bell,Sendy,Lamp,Mirror,Battle,Argument,Nice," +
                    "Wonderful,Button,Constelation,Ham,Cheese,Back,Chopper,Air,Sword,Train,Instagram," +
                    "Spotify,Boy,Flower,Coal,Hand,Boot,Steam,Tripod,Key,Round,Open,YWAM,Time,Flash," +
                    "School,Mustard,Hotrod,Movie,Man,Napolean,Word,Myth,Psych,Blue,France,Clock," +
                    "Monopoly,Hummus,Savage,Flert,Flee,Espresso,Katana,Plain,Beard,Julio Roberts," +
                    "Plank,Skin,Hair,Organ,Thor,Metal,Screw,Toast,Wax,Fire,Mom,Backpack,Watch," +
                    "Fur,Slang,Baby,Arm,War,Train,Drink,Viking,Iron,Barn,Bear,Mushroom,Acid,Assassin," +
                    "Turkey,Write,Plate,Pants,Pizza,Gladius,Drug,Will Smith,Plank,Wood,Plain,Prince," +
                    "Antler,Play,Blast,Knight,Shoot,Flag,Sea,Holmes County,Trash,Glass,Europe,Plaid," +
                    "Hairball,Sick,Grease,Universe,Climb,Ball,Molasses,Gladiator,Orb,Cute,Row,Watch," +
                    "Deoderant,Russia,Blanket,Makeup,Phone,Dictionary,Monkey,Banana,Moccasins").Split(',');
        }

        public void AddDefaultWords()
        {
            var words = GetDefaultWords();
            var randomizer = new Randomizer(words.Length);
            for (int i = 0; i < 30; i++)
            {
                Add(new Word(words[randomizer.Next()]));
            }
        }

        public void NewWord()
        {
            Add(new Word(""));
        }
    }
}

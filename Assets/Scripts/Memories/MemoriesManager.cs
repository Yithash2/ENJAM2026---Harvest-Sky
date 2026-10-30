using System.Collections.Generic;
using UnityEngine;

public class MemoriesManager : MonoBehaviour
{
    public static MemoriesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public Dictionary<string, string> LoreTexts = new Dictionary<string, string>
    {
        {"L1-1", "To the ----- who stole my spear, give it back or I'm setting your damned house on fire by the next moon!"},
        {"L1-2", "Tomb of Flavius, he who inspired fear and respect in equal measure. Your ruthlessness will be ---embered by generations to come\nTellusian law compells you to attend the weekly city assembly, according to decrees 6, 7 and --(unlegible)"},
        {"L1-3", "Tellusia: Proud and Stro--(unlegible)"},
        {"L2-1", "fresh bread at this address everyday, 10 gold Flecks only"},
        {"L2-2", "Cervius, you bastard! I saw you steal my cat! may your crops wither and die!"},
        {"L2-3", "Martell, beloved father and eternal friend to all (unlegible)"},
        {"L2-4", "Agernum: To exist is to Nurture\nMay the sun shine on whoever reads this and ensure them a generous and bountiful harvest "},
        {"L3-1", "Sylvos: The power of the tree and strength of the root\nThe rustle of the leaves forever tie us to the soil, tethered down by the reminder of its nourishment."},
        {"L3-2", "Here lie Rosa Alba and Quercus Sempervirens, claimed by the soil and forever joined in the eternal embrace of the trees"},
        {"L3-3", "Deposit your letters and notes in the box below. Add your recipient's specific location on the lower lands for easier distribution."},
        {"L4-1", "Gone exploring the lower depths, do not expect me to be back for supper."},
        {"L4-2", "I saw the great --lestial beast from the great Northern Tower this past night. No one believes me but I know what I saw!"},
        {"L4-3", "here lies the mortal shell of Adonia, flown up to the gods and forever missed below."},
        {"L4-4", "Cirrus: Great city of the clouds\n(barely legible) When the clouds part and the --- blows, the face of the gods most ardently glows "},
        {"L5-1", "Book club meeting at sundown. The --cret location will be divulged by the South-East passing bluebird flock."},
        {"L5-2", "Sofia, we will meet ag--- when the great city in the sky opens itself to us. Wait for me there. - your Dreya"},
        {"L5-3", "Here lies Lunaria, for whom the clouds have parted and and ascended her to the higher realms"},
        {"L5-4", "Celestia: Ever closer to the gods\nAscension is near- do not relent in your prayers and keep your gaze fixed upwards "}
    };
}

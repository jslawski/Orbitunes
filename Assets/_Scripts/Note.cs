using UnityEngine;

/* * * 
 * The Note class handles the behavior of any object that emits sound to the beat of the Metronome.  
 * If the note is static, it will always play the same pitch.
 * If the note is dynamic, it will change it's pitch depending on its distance from the center
 * This class should ONLY handle calculating the pitch and playing an individual note for an individual asteroid
 * The NoteParent class should handle the overall phrase and decide when a note should/shouldn't be played
 *  *  */
public class Note : MonoBehaviour {
	public string noteName;
	public AudioSource noteAudio;

    private float curDistanceFromStar = 0f;
	private float maxDistanceFromStar = 35f;
	private int curPitchIndex = 0;

    //Note to self: When you declare a constructor, you need to set ALL of the values of the component.  Otherwise they are set to 0.
    /*public Note(string noteName, AudioSource noteAudio, ushort phraseNumber, int beatsPerPhrase)
	{
		this.noteName = noteName;
		this.noteAudio = noteAudio;
		this.phraseNumber = phraseNumber;
		this.beatsPerPhrase = beatsPerPhrase;
	}*/

    /*public void SetupNote(bool isDynamic)
	{
		PitchManager.instance.AddAudioSource(this.noteAudio);
	}*/

    public delegate void NoteDestroyed(Note thisNote);
    public event NoteDestroyed OnNoteDestroyed;

    private void OnDestroy()
    {
        if (this.OnNoteDestroyed != null)
        {
            this.OnNoteDestroyed(this);
        }
    }

    public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}

	private void GetCurrentPitchIndex(GameObject leadingAsteroid)
	{
		this.curDistanceFromStar = this.GetDistance(Vector2.zero, leadingAsteroid.transform.position);

		this.curPitchIndex = Mathf.RoundToInt((this.curDistanceFromStar / this.maxDistanceFromStar) * (PitchManager.notesInScale - 1));

        Debug.LogError("Current Pitch Index:" + this.curPitchIndex);

        if (this.curPitchIndex > PitchManager.notesInScale - 1) 
		{
			this.curPitchIndex = PitchManager.notesInScale - 1;
		}
	}

	//Pitch changes depending on distance to the star
	public void PlayDynamicNote(GameObject leadingAsteroid)
	{
	    this.GetCurrentPitchIndex(leadingAsteroid);

        this.noteAudio.pitch = PitchManager.instance.cMajorScale[this.curPitchIndex];
		this.noteAudio.PlayScheduled(Metronome.nextBeatTime);
	}

	//Pitch is static throughout its lifecycle
	public void PlayStaticNote()
	{
		this.noteAudio.PlayScheduled(Metronome.nextBeatTime);
	}
}

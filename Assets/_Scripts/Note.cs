using UnityEngine;

/* * * 
 * The Note class handles the behavior of any object that emits sound to the beat of the Metronome.  
 * If the note is static, it will always play the same pitch.
 * If the note is dynamic, it will change it's pitch depending on its distance from the center
 * This class should ONLY handle calculating the pitch and playing an individual note for an individual asteroid
 * The NoteParent class should handle the overall phrase and decide when a note should/shouldn't be played
 *  *  */
public class Note : MonoBehaviour {
	public AudioSource[] noteAudio;

    private float curDistanceFromStar = 0f;
	private float maxDistanceFromStar = 35f;
	private int curPitchIndex = 0;

    public delegate void NoteDestroyed(Note thisNote);
    public event NoteDestroyed OnNoteDestroyed;

    private Phrase mainPhrase;
    private int audioSourceIndex = 0;

    private void Awake()
    {
        noteAudio = GetComponents<AudioSource>();
    }

    private void OnDestroy()
    {
        if (this.OnNoteDestroyed != null)
        {
            this.OnNoteDestroyed(this);
        }

        Metronome.OnStep -= this.PlayDynamicNote;
        Metronome.OnStep -= this.PlayStaticNote;
    }

    public void SetupNote(ushort phraseNumber, int beatsPerPhrase, bool isDynamic)
    {
        this.mainPhrase = new Phrase(phraseNumber, beatsPerPhrase);

        if (isDynamic == true)
        {
            Metronome.OnStep += this.PlayDynamicNote;
            this.PlayDynamicNote();
        }
        else
        {
            Metronome.OnStep += this.PlayStaticNote;
            this.PlayStaticNote();
        }
    }

    public void AssignAudioClip(AudioClip clip)
    {
        for (int i = 0; i < this.noteAudio.Length; i++)
        {
            this.noteAudio[i].clip = clip;
        }
    }

    public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}

	private void GetCurrentPitchIndex()
	{
		this.curDistanceFromStar = this.GetDistance(Vector2.zero, this.gameObject.transform.position);

		this.curPitchIndex = Mathf.RoundToInt((this.curDistanceFromStar / this.maxDistanceFromStar) * (PitchManager.notesInScale - 1));

        if (this.curPitchIndex > PitchManager.notesInScale - 1) 
		{
			this.curPitchIndex = PitchManager.notesInScale - 1;
		}
	}

	//Pitch changes depending on distance to the star
	public void PlayDynamicNote()
	{
        if (Metronome.metronomeStarted == false)
        {
            return;
        }

        if (this.mainPhrase.ShouldPlayAtStep() == true)
        {
            this.GetCurrentPitchIndex();

            this.noteAudio[this.audioSourceIndex].pitch = PitchManager.instance.cMajorScale[this.curPitchIndex];
            this.noteAudio[this.audioSourceIndex].PlayScheduled(Metronome.nextBeatTime);
            this.audioSourceIndex = (this.audioSourceIndex + 1) % this.noteAudio.Length;
        }

        this.mainPhrase.IncrementStep();
	}

	//Pitch is static throughout its lifecycle
	public void PlayStaticNote()
	{
        if (Metronome.metronomeStarted == false)
        {
            return;
        }

        if (this.mainPhrase.ShouldPlayAtStep() == true)
        {
            this.noteAudio[this.audioSourceIndex].PlayScheduled(Metronome.nextBeatTime);
            this.audioSourceIndex = (this.audioSourceIndex + 1) % this.noteAudio.Length;
        }

        this.mainPhrase.IncrementStep();
	}
}

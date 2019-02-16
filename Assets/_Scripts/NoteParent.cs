using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///The NoteParent class is responsible for managing when notes are played for each individual asteroid.
///This is a necessary class in order to have an asteroid group play overlapping notes without clipping.
///Each note should be played individually per asteroid prefab, instead of the leading asteroid playing all of the notes.
/// </summary>
public class NoteParent : MonoBehaviour
{
    public List<Note> asteroidNotes;

    private GameObject leadingAsteroid;

    private int noteListIndex = 0;

    private Phrase mainPhrase;

    public void SetupNoteParent(Note initialNote, ushort phraseNumber, int beatsPerPhrase, bool isDynamic) 
    {
        this.asteroidNotes = new List<Note>();
        this.asteroidNotes.Add(initialNote);
        this.leadingAsteroid = initialNote.transform.parent.gameObject;
        //initialNote.OnNoteDestroyed += this.RemoveNoteFromList;

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

    private void OnDestroy()
    {
        Metronome.OnStep -= this.PlayDynamicNote;
        Metronome.OnStep -= this.PlayStaticNote;
        //PitchManager.instance.RemoveAudioSource(this.noteAudio);
    }

    public void AddNoteToList(Note noteToAdd)
    {
        this.asteroidNotes.Add(noteToAdd);
        //noteToAdd.OnNoteDestroyed += this.RemoveNoteFromList;
    }

    private void RemoveNoteFromList(Note noteToRemove)
    {
        this.asteroidNotes.Remove(noteToRemove);

        noteToRemove.OnNoteDestroyed -= this.RemoveNoteFromList;

        if (this.gameObject.transform.childCount <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void PlayDynamicNote()
    {
        if (Metronome.metronomeStarted == false)
        {
            return;
        }

        if (this.mainPhrase.ShouldPlayAtStep() == true)
        {
            //Check to make sure the asteroid hasn't been destroyed somehow
            if (this.asteroidNotes[this.noteListIndex] != null && this.leadingAsteroid != null)
            {
                this.asteroidNotes[this.noteListIndex].PlayDynamicNote(this.leadingAsteroid);
            }
            this.noteListIndex = (this.noteListIndex + 1) % this.asteroidNotes.Count;
        }

        this.mainPhrase.IncrementStep();
    }

    private void PlayStaticNote()
    {
        if (Metronome.metronomeStarted == false)
        {
            return;
        }

        if (this.mainPhrase.ShouldPlayAtStep() == true)
        {
            //Check to make sure the asteroid hasn't been destroyed somehow
            if (this.asteroidNotes[this.noteListIndex] != null)
            {
                this.asteroidNotes[this.noteListIndex].PlayStaticNote();
            }
            this.noteListIndex = (this.noteListIndex + 1) % this.asteroidNotes.Count;
        }

        this.mainPhrase.IncrementStep();
    }
}

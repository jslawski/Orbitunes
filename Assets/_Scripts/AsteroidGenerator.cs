using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/* * *
 * The AsteroidGenerator class should only handle the behavior of launching an asteroid
 * as well as its accompanying trailing asteroids.  Relies on AsteroidSelector in order to determine
 * which asteroid to launch
 * * */
public class AsteroidGenerator : MonoBehaviour
{
    public GameObject noteParentPrefab;

    private LaunchValues launchValues;
    private GameObject launchAsteroid;
    private Phrase phrase;
    private NoteParent currentNoteParent;

    private bool initialAsteroidGenerated = false;

    public void SetupAsteroidGenerator(LaunchValues launchValues)
    {
        this.launchValues = launchValues;
        this.launchAsteroid = AsteroidSelector.selectedAsteroid;

        PhraseMetadata asteroidPhrase = this.launchAsteroid.GetComponentInChildren<PhraseMetadata>();
        this.phrase = new Phrase(asteroidPhrase.phraseNumber, asteroidPhrase.beatsPerPhrase);

        Metronome.OnStep += this.LaunchAsteroid;
        GameManager.OnRestartButtonClicked += this.DestroyAsteroidGenerator;
    }

    private void OnDestroy()
    {
        Metronome.OnStep -= this.LaunchAsteroid;
        GameManager.OnRestartButtonClicked -= this.DestroyAsteroidGenerator;
    }

    private void SetupAsteroid(GameObject asteroid)
    {
        Asteroid newAsteroid = asteroid.GetComponent<Asteroid>();
        newAsteroid.isStationary = false;
        newAsteroid.SetupAsteroid(this.launchValues);
    }

    private void SetupNoteParent(GameObject asteroid)
    {
        GameObject newNoteParentObject = GameObject.Instantiate(this.noteParentPrefab, this.transform.position, new Quaternion()) as GameObject;

        this.currentNoteParent = newNoteParentObject.GetComponent<NoteParent>();
        PhraseMetadata metaData = asteroid.GetComponentInChildren<PhraseMetadata>();
        Note asteroidNote = asteroid.GetComponentInChildren<Note>();

        this.currentNoteParent.SetupNoteParent(asteroidNote, metaData);
    }

    private void AddNoteToNoteParent(GameObject asteroid)
    {
        this.currentNoteParent.asteroidNotes.Add(asteroid.GetComponentInChildren<Note>());
    }

	/*private void SetupNote(GameObject asteroid)
	{
		Note newNote = asteroid.GetComponentInChildren<Note>();
		newNote.SetupNote(true);
	}*/

	private void SetupParticlePulse(GameObject asteroid)
	{
		ParticlePulse particlePulse = asteroid.GetComponentInChildren<ParticlePulse>();
		PhraseMetadata asteroidPhrase = asteroid.GetComponentInChildren<PhraseMetadata>();
		particlePulse.SetupParticlePulse(asteroidPhrase.beatsPerPhrase, this.phrase.currentStep);
	}

	private void DestroyAsteroidGenerator()
	{
		Destroy(this.gameObject);
	}

	public void LaunchAsteroid()
	{
		//Stop generating asteroids once the phrase is complete
		if (this.phrase.currentStep >= (this.phrase.GetMaxStepCount() - 1))
		{
			Metronome.OnStep -= LaunchAsteroid;
			Destroy(this.gameObject);
		}

		if (this.phrase.ShouldPlayAtStep() == true)
		{
			GameObject asteroidObject = GameObject.Instantiate(this.launchAsteroid, this.launchValues.startPoint, new Quaternion()) as GameObject;
			this.SetupAsteroid(asteroidObject);

            //Only setup a note for the initial asteroid
            if (this.initialAsteroidGenerated == false)
            {
                this.SetupNoteParent(asteroidObject);

                //this.SetupNote(asteroidObject);
                this.initialAsteroidGenerated = true;

                AnalyticsEvent.Custom("Asteroid_Launched", new Dictionary<string, object> { { "Asteroid_Name", this.launchAsteroid.name }, { "Phrase_Number", this.phrase.phraseNumber },
                    { "Launch_Start_Point", this.launchValues.startPoint }, { "Launch_End_Point", this.launchValues.endPoint }, { "Launch_Direction", this.launchValues.curDirection },
                    { "Launch_Magnitude", this.launchValues.rawMagnitude } });
            }
            else
            {
                this.AddNoteToNoteParent(asteroidObject);
            }
            
			this.SetupParticlePulse(asteroidObject);
		}

		this.phrase.IncrementStep();

        
    }
}

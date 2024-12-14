using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimetableController : MonoBehaviour
{
	public GameObject H0;
	public GameObject H1;
	public GameObject H2;
	public GameObject H3;
	public GameObject H4;
	public GameObject H5;
	public GameObject H6;
	public GameObject H7;
	public GameObject H8;
	public GameObject H9;
	public GameObject H10;
	public GameObject H11;
	public GameObject H12;
	public GameObject H13;
	public GameObject H14;
	public GameObject H15;
	public GameObject H16;
	public GameObject H17;
	public GameObject H18;
	public GameObject H19;
	public GameObject H20;
	public GameObject H21;
	public GameObject H22;
	public GameObject H23;

	private List<GameObject> Hours;

	private void Awake()
	{
		Hours = new List<GameObject>() {
			H0, H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12, H13, H14, H15, H16, H17, H18, H19, H20, H21, H22, H23
		};
	}

	private void Update()
	{
		UpdateTimeTable();
	}

	void UpdateTimeTable()
	{
		int currentHour = PlayerCharacter.GetClock().GetHour();

		Dictionary<int, BuildingVariant> timetable = new();
		foreach ( var element in PlayerCharacter.GetBase().GetTimetable() ) {
			timetable[GetCorrectTimeDone(currentHour, element.Value)] = element.Key;
		}

        foreach (GameObject hour in Hours)
        {
			int keyHour = GetHourByHour(hour);

			if (keyHour >= currentHour && timetable.ContainsKey(keyHour)) SetText(timetable[keyHour], hour);
			else SetEmpty(hour);
        }
    }

	private GameObject GetHourByHour( int hour )
	{
		if ( hour >= 0 && hour < Hours.Count ) {
			return Hours[hour];
		}
		else {
			Debug.LogError( $"Invalid hour: {hour}. Hour must be between 0 and 23." );
			return null;
		}
	}

    private int GetHourByHour(GameObject gameObject)
    {
		for (int i = 0; i < Hours.Count; i++)
		{
			if (gameObject.Equals( Hours[i])) return i;
		}

		return 0;
    }

    private void SetText(BuildingVariant variant, GameObject gameObject)
	{
		if ( gameObject == null )
			return;

		gameObject.transform.Find( "UIText" ).GetComponent<TMP_Text>().text = CreateStringFromVariant( variant );
	}

	private void SetEmpty(GameObject gameObject)
	{
        if (gameObject == null)
            return;

		gameObject.transform.Find("UIText").GetComponent<TMP_Text>().text = "";

    }

	private string CreateStringFromVariant(BuildingVariant variant )
	{
		return $"{variant.GetName()} will be done";
	}

	private int GetCorrectTimeDone( int currentHour, int timeToComplete )
	{
		int hour = currentHour;
		while ( timeToComplete > 0 ) {
			hour = ( hour + 1 ) % 24;

			if ( hour >= 21 || hour <= 3 ) {
				continue;
			}

			timeToComplete--;
		}

		return hour;
	}

}

using UnityEngine;
using Newtonsoft.Json;

public class Clock
{
	private int DAYBREAK;
	private int DUSK;
	private int NIGHT;


    [JsonProperty]
    private int Hour;
    [JsonProperty]
    private int Minutes;

    [JsonProperty]
    private int Year;
    [JsonProperty]
    private int Month;
    [JsonProperty]
    private int Day;

	public Clock()
	{
		DAYBREAK = 4;
		DUSK = 20;
		NIGHT = 21;


        Hour = 5;
		Minutes = 0;

		Year = 1827;
		Month = 6;
		Day = 13;
	}

	public bool IsDayTime()
	{
		return Hour >= DAYBREAK && Hour < DUSK;
	}

	public bool IsDusk()
	{
		return Hour == DUSK;
	}

	public bool IsNight()
	{
        return Hour == NIGHT;
    }

	public int GetHour()
	{
		return Hour;
	}

	public int HourInMinutes( int hour )
	{
		return hour * 60;
	}

	public int GetMinute()
	{
		return Minutes;
	}

	public int GetTimeInMinutes()
	{
		return GetHour() * 60 + Minutes;
	}

	public int GetYear()
	{
		return Year;
	}

	public int GetNumericMonth()
	{
		return Month;
	}

	public string GetMonth()
	{
		return Month switch {
			1 => "January",
			2 => "February",
			3 => "March",
			4 => "April",
			5 => "May",
			6 => "June",
			7 => "July",
			8 => "August",
			9 => "September",
			10 => "October",
			11 => "November",
			12 => "December",
			_ => throw new System.Exception( "No corresponding month to stored value" ),
		};
	}

	public int GetDay()
	{
		return Day;
	}

	private void SetHour( int hour )
	{
		if ( hour >= 0 && hour <= 24 )
			Hour = hour;
	}

	private void SetMinutes( int minutes )
	{
		if ( minutes >= 0 && minutes <= 60 )
			Minutes = minutes;
	}

	private void SetDay( int day )
	{
		Day = day;
	}

	private void SetMonth( int month )
	{
		Month = month;
	}

	private void SetYear( int year )
	{
		Year = year;
	}

	private void IncrementYear()
	{
		++Year;
	}

	private void IncrementMonth()
	{
		if ( Month == 12 ) {
			IncrementYear();
			SetMonth( 1 );
		}
		else
			++Month;
	}

	private void IncrementDay()
	{
		if ( Day == 30 ) {
			IncrementMonth();
			SetDay( 1 );
		}
		else
			++Day;
	}

	public void ProgressToDusk()
	{
		SetHour( DUSK );
		SetMinutes( 0 );
	}

    public void ProgressToNight()
    {
        SetHour(NIGHT);
        SetMinutes(0);
    }

    public void ProgressToNextMorning()
	{
		IncrementDay();
		SetHour( DAYBREAK );
		SetMinutes( 0 );
		PlayerCharacter.AddIncome();
	}

	public bool AddMinutes( int minutes )
	{
		Minutes += minutes % 60;
		Hour += minutes / 60;
        return !(GetTimeInMinutes() >= HourInMinutes(DUSK));
	}

	public bool CanAddMinutes(int minutes)
	{
		return GetMinutesUntilDusk() >= minutes; 
	}

    public bool CanAddHours(int hours)
    {
        return CanAddMinutes(hours * 60);
    }

    public bool AddHours( int hours )
	{
		return AddMinutes( hours * 60 );
	}

	public bool AddHoursAndMinutes(int hours, int minutes)
	{
        return AddMinutes(hours * 60 + minutes);
    }

	public int GetMinutesUntilDusk()
	{
		return HourInMinutes( DUSK ) - GetTimeInMinutes();
	}

	public string TimeToString()
	{
		string minutes = GetMinute().ToString();
		if (minutes.Length == 1) minutes = "0" + minutes[0];
		return $"{GetHour()}:{minutes}";
    }

    public string DateToString()
    {
        return $"{GetYear().ToString()}, {GetMonth()} {GetDay()}.";
    }
}
